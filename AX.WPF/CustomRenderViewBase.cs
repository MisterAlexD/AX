using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alex.WPF
{
    public class CustomRenderViewBase<ViewModelClass> : CustomRenderOverlayElement, IView
        where ViewModelClass : class, INotifyPropertyChanged
    {
        public PropertyChangedEventHandler VMPropertyChangedCallback => ViewModelPropertyChanged;

        public ViewModelClass ViewModel
        {
            get { return (ViewModelClass)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = VMHolder<ViewModelClass>.AddVMOwner(typeof(CustomRenderViewBase<ViewModelClass>));

        public CustomRenderViewBase()
        {
            
        }

        protected virtual void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        { }

    }
}
