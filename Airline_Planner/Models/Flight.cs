using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Airline_Planner.Models
{
    public class Flight
    {
        private int _id;
        private int _flight_number;
        private DateTime _time = new DateTime();
        private string _adl;
        private string _status;

        public Flight(int id, int flight_number, DateTime time, string adl, string status)
        {
            _id = id;
            _flight_number = flight_number;
            _time = time;
            _adl = adl;
            _status = status;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetFlightNumber()
        {
            return _flight_number;
        }

        public DateTime GetTime()
        {
            return _time;
        }

        public string GetADL()
        {
            return _adl;
        }

        public string GetStatus()
        {
            return _status;
        }


        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight)otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool nameEquality = this.GetFlightNumber() == newFlight.GetFlightNumber();
                bool timeEquality = this.GetTime() == newFlight.GetTime();
                bool adlEquality = this.GetADL() == newFlight.GetADL();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                //return (idEquality && nameEquality && timeEquality && departureEquality && arrivalEquality && statusEquality);
                return (idEquality && nameEquality && timeEquality && adlEquality && statusEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetFlightNumber().GetHashCode();
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                int Flight_Number = rdr.GetInt32(1);
                DateTime Time = rdr.GetDateTime(2);
                string ADL = rdr.GetString(3);
                string Status = rdr.GetString(4);
                Flight newFlight = new Flight(Id, Flight_Number, Time, ADL, Status);
                allFlights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (flight_number, time, adl, status) VALUES (@flight_number, @time, @adl, @status);";

            MySqlParameter flight_number = new MySqlParameter();
            flight_number.ParameterName = "@flight_number";
            flight_number.Value = this._flight_number;
            cmd.Parameters.Add(flight_number);

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@time";
            time.Value = this._time;
            cmd.Parameters.Add(time);

            MySqlParameter adl = new MySqlParameter();
            adl.ParameterName = "@adl";
            adl.Value = this._adl;
            cmd.Parameters.Add(adl);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@status";
            status.Value = this._status;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int FlightId = 0;
            int FlightNumber = 0;
            DateTime Time = new DateTime();
            string ADL = String.Empty;
            string Status = String.Empty;
            while (rdr.Read())
            {
                FlightId = rdr.GetInt32(0);
                FlightNumber = rdr.GetInt32(1);
                Time = rdr.GetDateTime(2);
                ADL = rdr.GetString(3);
                Status = rdr.GetString(4);
            }
            Flight newFlight = new Flight(FlightId, FlightNumber, Time, ADL, Status);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newFlight;
        }


        public void UpdateFlight(DateTime newTime, string newStatus)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET (time, status) = (@newTime, @newStatus WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@newTime";
            time.Value = newTime;
            cmd.Parameters.Add(time);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@newTime";
            status.Value = newStatus;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _time = newTime;
            _status = newStatus;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId; DELETE FROM cities_flights WHERE flight_id = @FlightId;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = this.GetId();
            cmd.Parameters.Add(flightIdParameter);

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
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = newCity.GetId();
            cmd.Parameters.Add(city_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = _id;
            cmd.Parameters.Add(flight_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public List<City> GetCities()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT cities.* FROM flights
                JOIN cities_flights ON (flights.id = cities_flights.flight_id)
                JOIN cities ON (cities_flights.city_id = cities.id)
                WHERE flights.id = @FlightId;";

            MySqlParameter FlightIdParameter = new MySqlParameter();
            FlightIdParameter.ParameterName = "@FlightId";
            FlightIdParameter.Value = this._id;
            cmd.Parameters.Add(FlightIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<City> cities = new List<City> { };

            while (rdr.Read())
            {
                int cityId = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                City newCity = new City(cityId, cityName);
                cities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return cities;
        }
    }
}


