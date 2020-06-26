using SpriteRemix.Classes.Tools;
using SpriteRemix.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace SpriteRemix.ViewModels
{
    public class MainWindowViewModel : DataAwareViewModel
    {
        private string toolbarWidth = "120";
        public string ToolbarWidth 
        {
            get { return toolbarWidth; }
            set
            {
                toolbarWidth = value;               
                OnPropertyChanged();
            }
        }

        public Color? selectedColor;
        public Color? SelectedColor 
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                if (SelectedTool != null && value != null)
                {
                    SelectedTool.PrimaryColor = value.Value;
                    if (CanvasVM != null)
                    {
                        CanvasVM.ChangeToolPrimaryColor(value.Value);
                    }
                }
                OnPropertyChanged();
                OnPropertyChanged("SelectedTool");
            }
        }

        private ITool selectedTool;
        public ITool SelectedTool
        {
            get { return selectedTool; }
            set
            {
                if (value != null)
                    value.PrimaryColor = SelectedColor ?? Colors.Transparent;

                selectedTool = value;
                CanvasVM.SelectedTool = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ITool> ToolCollection { get; set; }

        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        
        public CanvasViewModel CanvasVM { get; private set; }
        public MainWindowViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow, null);
            CanvasVM = new CanvasViewModel();
            InitializeToolData();
        }

        private void InitializeToolData()
        {
            var toolList = new List<ITool>();
            toolList.Add(new SinglePointTool());
            toolList.Add(new FillTool());
            toolList.Add(new CircleTool());


            ToolCollection = new ObservableCollection<ITool>(toolList);
            SelectedTool = ToolCollection[0];
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
