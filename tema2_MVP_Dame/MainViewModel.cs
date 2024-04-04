using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace tema2_MVP_Dame
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Board board;
        public Board Board
        {
            get { return board; }
            set
            {
                board = value;
                OnPropertyChanged(nameof(Board));
            }
        }

        public ObservableCollection<CellViewModel> Cells { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand MakeMoveCommand { get; private set; }

        public MainViewModel()
        {
            Board = new Board();
            Player player1 = new Player("Vlad", PieceColor.White);
            Player player2 = new Player("Andrei", PieceColor.Red);

            // Initializează comanda pentru a efectua o mutare
            MakeMoveCommand = new RelayCommand(MakeMove);

            // Inițializează celulele tablei de joc
            InitializeBoard();
        }

        private void MakeMove(object parameter)
        {
            // Logică pentru a efectua o mutare pe tabla de joc
            // (poate fi adăugată aici sau într-o altă metodă, în funcție de necesități)
        }

        private void InitializeBoard()
        {
            Cells = new ObservableCollection<CellViewModel>();

            // Adaugă celulele în colecția de celule
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Creează o celulă și adaugă-o în colecție
                    CellViewModel cellViewModel = new CellViewModel(new Cell(row, col));
                    Cells.Add(cellViewModel);
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
