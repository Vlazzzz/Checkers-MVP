using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace tema2_MVP_Dame
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private GameViewModel gameVM;
        //public ICommand CellClickCommand { get; private set; }

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
            //CellClickCommand = new RelayCommand(CellClick);
        }

        //private void CellClick(object parameter)
        //{
        //    // Logic for handling click on the cell
        //    CellViewModel clickedCell = (CellViewModel)parameter;
        //    gameVM.Game.ShowPotentialMoves(clickedCell.CellModel);
        //}

        //private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    // Obțineți celula asociată evenimentului de click
        //    if (sender is FrameworkElement element && element.DataContext is CellViewModel cellViewModel)
        //    {
        //        Cell clickedCell = cellViewModel.CellModel;

        //        // Apelați metoda pentru a gestiona acțiunea de click pe celulă
        //        gameVM.Game.ShowPotentialMoves(clickedCell);

        //    }
        //}

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