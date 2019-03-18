using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksList.Models
{
    public class TaskModel : IModel
    {
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }
    }
}
