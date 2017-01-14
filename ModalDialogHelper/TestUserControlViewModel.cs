using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModalDialogHelper
{
    public class TestUserControlViewModel : INotifyPropertyChanged,IModalDialog<bool>
    {
        public DelegateCommand ReturnTrueCommand { get; set; }
        public DelegateCommand ReturnFalseCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<bool> DialogDismissRequested;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public async Task OnInitializedAsync()
        {
            await Task.Delay(5000);
        }

        private string _testString = "This is TestUserControl";

        public string TestString
        {
            get { return _testString; }
            set { _testString = value; OnPropertyChanged(nameof(TestString)); }
        }

        public TestUserControlViewModel()
        {
            ReturnFalseCommand = new DelegateCommand(ReturnFalse);
            ReturnTrueCommand = new DelegateCommand(ReturnTrue);
        }

        private void ReturnFalse()
        {
            DialogDismissRequested.Invoke(null, false);
        }

        private void ReturnTrue()
        {
            DialogDismissRequested.Invoke(null, true);
        }
    }
}
