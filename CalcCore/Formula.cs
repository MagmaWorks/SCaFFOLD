﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class Formula
    {
        public List<string> Expression { get; set; } = new List<string>() { "" };
        public string Ref { get; set; } = "";
        public string Narrative { get; set; } = "";
        public string Conclusion { get; set; } = "";
        public CalcStatus Status { get; set; } = CalcStatus.NONE;

        public Formula()
        {
        }
    }
}
