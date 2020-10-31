using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Toute
{
    /// <summary>
    /// Interaction logic for ImageFullWindow.xaml
    /// </summary>
    public partial class ImageFullWindow : Window
    {
        public ImageFullWindow()
        {
            InitializeComponent();

            DataContext = new ImageFullWindowViewModel(this);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
