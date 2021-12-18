using System;
using System.Windows.Input;
using AppMetricsCSharp.ViewModels;
using ConsoleApp1;

namespace SwitchingViewsMVVM.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Column")
            {
                MetricsRegistry.NullifyRegistry();
                viewModel.SelectedViewModel = new ColumnChartModel();
            }
            else if (parameter.ToString() == "Pie")
            {
                MetricsRegistry.NullifyRegistry();
                viewModel.SelectedViewModel = new PieChartModel();
            }
            else if (parameter.ToString() == "Line")
            {
                MetricsRegistry.NullifyRegistry();
                viewModel.SelectedViewModel = new LineChartModel();
            }
        }
    }
}