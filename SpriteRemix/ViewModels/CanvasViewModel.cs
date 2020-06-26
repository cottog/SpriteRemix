using SpriteRemix.Classes.Tools;
using System.Windows.Media;

namespace SpriteRemix.ViewModels
{
    public class CanvasViewModel : DataAwareViewModel
    {
        private ITool selectedTool;
        public ITool SelectedTool
        {
            get { return selectedTool; }
            set
            {
                selectedTool = value;
                OnPropertyChanged();
            }
        }


        public void ChangeToolPrimaryColor(Color color)
        {
            if (SelectedTool != null)
            {
                SelectedTool.PrimaryColor = color;
                OnPropertyChanged("SelectedTool");
            }

        }
    }
}
