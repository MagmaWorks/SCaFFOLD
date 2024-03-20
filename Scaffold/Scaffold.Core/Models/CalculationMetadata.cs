﻿using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Models;

public class CalculationMetadata : ICalculationMetadata
{
    public string Title { get; set; }
    public string Type { get; init; }
}