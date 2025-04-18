﻿using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Materials.StandardMaterials;
using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcMaterial : ICalcValue, IStandardMaterial<IStandard>
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }
        public IStandard Standard { get; set; }
        public MaterialType Type { get; set; }

        public bool TryParse(string strValue)
        {
            try
            {
                CalcMaterial calcStandard = strValue.FromJson<CalcMaterial>();
                DisplayName = calcStandard.DisplayName;
                Symbol = calcStandard.Symbol;
                Status = calcStandard.Status;
                Standard = calcStandard.Standard;
                Type = calcStandard.Type;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ValueAsString()
        {
            return this.ToJson();
        }
    }
}
