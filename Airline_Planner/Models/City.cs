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
                //return (idEquality && nameEquality);
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
            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string Name = rdr.GetString(1);
                City newCity = new City(Id, Name);
                allCities.Add(newCity);
             }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";
           
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CityId = 0;
            string Name = String.Empty;
            while (rdr.Read())
            {
                CityId = rdr.GetInt32(0);
                Name = rdr.GetString(1);
            }
            City newCity = new City(CityId, Name);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newCity;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_flights WHERE city_id = @CityId;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = this.GetId();
            cmd.Parameters.Add(cityIdParameter);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = newFlight.GetId();
            cmd.Parameters.Add(flight_id);

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = _id;
            cmd.Parameters.Add(city_id);
        }

        public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
                JOIN cities_flights ON (cities.id = cities_flights.city_id)
                JOIN flights ON (cities_flights.flight_id = flights.id)
                WHERE cities.id = @CityId;";

            MySqlParameter CityIdParameter = new MySqlParameter();
            CityIdParameter.ParameterName = "@CityId";
            CityIdParameter.Value = this._id;
            cmd.Parameters.Add(CityIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight> { };

            while (rdr.Read())
            {
                //int flightId = rdr.GetInt32(0);
                //int flightNum = rdr.GetInt32(1);
                //DateTime flightTime = rdr.GetDateTime(2);
                //string flightADL = rdr.GetString(3);
                //string flightStatus = rdr.GetString(4);

                //Flight newFlight = new Flight(flightId, flightNum, flightTime, flightADL, flightStatus);
                DateTime testTime = new DateTime(1999, 1, 12);
                Flight testFlight = new Flight(1, 22, testTime, "D", "on time");
                flights.Add(testFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Console.WriteLine(flights.Count);
            return flights;
        }

    }

}
