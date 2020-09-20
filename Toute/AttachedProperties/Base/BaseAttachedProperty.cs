using System.Windows;

namespace Toute
{
    /// <summary>
    /// Base Attached Property can be easily used to 
    /// create simple attached properties
    /// </summary>
    /// <typeparam name="Parent">Class itself</typeparam>
    /// <typeparam name="T">Type of value to change</typeparam>

    public class BaseAttachedProperty<Parent, T> where Parent : new()
    {
        /// <summary>
        /// Gets the value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetValue(DependencyObject obj)
        {
            return (T)obj.GetValue(ValueProperty);
        }

        /// <summary>
        /// Set value of dependency property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetValue(DependencyObject obj, T value)
        {
            obj.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Create a single instance of class that are using DP
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        /// <summary>
        /// Register DP
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(T), typeof(BaseAttachedProperty<Parent, T>), new PropertyMetadata(false, OnValuePropertyChanged));

        /// <summary>
        /// Fired every time, when DP is changed
        /// This method fires a <see cref="OnValueChanged(DependencyObject, DependencyPropertyChangedEventArgs)"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Instance of class is calling OnValueChanged, that
            //will fire OnValueChanged
            (Instance as BaseAttachedProperty<Parent, T>).OnValueChanged(d, e);
        }

        /// <summary>
        /// Virtual method, that is fired every time when value is changed
        /// Can be overridden in inherited class
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }
    }
}
