using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTT_Map_API.Structs;

namespace TTT_Map_API
{
    public class Database
    {        
        static string databaseSource = "Data Source=C:\\Users\\Janno\\Desktop\\TTT_Map_Rating.db";
        static SqliteConnection connection = new SqliteConnection(databaseSource);

        static void OpenConnection()
        {
            connection.Open();            
        } 

        static void CloseConnection()
        {
            connection.Close();
        }
      
        public static List<string> GetAllMapNames()
        {
            var maps = new List<string>();

            OpenConnection();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT map_name
                FROM Map                                        
            ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    maps.Add(reader.GetString(0));
                }
            }

            CloseConnection();

            return maps;
        }

        public static long GetMapIdByName(string mapName)
        {
            long mapId = 0;

            OpenConnection();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT map_id
                FROM Map                        
                WHERE 
                    map_name LIKE $mapName
            ";

            command.Parameters.AddWithValue("$mapName", mapName);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    mapId = reader.GetInt64(0);
                }
            }

            CloseConnection();

            return mapId;
        }

        public static string GetMapNameById(long mapId)
        {
            string mapName = "";

            OpenConnection();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT map_name
                FROM Map                        
                WHERE 
                    map_id LIKE $map_id
            ";

            command.Parameters.AddWithValue("$map_id", mapId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    mapName = reader.GetString(0);
                }
            }

            CloseConnection();

            return mapName;
        }

        public static int GetMapAverageRating(long mapId)
        {
            var ratings = new List<int>();

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT rating_value
                FROM Rating
                    INNER JOIN Map
                    ON Rating.map_id = Map.map_id        
                WHERE 
                    Map.map_id = $id
            ";

            command.Parameters.AddWithValue("$id", mapId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ratings.Add(reader.GetInt32(0));
                }
            }

            CloseConnection();

            var sumOfRatingValues = 0;
            foreach (var rating in ratings)
            {
                sumOfRatingValues += rating;
            }

            return sumOfRatingValues / ratings.Count;
        }
    }
}
