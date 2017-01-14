using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModalDialogHelper
{
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public class ModalDialogContainer : Control
    {
        private ContentPresenter _contentPresenter;
        private readonly Control _modalControl;
        public ModalDialogContainer(Control modalControl)
        {
            _modalControl = modalControl;
        }

        protected override void OnApplyTemplate()
        {
            _contentPresenter = GetTemplateChild("ContentPresenter") as ContentPresenter;
            _contentPresenter.Content = _modalControl;

            base.OnApplyTemplate();
        }
    }
}
