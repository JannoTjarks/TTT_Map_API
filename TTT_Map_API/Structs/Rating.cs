using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT_Map_API.Structs
{
    public class Rating
    {
        public Rating(long id, int value, string user, string map)
        {
            Id = id;
            Value = value;
            User = user;
            Map = map;
        }

        public long Id { get; }
        public string User { get; }
        public string Map { get; }
        public int Value { get; }

        public override string ToString() => $"({Id}, {Map}, {User}, {Value})";
    }
}
