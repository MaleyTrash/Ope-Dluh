using SQLite;
using System;

namespace Dluh
{
    class Item : ATable
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public DateTime ToDate { get; set; }
        public int Multiplier { get; set; }

        public override string ToString()
        {
            if(Date == DateTime.MinValue || Multiplier < 0)
            {
                return string.Format("{0} | {1}", Name, Price);
            }
            return string.Format("{0} | {1},- | {2} | {3} | {4}", Name, Price, Date.ToString(), ToDate.ToString(), Multiplier);
        }
    }
}
