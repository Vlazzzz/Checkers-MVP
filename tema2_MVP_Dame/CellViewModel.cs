using System.Windows.Input;

namespace tema2_MVP_Dame
{
    public class CellViewModel
    {
        public Cell CellModel { get; set; }
        public ICommand CellClickCommand { get; private set; }

        public CellViewModel(Cell cellModel)
        {
            CellModel = cellModel;

            // Initialize command for handling cell clicks
            CellClickCommand = new RelayCommand(CellClick);
        }

        private void CellClick(object parameter)
        {
            // Logic for handling click on the cell
        }
    }
}