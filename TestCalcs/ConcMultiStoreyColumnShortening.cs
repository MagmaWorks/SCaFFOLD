using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    [CalcName("Concrete multi storey column shortening")]
    public class ConcMultiStoreyColumnShortening : CalcBase
    {
        List<Formula> expressions;
        CalcDouble notionalcreepcoeff;
        CalcDouble creepCoeff;
        CalcSelectionList concGrade;
        CalcDouble meanCompStr;
        CalcDouble charCompStr;
        CalcDouble relativeHumidity1;
        //CalcDouble relativeHumiditySwitchTime;
        //CalcDouble relativeHumidity2;
        //CalcDouble time0;
        CalcDouble time;
        CalcDouble timeShrinkStart;
        //CalcDouble F;
        CalcDouble creepTimeCoeff;
        CalcSelectionList cementType;
        CalcDouble dryingStrain;
        CalcDouble totalShrinkageStrain;
        CalcDouble totalCreepMovement;
        CalcDouble shrinkageMovement;
        CalcDouble movementLevel;
        CalcDouble movement1;
        CalcDouble movement2;
        CalcDouble diffMovement;
        CalcListOfDoubleArrays _columnData1;
        CalcListOfDoubleArrays _columnData2;


        public ConcMultiStoreyColumnShortening()
        {
            initialise();
        }

        private void initialise()
        {
            _columnData1 = inputValues.CreateCalcListOfDoubleArrays("List of column data 2: T, H, L1, T1, L2, T2, W, L",
                new List<double[]>
                    {
                        new double[]{0, 35000, 1400, 56, 2800, 112, 1000, 1000 },
                        new double[]{112, 35000, 1400, 168, 2800, 224, 800, 800 }
                    });
            _columnData2 = inputValues.CreateCalcListOfDoubleArrays("List of column data 1: T, H, L1, T1, L2, T2, W, L",
                new List<double[]>
                    {
                        new double[]{0, 35000, 1400, 56, 2800, 112, 1400, 1400 },
                        new double[]{112, 35000, 1400, 168, 2800, 224, 900, 900 }
                    });
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            notionalcreepcoeff = outputValues.CreateDoubleCalcValue("Notional creep coefficient", @"\varphi(t,t_0)", "", 0);
            charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            relativeHumidity1 = inputValues.CreateDoubleCalcValue("Relative humidity 1", "RH", "", 70);
            //relativeHumidity2 = inputValues.CreateDoubleCalcValue("Relative humidity 2", "RH", "", 50);
            //relativeHumiditySwitchTime = inputValues.CreateDoubleCalcValue("Relative humidity switch time", "t_{humiditychange}", "d", 80);
            //time0 = inputValues.CreateDoubleCalcValue("Time load applied", "t_0", "days", 28);
            time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            timeShrinkStart = inputValues.CreateDoubleCalcValue("Shrinkage start", "t_s", "days", 7);
            creepTimeCoeff = outputValues.CreateDoubleCalcValue("Coefficient for creep with time", @"\beta(t,t_0)", "", 0);
            creepCoeff = outputValues.CreateDoubleCalcValue("Creep coefficient", @"\varphi_0", "", 0);
            cementType = inputValues.CreateCalcSelectionList("Cement type", "S", new List<string> { "S", "N", "R" });
            dryingStrain = outputValues.CreateDoubleCalcValue("Unrestrained drying strain", @"\epsilon_{cd,0}", "", 0);
            totalShrinkageStrain = outputValues.CreateDoubleCalcValue("Total shrinkage strain", @"\epsilon_{cs}", "", 0);
            totalCreepMovement = outputValues.CreateDoubleCalcValue("Total creep movement", "", "mm", 0);
            shrinkageMovement = outputValues.CreateDoubleCalcValue("Shrinkage movement", "", "mm", 0);
            movementLevel = inputValues.CreateDoubleCalcValue("Calc movement at level", "", "", 1);
            movement1 = outputValues.CreateDoubleCalcValue("Column 1 movement at level", "", "mm", 0);
            movement2 = outputValues.CreateDoubleCalcValue("Column 2 movement at level", "", "mm", 0); 
            diffMovement = outputValues.CreateDoubleCalcValue("Diff movement", "", "mm", 0);
            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

        public override void UpdateCalc()
        {
            // set up time-load history for each column lift
            List<ConcColumnShortening> columnCalcs1 = new List<ConcColumnShortening>();
            for (int i = 0; i < _columnData1.Value.Count; i++)
            {
                List<double[]> loadTimes = new List<double[]>();

                var _coldata = _columnData1.Value[i];
                var startTime = _coldata[0];
                if (startTime <= time.Value)
                {
                    for (int j = i; j < _columnData1.Value.Count; j++)
                    {
                        var _data = _columnData1.Value[j];
                        loadTimes.Add(new double[] { _data[2], _data[3] - startTime });
                        loadTimes.Add(new double[] { _data[4], _data[5] - startTime });
                    }
                    ConcColumnShortening calc = new ConcColumnShortening(loadTimes,
                        concGrade.ValueAsString,
                        relativeHumidity1.Value,
                        time.Value - startTime,
                        timeShrinkStart.Value,
                        _coldata[6],
                        _coldata[7],
                        _coldata[1]);
                    calc.UpdateCalc();
                    columnCalcs1.Add(calc);
                }
            }

            List<ConcColumnShortening> columnCalcs2 = new List<ConcColumnShortening>();
            for (int i = 0; i < _columnData2.Value.Count; i++)
            {
                List<double[]> loadTimes = new List<double[]>();
                var _coldata = _columnData2.Value[i];
                var startTime = _coldata[0];
                if (startTime <= time.Value)
                {
                    for (int j = i; j < _columnData2.Value.Count; j++)
                    {
                        var _data = _columnData2.Value[j];
                        loadTimes.Add(new double[] { _data[2], _data[3] - startTime });
                        loadTimes.Add(new double[] { _data[4], _data[5] - startTime });
                    }
                    ConcColumnShortening calc = new ConcColumnShortening(loadTimes,
                        concGrade.ValueAsString,
                        relativeHumidity1.Value,
                        time.Value - startTime,
                        timeShrinkStart.Value,
                        _coldata[6],
                        _coldata[7],
                        _coldata[1]);
                    calc.UpdateCalc();
                    columnCalcs2.Add(calc); 
                }
            }
            int levelCount = (int)movementLevel.Value;
            double move1 = 0; double move2 = 0;
            double preMove1 = 0; double preMove2 = 0;
            if (levelCount > 1 && levelCount < columnCalcs1.Count + 1)
            {
                double timeAtConstructionOfLevel1 = _columnData1.Value[levelCount -1][0];
                for (int i = 0; i < levelCount - 1; i++)
                {
                    columnCalcs1[i].Time = timeAtConstructionOfLevel1 - _columnData1.Value[i][0];
                    preMove1 += columnCalcs1[i].TotalMovement;
                    columnCalcs1[i].Time = time.Value - _columnData1.Value[i][0];
                }
            }
            if (levelCount > 1 && levelCount < columnCalcs2.Count + 1)
            {
                double timeAtConstructionOfLevel2 = _columnData2.Value[levelCount - 1][0];
                for (int i = 0; i < levelCount - 1; i++)
                {
                    columnCalcs2[i].Time = timeAtConstructionOfLevel2 - _columnData2.Value[i][0];
                    preMove2 += columnCalcs2[i].TotalMovement;
                    columnCalcs2[i].Time = time.Value - _columnData2.Value[i][0];
                }
            }


            if (levelCount > 0 && levelCount < columnCalcs1.Count + 1)
            {
                for (int i = 0; i < levelCount; i++)
                {
                    move1 += columnCalcs1[i].TotalMovement;
                }
            }
            if (levelCount > 0 && levelCount < columnCalcs2.Count + 1)
            {
                for (int i = 0; i < levelCount; i++)
                {
                    move2 += columnCalcs2[i].TotalMovement;
                }
            }
            movement1.Value = move1 - preMove1;
            movement2.Value = move2 - preMove2;
            diffMovement.Value = movement1.Value - movement2.Value;
        }
    }
}
