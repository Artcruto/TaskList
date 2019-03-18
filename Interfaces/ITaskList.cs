﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksList.Interfaces
{
    public interface IDataRepository<Ttype>
    {
        ObservableCollection<Ttype> GetAll();
        void Add(Ttype entity);
        void Save(Ttype entity);
        void Delete(Ttype entity);
    }
}
