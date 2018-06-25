using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Calculator_DBLog;
using System;

namespace Calculator_DBLog.Tests
{
	/// <summary>
    /// DBLog tests.
    /// </summary>
    [TestClass]
    public class DBLogTests
    {
		//define transcation
        private TransactionScope transaction;

        //initialize
        [TestInitialize]
        public void Initialize()
        {
			//create transcation
            transaction = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
			//roll back
            transaction.Dispose();
        }

        [TestMethod]
        public void WriteDateTimeAndReturnsContainsDataTime()
        {
            //Arrange
            var log = new DBLog();
            var dataTime = DateTime.Now.ToString();

            //Act
            log.Write(dataTime);

            //Assert
            Assert.IsTrue(log.Read().Contains(dataTime));
        }
    }
}