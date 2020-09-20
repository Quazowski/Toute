using System.ComponentModel;

namespace Toute
{
    /// <summary>
    /// Base ViewModel that is used for all ViewModel in application
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// INotifyPropertyChanged implementation that
        /// changes values run time
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Method that update value of property
        /// </summary>
        /// <param name="name">Name of property which value should be updated </param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
