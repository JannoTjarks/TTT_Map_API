using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT_Map_Frontend
{
    public class Rating
    {
        public Rating(long id, int value, DateTime timestamp, string user, string map)
        {
            Id = id;
            Value = value;
            Timestamp = timestamp;
            User = user;
            Map = map;
        }

        public long Id { get; }
        public DateTime Timestamp { get; }
        public string User { get; }
        public string Map { get; }
        public int Value { get; }

        public override string ToString() => $"({Id}, {Timestamp}, {Map}, {User}, {Value})";
    }
}
