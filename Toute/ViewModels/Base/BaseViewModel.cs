using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Toute
{
    /// <summary>
    /// Base ViewModel that is used for all ViewModel in application
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected object mGlobalLock = new object();

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

        /// <summary>
        /// Method that runs command asynchronously
        /// </summary>
        /// <param name="updatingFlag">Unique boolean value</param>
        /// <param name="action">Action to perform</param>
        /// <returns></returns>
        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            lock (mGlobalLock)
            {
                if (updatingFlag.GetPropertyValue())
                    return;

                updatingFlag.SetPropertyValue(true);

            }
            try
            {
                await action();
            }
            finally
            {
                updatingFlag.SetPropertyValue(false);
            }
        }
    }
}
