using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AX.WPF
{
    public class CustomRenderControlViewBase<ViewModelType> : CustomRenderOverlayControl
       where ViewModelType : INotifyPropertyChanged
    {
        public ViewModelType ViewModel
        {
            get { return (ViewModelType)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelType), typeof(CustomRenderControlViewBase<ViewModelType>), new PropertyMetadata(null));


        protected virtual void OnViewModelChanged(ViewModelType oldValue, ViewModelType newValue)
        {
            this.DataContext = newValue;

            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ViewModel_PropertyChanged;
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += ViewModel_PropertyChanged;
            }

        }

        protected virtual void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as CustomRenderControlViewBase<ViewModelType>;
            element.OnViewModelChanged((ViewModelType)e.OldValue, (ViewModelType)e.NewValue);
        }

    }
}
