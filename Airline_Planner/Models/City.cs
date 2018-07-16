using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Airline_Planner.Models
{
    public class City
    {
        private int _id;
        private string _name;

        public City(int id, string name)
        {
            _id = id;
            _name = name;
        }


        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City)otherCity;
                bool idEquality = this.GetId() == newCity.GetId();
                bool nameEquality = this.GetName() == newCity.GetName();
                return (idEquality && nameEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            cmd.ExecuteReader();
            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string Name = rdr.GetString(1);
                City newCity = new City(Id, Name);
                allCities.Add(newCity);
             }
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }
    }

}
