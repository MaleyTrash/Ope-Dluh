using SQLite;
using System;

namespace Dluh
{
    class Item : ATable
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            if(Date == DateTime.MinValue)
            {
                return string.Format("{0} | {1}", Name, Price);
            }
            return string.Format("{0} | {1},- | {2}", Name, Price, Date.ToString());
        }
    }
}
