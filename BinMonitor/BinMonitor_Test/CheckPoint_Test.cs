using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinMonitor.Common;

namespace BinMonitor.Testing
{
    [TestClass]
    public class CheckPoint_Test
    {
        [TestMethod]
        public void Ellapsed_True()
        {
            CheckPoint checkpoint = new CheckPoint();
            checkpoint.Configuration.Duration = TimeSpan.FromMinutes(5);
            DateTime origin = new DateTime(2000, 1, 1, 13, 0, 0);
            DateTime current = new DateTime(2000, 1, 1, 13, 5, 1);
            Assert.IsTrue(checkpoint.Elapsed(origin, current));
        }

        [TestMethod]
        public void Ellapsed_False()
        {
            CheckPoint checkpoint = new CheckPoint();
            checkpoint.Configuration.Duration = TimeSpan.FromMinutes(5);
            DateTime origin = new DateTime(2000, 1, 1, 13, 0, 0);
            DateTime current = new DateTime(2000, 1, 1, 13, 4, 59);
            Assert.IsFalse(checkpoint.Elapsed(origin, current));
        }
    }
}
