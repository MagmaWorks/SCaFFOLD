using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestCalcs.Moment myMomentCalc = new TestCalcs.Moment(5,4);
            Assert.AreEqual(
                "12.5",
                myMomentCalc.GetOutputs()[0].ValueAsString
                );
            
        }
    }
}
