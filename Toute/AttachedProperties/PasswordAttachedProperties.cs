using System.Windows;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Attached Property that will be used to monitor PasswordBox.
    /// If value of password will be changed, <see cref="HasPasswordAttachedProperty"/>
    /// will be changed too.
    /// </summary>
    public class MonitorPasswordAttachedProperty : BaseAttachedProperty<MonitorPasswordAttachedProperty, bool>
    {
        /// <summary>
        /// Override base <see cref="OnValueChanged(DependencyObject, DependencyPropertyChangedEventArgs)"/>
        /// </summary>
        /// <param name="d">Dependency Object to send
        /// It have to be PasswordBox</param>
        /// <param name="e">Value</param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //If it is not PasswordBox...
            if (!(d is PasswordBox box))
                return;

            //Clear previous Box_PasswordChanged events
            box.PasswordChanged -= Box_PasswordChanged;

            //If it is new value...
            if ((bool)e.NewValue)
            {
                //Fire Box_PasswordChanged event
                box.PasswordChanged += Box_PasswordChanged;
            }
        }

        /// <summary>
        /// Method that will be fired every time when
        /// PasswordChanged event will occur.
        /// </summary>
        /// <param name="sender">PasswordBox</param>
        /// <param name="e"></param>
        private void Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Sets new value of HasPasswordAttachedProperty
            HasPasswordAttachedProperty.SetValue((PasswordBox)sender);
        }
    }

    /// <summary>
    /// Attached Property that will set boolean value of itself
    /// depends of length of password
    /// </summary>
    public class HasPasswordAttachedProperty : BaseAttachedProperty<HasPasswordAttachedProperty, bool>
    {
        /// <summary>
        /// Sets Value of <see cref="HasPasswordAttachedProperty"/>
        /// </summary>
        /// <param name="sender">PasswordBox</param>
        public static void SetValue(DependencyObject sender)
        {
            //If Password have any length, sets value to true, instead to false
            SetValue(sender, ((PasswordBox)sender).SecurePassword.Length > 0);
        }
    }
}
