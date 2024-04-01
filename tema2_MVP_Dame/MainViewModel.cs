using System;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand MakeMoveCommand { get; private set; }

        public MainViewModel()
        {
            Board = new Board();
            MakeMoveCommand = new RelayCommand(MakeMove);
        }

        private void MakeMove(object parameter)
        {
            //iau linia si coloana de start si de final in urma click-ului pe tabla
            board.MakeMove(0, 0, 1, 1);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}