using System;
using System.Windows.Input;
using AppMetricsCSharp.ViewModels;
using AppMetricsCSharp.Views;

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
            if(parameter.ToString() == "Column")
            {
                viewModel.SelectedViewModel = new ColumnChartModel();
            }
            else if(parameter.ToString() == "Pie")
            {
                viewModel.SelectedViewModel = new PieChartModel();
            }else if (parameter.ToString() == "Line")
            {
                viewModel.SelectedViewModel = new LineChartModel();
            }
        }
    }
}
