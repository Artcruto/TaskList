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
    /// Логика взаимодействия для TasksListWindow.xaml
    /// </summary>
    public partial class TasksListWindow : Window
    {
        public TasksListWindow()
        {
            InitializeComponent();
        }
    }
}
