using System.Windows;
using System.Windows.Controls;
using System.Security;
using System;

namespace Toute
{
    public class MonitorPasswordAttachedProperty : BaseAttachedProperty<MonitorPasswordAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PasswordBox box))
                return;

            box.PasswordChanged -= Box_PasswordChanged;

            if ((bool)e.NewValue)
            {
                HasPasswordAttachedProperty.SetValue(box);
                box.PasswordChanged += Box_PasswordChanged;
            }
        }

        private void Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HasPasswordAttachedProperty.SetValue((PasswordBox)sender);
        }
    }

    public class HasPasswordAttachedProperty : BaseAttachedProperty<HasPasswordAttachedProperty, bool>
    {
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, ((PasswordBox)sender).SecurePassword.Length > 0);
        }
    }
}
