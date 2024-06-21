using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.RpcContracts.Documents;
using Scaffold.VisualStudio.Models.Xaml;

namespace Scaffold.VisualStudio.XamlDesigner;

/// <summary>
/// See XAML file for notes on these interactions in this project.
/// This file does not need copying over to the AddIn project.
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();

        ViewModel = new MainWindowViewModel();

        StartWatcherButton.Click += StartWatcher_Click;
        StopWatcherButton.Click += StopWatcher_Click;
        DataContext = ViewModel;
        
        OpenTab_Click(null, null);
        StartWatcher_Click(null, null);
    }

    private async void OpenTab_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.ShownAsync(
            new DocumentEventArgs(
                new Uri(
                    "file:///C:/Users/d.growns/Documents/Repos/ScaffoldForVsTesting/VsTesting/Core/AdditionCalculation.cs?vs-version=1")), CancellationToken.None);

        SimulateOpenTab.Visibility = Visibility.Collapsed;
    }

    private async void FileSaved_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SavedAsync(
            new DocumentEventArgs(
                new Uri("file:///C:/Users/d.growns/Documents/Repos/ScaffoldForVsTesting/VsTesting/Core/AdditionCalculation.cs?vs-version=2")), CancellationToken.None);
    }

    private async void TreeItem_Clicked(object sender, RoutedEventArgs e)
    {
        var button = (Button)e.Source;
        var treeItem = (TreeItem)button.DataContext;

        await treeItem.ChangeTreeItemExpansionCommand.ExecuteAsync(null, null, CancellationToken.None);
    }

    private async void StartWatcher_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.StartWatcherCommand.ExecuteAsync(null, null, CancellationToken.None);

        SimulateFileSaved.Visibility = Visibility.Visible;
    }

    private async void StopWatcher_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.StopWatcherCommand.ExecuteAsync(null, null, CancellationToken.None);

        SimulateFileSaved.Visibility = Visibility.Collapsed;
    }

    public static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var list = new List<T>();
        if (depObj == null) 
            return list;

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);

            if (child is T dependencyObject)
                list.Add(dependencyObject);
            
            var childItems = FindVisualChildren<T>(child);
            if (childItems == null || childItems.Any() == false) 
                continue;

            list.AddRange(childItems);
        }

        return list;
    }
}