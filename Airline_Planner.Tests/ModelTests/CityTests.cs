using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Airline_Planner.Models;
using Airline_Planner;

namespace MySQLCore.Tests
{

    [TestClass]
    public class CityTest : IDisposable
    {
        public CityTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
        }
        public void Dispose()
        {
            Flight.DeleteAll();
            City.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = City.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_TrueForSameDescription_City()
        {
            //Arrange, Act
            City firstCity = new City(1,"portland");
            City secondCity = new City(1, "portland");

            //Assert
            Assert.AreEqual(firstCity, secondCity);
        }

        [TestMethod]
        public void Save_CitySavesToDatabase_CityList()
        {
            //Arrange
            City testCity = new City(1, "Portland");
            testCity.Save();

            //Act
            List<City> result = City.GetAll();
            List<City> testList = new List<City> { testCity };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            //Arrange
            City testCity = new City(1,"portland");
            testCity.Save();

            //Act
            City savedCity = City.GetAll()[0];

            int result = savedCity.GetId();
            int testId = testCity.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsCityInDatabase_City()
        {
            //Arrange
            City testCity = new City(1, "portland");
            testCity.Save();

            //Act
            City result = City.Find(testCity.GetId());

            //Assert
            Assert.AreEqual(testCity, result);
        }

        [TestMethod]
        public void GetFlights_ReturnsAllCityFlights_FlightsList()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            City testCity = new City(1,"portland");
            testCity.Save();


            Flight testFlight1 = new Flight(1, 22, testTime, "a", "on time");
            testFlight1.Save();

            Flight testFlight2 = new Flight(2, 22, testTime, "d", "on time");
            testFlight2.Save();

            //Act
            testCity.AddFlight(testFlight1);
            testCity.AddFlight(testFlight2);
            List<Flight> result = testCity.GetFlights();
            List<Flight> testList = new List<Flight> { testFlight1, testFlight2 };

            //Assert
            Console.WriteLine(result.Count);
            Console.WriteLine(testList.Count );
            Console.WriteLine("here");
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesCityAssociationsFromDatabase_CityList()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight testFlight = new Flight(1, 22, testTime, "A", "on time");            
            testFlight.Save();

            City testCity = new City(1, "Portland");
            testCity.Save();

            //Act
            testCity.AddFlight(testFlight);
            testCity.Delete();

            List<City> resultFlightCities = testFlight.GetCities();
            List<City> testFlightCities = new List<City> { };

            //Assert
            CollectionAssert.AreEqual(testFlightCities, resultFlightCities);
        }
    }
}