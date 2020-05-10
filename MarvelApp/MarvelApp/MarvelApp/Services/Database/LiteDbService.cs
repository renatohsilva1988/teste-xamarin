using LiteDB;
using MarvelApp.Helpers;
using MarvelApp.Model.Base;
using MarvelApp.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MarvelApp.Services.Database
{
    public class LiteDbService : ILiteDbService
    {
        LiteDatabase database;

        public LiteDbService()
        {
            database = new LiteDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("Marvel.db"));
        }

        public T FirstOrDefaut<T>() where T : ModelBase
        {
            ILiteCollection<T> collection = database.GetCollection<T>();
            return collection.FindAll().FirstOrDefault();
        }

        public T FindById<T>(int id) where T : ModelBase
        {
            ILiteCollection<T> collection = database.GetCollection<T>();
            return collection.FindOne(item => item.Id == id);
        }

        public IEnumerable<T> FindAll<T>() where T : ModelBase
        {
            ILiteCollection<T> collection = database.GetCollection<T>();
            return collection.FindAll();
        }

        public bool UpsertItem<T>(T item) where T : ModelBase
        {
            ILiteCollection<T> collection = database.GetCollection<T>();
            return collection.Upsert(item);
        }

        public int DeleteAll<T>() where T : ModelBase
        {
            ILiteCollection<T> collection = database.GetCollection<T>();
            return collection.DeleteAll();
        }
    }
}
