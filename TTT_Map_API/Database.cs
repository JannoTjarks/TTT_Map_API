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
        public static string DataSource { get; set; }

        private static Database _instance = null;
        private SqliteConnection connection = null;

        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
                _instance.connection = new SqliteConnection(DataSource);
            }

            return _instance;
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public List<string> GetAllMapNames()
        {
            var maps = new List<string>();

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

            return maps;
        }

        public long GetMapIdByName(string mapName)
        {
            long result = -1;
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT map_id
                FROM Map                        
                WHERE 
                    map_name LIKE $mapName
            ";

            command.Parameters.AddWithValue("$mapName", mapName);

            if (command.ExecuteScalar() != null)
            {
                Int64.TryParse(command.ExecuteScalar().ToString(), out result);
            }

            return result;
        }

        public string GetMapNameById(long mapId)
        {
            string mapName = "";

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

            return mapName;
        }

        public float GetMapAverageRating(long mapId)
        {
            var ratings = new List<int>();
            float count = 0;

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

            float sumOfRatingValues = 0;
            foreach (float rating in ratings)
            {
                sumOfRatingValues += rating;
            }

            if (ratings.Count != 0)
            {
                return sumOfRatingValues / ratings.Count;
            }

            return 0;
            
        }

        public bool InsertRating(Rating rating)
        {
            if (isUserIdPresent(rating.User))
            {
                var mapId = GetMapIdByName(rating.Map);

                var ratingId = isUserRatingAlreadyPresent(rating.User, mapId);
                if (ratingId == -1)
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                            INSERT INTO Rating (rating_value, map_id, user_id)
                            VALUES ($rating_value, $map_id, $user_id); 
                        ";

                    command.Parameters.AddWithValue("$rating_value", rating.Value);
                    command.Parameters.AddWithValue("$map_id", mapId);
                    command.Parameters.AddWithValue("$user_id", rating.User);

                    command.ExecuteNonQuery();
                }
                else
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                            UPDATE Rating 
                            SET rating_value = $rating_value
                            WHERE rating_id = $rating_id;
                        ";

                    command.Parameters.AddWithValue("$rating_value", rating.Value);
                    command.Parameters.AddWithValue("$rating_id", ratingId);

                    command.ExecuteNonQuery();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isUserIdPresent(string userId)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT user_id 
                FROM User 
                WHERE user_id LIKE $user_id;
            ";

            command.Parameters.AddWithValue("$user_id", userId);

            if (command.ExecuteScalar() != null)
            {
                return true;
            }

            return false;
        }

        public long isUserRatingAlreadyPresent(string userId, long mapId)
        {
            long result = -1;
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT rating_id 
                FROM Rating 
                WHERE user_id LIKE $user_id
	                AND 
	                map_id = $map_id;
            ";

            command.Parameters.AddWithValue("$user_id", userId);
            command.Parameters.AddWithValue("$map_id", mapId);

            if (command.ExecuteScalar() != null)
            {
                Int64.TryParse(command.ExecuteScalar().ToString(), out result);
            }

            return result;
        }
    }
}
