using System.Windows;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Property that get focus on any control
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
                return;

            //If value is true
            if ((bool)e.NewValue)
            {
                //Fires a event, when control is loaded...
                control.Loaded += (sender, e) =>
                {
                    //and focus the control
                    control.Focus();
                };
            }
        }
    }

    /// <summary>
    /// Property that get focus on any control
    /// </summary>
    public class GetFocusWhenVisibleIsUpAttachedProperty : BaseAttachedProperty<GetFocusWhenVisibleIsUpAttachedProperty, bool>
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
                return;

            //If value is true
            if ((bool)e.NewValue)
            {
                //Fires a event, when control visibility is changed...
                control.IsVisibleChanged += (sender, e) =>
                {
                    if(control.Visibility == Visibility.Visible)
                        control.Focus();
                };
            }
        }
    }
}
