using System.ComponentModel;

namespace tema2_MVP_Dame
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private Game game;
        public Game Game
        {
            get { return game; }
            set
            {
                game = value;
                OnPropertyChanged(nameof(Game));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GameViewModel()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Game = new Game();
            Game.Player1 = new Player("Vlad", PieceColor.White);
            Game.Player2 = new Player("Andrei", PieceColor.Black);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}