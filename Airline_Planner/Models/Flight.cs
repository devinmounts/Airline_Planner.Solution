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
        private string _departure_city;
        private string _arrival_city;
        private string _status;

        public Flight(int id, int flight_number, DateTime time, string departure_city, string arrival_city, string status)
        {
            _id = id;
            _flight_number = flight_number;
            _time = time;
            _departure_city = departure_city;
            _arrival_city = arrival_city;
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

        public string GetDepartureCity()
        {
            return _departure_city;
        }
       
        public string GetArrivalCity()
        {
            return _arrival_city;
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
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool nameEquality = this.GetFlightNumber() == newFlight.GetFlightNumber();
                bool timeEquality = this.GetTime() == newFlight.GetTime();
                bool departureEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
                bool arrivalEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                return (idEquality && nameEquality && timeEquality && departureEquality && arrivalEquality && statusEquality);
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
            cmd.ExecuteReader();
            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                int Flight_Number = rdr.GetInt32(1);
                DateTime Time = rdr.GetDateTime(2);
                string Departure_City = rdr.GetString(3);
                string Arrival_City = rdr.GetString(4);
                string Status = rdr.GetString(5);
                Flight newFlight = new Flight(Id, Flight_Number, Time, Departure_City, Arrival_City, Status);
                allFlights.Add(newFlight);
            }
            if (conn != null)
            {
                conn.Dispose();
            }
            return  allFlights;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flighs (flight_number, time, departure_city, arrival_city, status) VALUES (@flight_number, @time, @departure_city, @arrival_city, @status);";

            MySqlParameter flight_number = new MySqlParameter();
            flight_number.ParameterName = "@flight_number";
            flight_number.Value = this._flight_number;
            cmd.Parameters.Add(flight_number);

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@time";
            time.Value = this._time;
            cmd.Parameters.Add(time);

            MySqlParameter departure_city = new MySqlParameter();
            departure_city.ParameterName = "@departure_city";
            departure_city.Value = this._departure_city;
            cmd.Parameters.Add(departure_city);

            MySqlParameter arrival_city = new MySqlParameter();
            arrival_city.ParameterName = "@arrival_city";
            arrival_city.Value = this._arrival_city;
            cmd.Parameters.Add(arrival_city);

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
            int FlightNumber= 0;
            DateTime Time = new DateTime();
            string DepartureCity = String.Empty;
            string ArrivalCity = String.Empty;
            string Status = String.Empty;
            while (rdr.Read())
            {
                FlightId = rdr.GetInt32(0);
                FlightNumber = rdr.GetInt32(1);
                Time = rdr.GetDateTime(2);
                DepartureCity = rdr.GetString(3);
                ArrivalCity = rdr.GetString(4);
            }
            Flight newFlight = new Flight(FlightId, FlightNumber, Time, DepartureCity, ArrivalCity, Status);
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
            cmd.CommandText = @"DELETE FROM flights WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

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

        public void AddCategory(City newCity)
        {
        }

        public List<City> GetCities(City newCity)
        {
            List<City> cities = new List<City> { };
            return cities;
        }
    }
    }

}
