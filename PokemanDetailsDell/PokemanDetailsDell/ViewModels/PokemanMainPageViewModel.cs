using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PokemanDetailsDell.ViewModels
{
    public class PokemanMainPageViewModel : INotifyPropertyChanged
    {
        public PokemanMainPageViewModel()
        {
        }

        #region Commands
        #endregion

        #region Properties
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        #region Public Functions
        
        #endregion
    }
}
