using System.Windows;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Property that get focus on any control when used
    /// </summary>
    public class GetFocusAttachedProperty : BaseAttachedProperty<GetFocusAttachedProperty, bool>
    {
        /// <summary>
        /// If value of property is changed
        /// </summary>
        /// <param name="d">Control</param>
        /// <param name="e">Value</param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //If it is not a control...
            if (!(d is Control control))
                //returns
                return;

            //If new value is true
            if ((bool)e.NewValue)
            {
                //Fires a event, when control is loaded...
                control.Loaded += (sender, e) =>
                {
                    //and focusing given control
                    control.Focus();
                };
            }
        }
    }
}
