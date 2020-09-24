using Ninject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toute
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        public static SettingsViewModel SettingsViewModel = IoC.Kernel.Get<SettingsViewModel>();

        public static ApplicationViewModel ApplicationViewModel = IoC.Kernel.Get<ApplicationViewModel>();
    }
}
