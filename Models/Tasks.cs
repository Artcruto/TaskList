using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    class Tasks
    {
        public int TaskId { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public DateTime Date { get; set; }
    }
}
