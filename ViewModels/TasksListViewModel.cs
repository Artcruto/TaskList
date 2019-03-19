using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TasksList.Models;

namespace TasksList.ViewModels
{
    class TasksListViewModel : INotifyPropertyChanged
    {
        private SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-6KH69CQ; Initial Catalog=tasks; Integrated Security=SSPI");

        private const string NotCompleted = "Не выполнено заданий - ";

        private const string AllDone = "Все задачи выполнены";

        private DateTime _chosenDate = DateTime.Now.Date;


        private ObservableCollection<TasksList.Models.TaskModel> _tasksList;

        private int _ItemIndex;

        private string _labelBackGround;

        private string _tasksStateString;

        private string _foreGround;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TasksList.Models.TaskModel> TasksList
        {
            get
            {
                return _tasksList;
            }

            set
            {
                _tasksList = value;
                OnPropertyChanged(nameof(TasksList));
            }
        }

        public DateTime ChosenDate
        {
            get
            {
                return _chosenDate;
            }

            set
            {
                _chosenDate = value;
                OnPropertyChanged(nameof(ChosenDate));
            }
        }

        public string TasksStateString
        {
            get
            {
                return _tasksStateString;
            }

            set
            {
                _tasksStateString = value;
                OnPropertyChanged(nameof(TasksStateString));
            }
        }

        public string ForeGround
        {
            get
            {
                return _foreGround;
            }

            set
            {
                _foreGround = value;
                OnPropertyChanged(nameof(ForeGround));
            }
        }

        public string LabelBackGround
        {
            get
            {
                return _labelBackGround;
            }

            set
            {
                _labelBackGround = value;
                OnPropertyChanged(nameof(LabelBackGround));
            }
        }


        public int ItemIndex
        {
            get
            {
                return _ItemIndex;
            }

            set
            {
                _ItemIndex = value;
                OnPropertyChanged(nameof(ItemIndex));
            }
        }


        public TasksListViewModel()
        {
            TasksList = new ObservableCollection<TasksList.Models.TaskModel>();
            TasksList.CollectionChanged += Tasks_CollectionChanged;
            _taskEditCommand = new DelegateCommand((s) => SyncTask());
            LoadTasks();
        }

        private void SyncTask()
        {
            if (ItemIndex == -1)
                return;
            var task = TasksList[ItemIndex];
            SyncTask(task);
        }

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    DeleteTask(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
            }
            Change_TasksState();
        }

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public DelegateCommand AddTask => new DelegateCommand((obj) => TasksList.Add(new TaskModel()));

        public void SyncTask(TaskModel task)
        {
            if (string.IsNullOrEmpty(task.Content))
                return;
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("update_task", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", task.Id);
                    cmd.Parameters.AddWithValue("@content", task.Content);
                    cmd.Parameters.AddWithValue("@is_done", task.IsDone);
                    cmd.Parameters.AddWithValue("@dt", ChosenDate);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                Change_TasksState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteTask(IList oldItems, int oldStartingIndex)
        {
            try
            {
                conn.Open();
                lock (oldItems.SyncRoot)
                {
                    foreach (var item in oldItems)
                    {
                        var task = item as TaskModel;
                        using (SqlCommand cmd = new SqlCommand("del_task", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id", task.Id);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadTasks()
        {
            using (SqlCommand cmd = new SqlCommand("get_tasks_by_date", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dt", ChosenDate);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var task = new TaskModel();
                        task.Id = Convert.ToInt32(reader["id"]);
                        task.Content = reader["content"].ToString();
                        task.IsDone = Convert.ToBoolean(reader["is_done"]);
                        task.Date = Convert.ToDateTime(reader["dt"]).Date;
                        TasksList.Add(task);
                    }
                }
                reader.Close();
                conn.Close();
            }
            Change_TasksState();
        }

        private readonly DelegateCommand _taskEditCommand;

        public DelegateCommand ChangeTask
        {
            get { return _taskEditCommand; }
            set { }
        }

        private void Change_TasksState()
        {
            var count = TasksList.Where(t => t.IsDone == false).Count();
            if (count != 0)
            {
                TasksStateString = NotCompleted + count;
                LabelBackGround = "Red";
            }
            else
            {
                TasksStateString = AllDone;
                LabelBackGround = "Green";
            }

            ForeGround = "white";
        }
    }
}
