using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT_Map_API.Structs
{
    public class Rating
    {
        public Rating(long id, int value, DateTime timestamp, string user)
        {
            Id = id;
            Value = value;
            Timestamp = timestamp;
            User = user;
        }

        public long Id { get; }
        public DateTime Timestamp { get; }
        public string User { get; }
        public int Value { get; }

        public override string ToString() => $"({Id}, {Timestamp}, {User}, {Value})";
    }
}
