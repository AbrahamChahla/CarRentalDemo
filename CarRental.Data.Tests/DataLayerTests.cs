using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Bootstrapper;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();
            var cars = repositoryTest.GetCars();
            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_factory_usage()
        {
            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass();
            var cars = repositoryTest.GetCars();
            Assert.IsTrue(cars != null);
        }


        [TestMethod]
        public void test_repository_mocking()
        {
            List<Car> cars = new List<Car>()
            {
                new Car() {CarId = 1, Description = "Mustang"},
                new Car() {CarId = 2, Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            RepositoryTestClass repositoryTest = new RepositoryTestClass(mockCarRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();
            Assert.IsTrue(ret == cars);
        }
        [TestMethod]
        public void test_repository_factory_mocking1()
        {
            List<Car> cars = new List<Car>()
            {
                new Car() {CarId = 1, Description = "Mustang"},
                new Car() {CarId = 2, Description = "Corvette"}
            };

            Mock<IDataRepositoryFactory> mockCarRepository = new Mock<IDataRepositoryFactory>();
            mockCarRepository.Setup(obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(cars);

            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass(mockCarRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();
            Assert.IsTrue(ret == cars);
        }
        [TestMethod]
        public void test_repository_factory_mocking2()
        {
            List<Car> cars = new List<Car>()
            {
                new Car() {CarId = 1, Description = "Mustang"},
                new Car() {CarId = 2, Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            Mock<IDataRepositoryFactory> mockDataReporsitory = new Mock<IDataRepositoryFactory>();
            mockDataReporsitory.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object);

            RepositoryFactoryTestClass repositoryTest = new RepositoryFactoryTestClass(mockDataReporsitory.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();
            Assert.IsTrue(ret == cars);
        }

       
    }

    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        private ICarRepository _CarRepository;

        public RepositoryTestClass(ICarRepository carRepository)
        {
            _CarRepository = carRepository;
        }

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _CarRepository.Get();
            return cars;
        }
    }
    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;


        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();

            IEnumerable<Car> cars = carRepository.Get();
            return cars;
        }

    }

}
