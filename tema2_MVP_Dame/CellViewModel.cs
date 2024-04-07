using System.Printing.IndexedProperties;
using System.Windows.Input;
using System;

namespace tema2_MVP_Dame
{
    public class CellViewModel
    {

        public Cell CellModel { get; set; }
        public CellViewModel(Cell cellModel)
        {
            CellModel = cellModel;
        }
    }
}