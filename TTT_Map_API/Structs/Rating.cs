using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT_Map_API.Structs
{
    public class Rating
    {
        public long Id { get; set; }
        public string User { get; set; }
        public string Map { get; set; }
        public int Value { get; set; }

        public override string ToString() => $"({Id}, {Map}, {User}, {Value})";
    }
}
