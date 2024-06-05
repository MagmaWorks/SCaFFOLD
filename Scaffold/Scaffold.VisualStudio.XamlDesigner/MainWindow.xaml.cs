using System;
using System.Threading;
using System.Windows;
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
}