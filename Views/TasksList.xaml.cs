using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TasksList
{
    public partial class TasksList : Window, INotifyPropertyChanged
    {
        private string SqlConnection = "Server=DESKTOP-6KH69CQ;Database=TasksList;Integrated Security=True ";

        private Interfaces.IDataRepository<Models.TaskModel> _repo;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Models.TaskModel> Tasks { get; set; }
        public ObservableCollection<Controls.TaskModelControl> TaskControlList { get; set; }

        public TasksList()
        {
            InitializeComponent();
            //this._repo
            //_repo.DataChanged += (s) => { Load(); };
            Tasks = new ObservableCollection<Models.TaskModel>();
            TaskControlList = new ObservableCollection<Controls.TaskModelControl>();
            this.DataContext = this;
            var conn = new SqlConnection(SqlConnection);
            HasRows(conn);
            //   Load();
        }

        private void Load()
        {
            Tasks.Clear();
            Tasks = _repo.GetAll();
            CreateControls();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaskControlList"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var model = new Models.TaskModel() { Content = MyTextBox.Text, IsDone = false };
            //_repo.Add(model);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var conn = new SqlConnection(SqlConnection);
            HasRows(conn);
            // Load();
        }

        private void CreateControls()
        {
            //TaskControlList.Clear();
            //foreach (var model in TasksList)
            //{
            //    var control = new Controls.TaskModelControl(model, _repo);
            //    TaskControlList.Add(control);
            //}
        }

        static void HasRows(SqlConnection conn)
        {
            using (conn)
            {
                SqlCommand command = new SqlCommand("GetAll_Tasks", conn);
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                DataTable schemaTable = reader.GetSchemaTable();

             //   Tasks = ref schemaTable;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //                      Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
        }

        static void DeleteTask(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("GettAll_Tasks", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Firstname", SqlDbType.VarChar);
                param1.Value = "my first name";

                // ... the rest params

                cmd.Parameters.Add(param1);

                // cmd.Parameters.Add(param2);....

                cmd.ExecuteNonQuery();
            }
        }

    }
}
