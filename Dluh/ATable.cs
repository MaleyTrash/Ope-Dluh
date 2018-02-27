using SQLite;

namespace Dluh
{
    public abstract class ATable
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}