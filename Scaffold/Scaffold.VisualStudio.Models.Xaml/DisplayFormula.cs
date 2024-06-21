﻿using System.Runtime.Serialization;
using System.Windows;
using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class DisplayFormula : NotifyPropertyChangedObject
{
    private Visibility _imageVisibility;
    
    [DataMember] public ObservableList<DisplayExpression> Expressions { get; set; }
    [DataMember] public string Ref { get; set; }
    [DataMember] public string Narrative { get; set; }
    [DataMember] public string Conclusion { get; set; }
    [DataMember] public string Status { get; set; }
    [DataMember] public string Image { get; set; }

    [DataMember]
    public Visibility ImageVisibility
    {
        get => _imageVisibility;
        set => SetProperty(ref _imageVisibility, value);
    }
}