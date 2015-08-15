using System;
using GameOfLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace Tester
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            List<Tuple<int, float>> list = new List<Tuple<int, float>>();
            list.Add(Tuple.Create(1, 1.1f));
            List<int> vals = new List<int>();
            vals.Add(7);
            Distibution<Tuple<int, float>, int> dist = new Distibution<Tuple<int, float>, int>(list,vals);
            AreEqual(dist[Tuple.Create(1, 1.1f)], 7);
        }

    }
}
