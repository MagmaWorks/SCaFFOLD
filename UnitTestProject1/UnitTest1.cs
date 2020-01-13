using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    TestCalcs.Moment myMomentCalc = new TestCalcs.Moment(5,4);
        //    Assert.AreEqual(
        //        "12.5",
        //        myMomentCalc.GetOutputs()[0].ValueAsString
        //        );
            
        //}

        [TestMethod]
        public void TestWoodArmerCalc()
        {
            EssentialCalcs.WoodArmer myCalc = new EssentialCalcs.WoodArmer(6.96, 17.75, -14.83);
            myCalc.UpdateCalc();
            var outputs = myCalc.GetOutputs();
            IEnumerable<CalcCore.CalcValueBase> resultValue =
                from result in outputs
                where result.Symbol == "M_xb"
                select result;
            Assert.AreEqual(
                21.8,
                Math.Round((resultValue.ToList()[0] as CalcCore.CalcDouble).Value, 1, MidpointRounding.AwayFromZero)
                );
            resultValue =
                 from result in outputs
                 where result.Symbol == "M_yb"
                 select result;
            Assert.AreEqual(
                32.6,
                Math.Round((resultValue.ToList()[0] as CalcCore.CalcDouble).Value, 1, MidpointRounding.AwayFromZero)
                );
            resultValue =
                from result in outputs
                where result.Symbol == "M_xt"
                select result;
            Assert.AreEqual(
                5.4,
                Math.Round((resultValue.ToList()[0] as CalcCore.CalcDouble).Value, 1, MidpointRounding.AwayFromZero)
                );
            resultValue =
                from result in outputs
                where result.Symbol == "M_yt"
                select result;
            Assert.AreEqual(
                0,
                Math.Round((resultValue.ToList()[0] as CalcCore.CalcDouble).Value, 2, MidpointRounding.AwayFromZero)
                );
        }


        [TestMethod]
        public void TestDynamicRelaxation()
        {
            var myDynamicModel = new DynamicRelaxation.DynamicRelaxationSystem();
            myDynamicModel.RunSteps(10, 0.1f);
        }
    }
}
