using MarvelApp.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.ServicesContracts
{
    public interface ILiteDbService
    {
        T FirstOrDefaut<T>() where T : ModelBase;

        T FindById<T>(int id) where T : ModelBase;

        IEnumerable<T> FindAll<T>() where T : ModelBase;

        bool UpsertItem<T>(T item) where T : ModelBase;

        int DeleteAll<T>() where T : ModelBase;
    }
}
