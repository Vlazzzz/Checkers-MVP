namespace tema2_MVP_Dame.ViewModels
{
    public class Cell : BaseViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string ImagePath { get; set; }
        public Cell(int row, int column, string imagePath)
        {
            Row = row;
            Column = column;
            ImagePath = imagePath;
        }
    }
}