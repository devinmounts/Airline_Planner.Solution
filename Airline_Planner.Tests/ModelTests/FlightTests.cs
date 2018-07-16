using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Airline_Planner.Models;
using Airline_Planner;

namespace MySQLCore.Tests
{

    [TestClass]
    public class FlightTest : IDisposable
    {
        public FlightTest()
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
            int result = Flight.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_TrueForSameDescription_Flight()
        {
            //Arrange, Act
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight firstFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");
            Flight secondFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");

            //Assert
            Assert.AreEqual(firstFlight, secondFlight);
        }

        [TestMethod]
        public void Save_FlightSavesToDatabase_FlightList()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight testFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");
            testFlight.Save();

            //Act
            List<Flight> result = Flight.GetAll();
            List<Flight> testList = new List<Flight> { testFlight };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight testFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");
            testFlight.Save();

            //Act
            Flight savedFlight = Flight.GetAll()[0];

            int result = savedFlight.GetId();
            int testId = testFlight.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsFlightInDatabase_Flight()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight testFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");
            testFlight.Save();

            //Act
            Flight result = Flight.Find(testFlight.GetId());

            //Assert
            Assert.AreEqual(testFlight, result);
        }

        [TestMethod]
        public void GetCategories_ReturnsAllFlightCategories_CategoryList()
        {
            //Arrange
            DateTime testTime = new DateTime(1999, 1, 12);
            Flight testFlight = new Flight(1, 22, testTime, "portland", "seattle", "on time");
            testFlight.Save();

            City testCity1 = new City(1, "portland");
            testCity1.Save();

            City testCity2 = new City(2, "Seattle");
            testCity2.Save();

            //Act
            testFlight.AddCity(testCity1);
            List<City> result = testFlight.GetCities();
            List<City> testList = new List<City> { testCity1 };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }
    }
}