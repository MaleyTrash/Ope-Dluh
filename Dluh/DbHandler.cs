using SQLite;
using System.Collections.Generic;

namespace Dluh
{
    class DbHandler
    {
        private const string DB_PATH = @"./database.db";
        SQLiteConnection _db;
        public DbHandler()
        {
            _db = new SQLiteConnection(DB_PATH);
            _db.CreateTable<Item>();
        }

        public int Insert<T>(T item) where T: ATable, new()
        {
            return _db.Insert(item);
        }

        public List<T> SelectAll<T>() where T : ATable, new()
        {
            List<T> ret = new List<T>();
            foreach(T item in _db.Table<T>())
            {
                ret.Add(item);
            }
            return ret;
        }

        public int Update<T>(T item) where T : ATable, new()
        {
            return _db.Update(item);
        }

        public List<T> SelectWhere<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> exp) where T : ATable, new()
        {
            List<T> ret = new List<T>();
            foreach(T item in _db.Table<T>().Where(exp))
            {
                ret.Add(item);
            }
            return ret;
        }
    }
}