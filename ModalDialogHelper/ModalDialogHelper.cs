using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModalDialogHelper
{
    public class ModalDialogHelper<T>
    {
        private TaskCompletionSource<T> completionSource;
        private Grid basePageContentGrid;
        private ModalDialogContainer dialogContainer;
        private IModalDialog<T> dialogInterface;
        private AppBar bottomBar;
        private AppBar topBar;

        public Task<T> ShowDialogAsync(Type controlType,object dataContext)
        {
            // Initialize helper.
            completionSource = new TaskCompletionSource<T>();
            var basePage = (Window.Current.Content as Frame)?.Content as Page;
            if (basePage.BottomAppBar != null)
            {
                bottomBar = basePage.BottomAppBar;
                bottomBar.Visibility = Visibility.Collapsed;
            }

            if (basePage.TopAppBar != null)
            {
                topBar = basePage.TopAppBar;
                topBar.Visibility = Visibility.Collapsed;
            }

            if (basePage == null)
                throw new Exception("Frame should contain Page object inside.");

            basePageContentGrid = basePage.Content as Grid;

            // Generate Control object from specified Type.
            var modalControl = Activator.CreateInstance(controlType) as Control;
            if (modalControl == null)
                throw new Exception("Specified type must be a Control type.");

            if (dataContext != null)
            {
                modalControl.DataContext = dataContext;
                dialogInterface = dataContext as IModalDialog<T>;
                if (dialogInterface != null)
                {
                    dialogInterface.DialogDismissRequested += DialogDismissRequested;
                    dialogInterface.OnInitializedAsync();
                }
            }

            // Construct container object.
            dialogContainer = new ModalDialogContainer(modalControl);
            if (basePageContentGrid.ColumnDefinitions.Count > 0)
                Grid.SetColumnSpan(dialogContainer, basePageContentGrid.ColumnDefinitions.Count);

            if (basePageContentGrid.RowDefinitions.Count > 0)
                Grid.SetRowSpan(dialogContainer, basePageContentGrid.RowDefinitions.Count);

            basePageContentGrid.Children.Add(dialogContainer);

            return completionSource.Task;
        }

        private void DialogDismissRequested(object sender, T e)
        {
            DismissInternal(e);
        }

        public void Hide()
        {
            DismissInternal(default(T));
        }

        private void DismissInternal(T e)
        {
            if (completionSource != null)
                completionSource.TrySetResult(e);

            if (dialogInterface != null)
                dialogInterface.DialogDismissRequested -= DialogDismissRequested;

            if (basePageContentGrid != null)
                basePageContentGrid.Children.Remove(dialogContainer);

            if (bottomBar != null)
                bottomBar.Visibility = Visibility.Visible;

            if (topBar != null)
                topBar.Visibility = Visibility.Visible;

            topBar = null;
            bottomBar = null;
            basePageContentGrid = null;
            dialogContainer = null;
            completionSource = null;
        }
    }
}
