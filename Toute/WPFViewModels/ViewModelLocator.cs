﻿using Ninject;

namespace Toute
{
    /// <summary>
    /// Locator that help find ViewModels in xaml, and keep single instances of them.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Static member of this class
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// Static ViewModel of ApplicationViewModel
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel = IoC.Kernel.Get<ApplicationViewModel>();
    }
}
