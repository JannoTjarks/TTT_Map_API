using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT_Map_API.Structs
{
    public class TableContent
    {
        [JsonConstructor]
        public TableContent(string name, float rating)
        {
            Name = name;
            Rating = rating;
        }

        public string Name { get; }
        public float Rating { get; }

        public override string ToString() => $"({Name}, {Rating})";
    }
}
