﻿using System.Windows;

namespace Toute
{
    /// <summary>
    /// Interaction logic for  AddGameWindow.xaml
    /// </summary>
    public partial class QuickSearchDialog : Window
    {
        /// <summary>
        /// Default constructor of AddGameWindow
        /// </summary>
        public QuickSearchDialog()
        {
            //Prepares components for window
            InitializeComponent();

            //Makes owner of window a MainWindow
            Owner = Application.Current.MainWindow;

            //Binds DataContext to AddGameWindowViewModel and pass window as parameter
            DataContext = new QuickSearchViewModel(this);
        }
    }
}
