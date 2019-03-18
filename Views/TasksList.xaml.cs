using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TasksList.Models;

namespace TasksList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string SqlConnection = @"Data Source=DESKTOP-6KH69CQ; Initial Catalog=tasks; Integrated Security=SSPI";

        private const string NotCompleted = "Не выполнено заданий - ";

        private const string AllDone = "Все задачи выполнены";

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Models.TaskModel> Tasks { get; set; }

        public string _TasksStateString;

        public string TasksStateString
        {
            get
            {
                return _TasksStateString;
            }

            set
            {
                _TasksStateString = value;
                OnPropertyChanged("Content");
            }
        }

        private string _ForeGround;

        public string ForeGround
        {
            get
            {
                return _ForeGround;
            }

            set
            {
                _ForeGround = value;
                OnPropertyChanged("Foreground");
            }
        }

        private string _LabelBackGround;

        public string LabelBackGround
        {
            get
            {
                return _LabelBackGround;
            }

            set
            {
                _LabelBackGround = value;
                OnPropertyChanged("Background");
            }
        }

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<Models.TaskModel>();
            this.DataContext = this;
            var conn = new SqlConnection(SqlConnection);
            HasRows(conn);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var conn = new SqlConnection(SqlConnection);
            HasRows(conn);
        }

        void HasRows(SqlConnection conn)
        {
            using (conn)
            {
                SqlCommand command = new SqlCommand("get_tasks", conn);
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var task = new TaskModel();
                        task.Id = Convert.ToInt32(reader["id"]);
                        task.Content = reader["content"].ToString();
                        task.IsDone = Convert.ToBoolean(reader["is_done"]);
                        task.Date = Convert.ToDateTime(reader["dt"]).Date;
                        Tasks.Add(task);
                    }
                }
                reader.Close();
            }
            Change_TasksStateString();
        }

        private void Change_TasksStateString()
        {
            if (Tasks.Where(t => t.IsDone == false).Any())
            {
                _TasksStateString = NotCompleted;
                _LabelBackGround = "#Red";
            }
            else
            {
                _TasksStateString = AllDone;
                _LabelBackGround = "Green";
            }

            _ForeGround = "white";
        }

        static void DeleteTask(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("update_task", conn))
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
