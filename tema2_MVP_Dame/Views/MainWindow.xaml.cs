﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tema2_MVP_Dame.ViewModels;

namespace tema2_MVP_Dame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Image clickedImage = (Image)sender;
        //    CellViewModel clickedCellViewModel = (CellViewModel)clickedImage.DataContext;
        //    viewModel.CellClickCommand.Execute(clickedCellViewModel);
        //}
    }
}