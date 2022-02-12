using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hunting.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage rabbit;
        private BitmapImage wolf;
        private BitmapImage hunter;
        public MainWindow()
        {
            InitializeComponent();
            ReadAssets();
            CreateGameField();
            UpdateGameField();

        }
        private void CreateGameField()
        {
            TextBlock textBlock;
            Image image;
            Border border;
            for (int i = 0; i < 40; i++)
            {
                XIndexes.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.025, GridUnitType.Star) });
                textBlock = new TextBlock { Text = $"{i}", 
                                            TextAlignment = TextAlignment.Center,
                                            VerticalAlignment = VerticalAlignment.Center };
                textBlock.SetValue(Grid.ColumnProperty, i);
                XIndexes.Children.Add(textBlock);

                YIndexes.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.025, GridUnitType.Star) });
                textBlock = new TextBlock { Text = $"{i}",                       
                                            TextAlignment = TextAlignment.Center,
                                            VerticalAlignment = VerticalAlignment.Center};
                textBlock.SetValue(Grid.RowProperty, i);
                YIndexes.Children.Add(textBlock);
            }
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    image = new Image();
                    border = new Border() { Background = Brushes.Green, 
                                            BorderBrush = Brushes.DarkGray, 
                                            BorderThickness = new Thickness() {Left = 1, Right = 1, Top = 1, Bottom = 1 } };
                    border.Child = image;
                    GameField.Children.Insert((i * 40 + j), border);
                }
            }
        }

        private void ReadAssets()
        {
            rabbit = new BitmapImage();
            rabbit.BeginInit();
            rabbit.UriSource = new Uri(@"Assets/rabbit.png", UriKind.Relative);
            rabbit.EndInit();
            wolf = new BitmapImage();
            wolf.BeginInit();
            wolf.UriSource = new Uri(@"Assets/wolf.png", UriKind.Relative);
            wolf.EndInit();
            hunter = new BitmapImage();
            hunter.BeginInit();
            hunter.UriSource = new Uri(@"Assets/hunter.png", UriKind.Relative);
            hunter.EndInit();
        }
        private void UpdateGameField()
        {
            int imageIndex;
            for(int i = 1; i < 1600; i += Random.Shared.Next(3, 10))
            {
                if ((GameField.Children[i] as Border).Child != null)
                {
                    imageIndex = Random.Shared.Next(0, 3);
                    switch (imageIndex)
                    {
                        case 0:
                            ((GameField.Children[i] as Border).Child as Image).Source = rabbit;
                            break;
                        case 1:
                            ((GameField.Children[i] as Border).Child as Image).Source = wolf;
                            break;
                        case 2:
                            ((GameField.Children[i] as Border).Child as Image).Source = hunter;
                            break;
                    }
                }
            }
        }
    }
}