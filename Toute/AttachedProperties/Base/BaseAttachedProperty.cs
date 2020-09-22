using System;
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
        #region Public events

        /// <summary>
        /// Fired when any value is updated
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, e) => { };

        /// <summary>
        /// Fired when any value is changed
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        #endregion

        #region Public properties

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

        #endregion

        #region Register

        /// <summary>
        /// Register DP
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(T), typeof(BaseAttachedProperty<Parent, T>), new PropertyMetadata(false, OnValuePropertyChanged, OnValuePropertyUpdated));

        #endregion

        #region Private methods

        /// <summary>
        /// Fired every time, when value is updated
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            //Instance of class is calling OnValueUpdated
            (Instance as BaseAttachedProperty<Parent, T>).OnValueUpdated(d, value);
            (Instance as BaseAttachedProperty<Parent, T>).ValueUpdated(d, value);

            return value;
        }

        /// <summary>
        /// Fired every time, when DP is changed
        /// This method fires a <see cref="OnValueChanged(DependencyObject, DependencyPropertyChangedEventArgs)"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Instance of class is calling OnValueChanged
            (Instance as BaseAttachedProperty<Parent, T>).OnValueChanged(d, e);
            (Instance as BaseAttachedProperty<Parent, T>).ValueChanged(d, e);
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Virtual method, that is fired every time when value is changed
        /// Can be overridden in inherited class
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// Virtual method, that is fired every time when value is updated
        /// Can be overridden in inherited class
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public virtual void OnValueUpdated(DependencyObject d, object value) { }

        #endregion

    }
}
