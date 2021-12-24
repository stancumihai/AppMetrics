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
            Console.WriteLine(parameter.ToString());
            switch (parameter.ToString())
            {
                case "Column":
                    MetricsRegistry.NullifyRegistry();
                    viewModel.SelectedViewModel = new ColumnChartModel();
                    break;
                case "Pie":
                    MetricsRegistry.NullifyRegistry();
                    viewModel.SelectedViewModel = new PieChartModel();
                    break;
                case "Line":
                    MetricsRegistry.NullifyRegistry();
                    viewModel.SelectedViewModel = new LineChartModel();
                    break;
            }
        }
    }
}