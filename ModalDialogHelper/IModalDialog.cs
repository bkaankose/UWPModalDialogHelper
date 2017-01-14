using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalDialogHelper
{
    public interface IModalDialog<T>
    {
        event EventHandler<T> DialogDismissRequested;
        Task OnInitializedAsync();
    }
}
