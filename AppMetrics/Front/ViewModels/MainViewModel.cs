using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using SwitchingViewsMVVM.Commands;

namespace AppMetricsCSharp.ViewModels
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }

        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }
    }
}