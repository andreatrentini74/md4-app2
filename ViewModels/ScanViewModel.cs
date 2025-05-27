using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace md4.ViewModels
{
    public class ScanViewModel : INotifyPropertyChanged
    {
        public ICommand CancelClicked { get; }
        public ScanViewModel()
        {
            CancelClicked = new Command(CommandCancelClicked);
        }
        private void CommandCancelClicked()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
