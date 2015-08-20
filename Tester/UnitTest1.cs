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
        public void DistibutionTest()
        {
            Distribution<float, int> one_dim = new Distribution<float, int>();
            Distribution<int, float, int> tow_Dim = new Distribution<int, float, int>();
            Random r = new Random();
            one_dim[1f] = 4;
            tow_Dim[1] = new Distribution<float, int>();
            tow_Dim[1][2f] = 3;
            AreEqual(one_dim[0f], 4);
            AreEqual(tow_Dim[1][1f], 3);
        }





    }
}
