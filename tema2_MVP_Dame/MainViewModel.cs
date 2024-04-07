using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace tema2_MVP_Dame
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private GameViewModel gameVM;

        public ObservableCollection<CellViewModel> Cells { get; set; }

        public MainViewModel()
        {
            // Create an instance of GameViewModel
            gameVM = new GameViewModel();

            // Initialize the observable collection of cells with the cells from the game board
            Cells = new ObservableCollection<CellViewModel>();
            foreach (Cell cell in gameVM.Game.GameBoard)
            {
                // Add each cell to the observable collection
                Cells.Add(new CellViewModel(cell));
            }
        }

        private void MakeMove(object parameter)
        {
            // Logic for making a move on the game board
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}