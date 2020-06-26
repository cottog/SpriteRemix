using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpriteRemix.ViewModels
{
    public class DataAwareViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
