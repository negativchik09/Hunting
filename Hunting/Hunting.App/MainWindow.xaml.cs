using System;
using System.Collections.Generic;
using System.IO;
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
using Hunting.BL;
using Hunting.BL.Matrix;
using Hunting.BL.Units;

namespace Hunting.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage rabbitImage;
        private BitmapImage wolfImage;
        private BitmapImage huntsmanImage;
        private SolidColorBrush grassColor;
        private SolidColorBrush waterColor;
        private SolidColorBrush dirtColor;
        private SolidColorBrush stoneColor;
        private SolidColorBrush treeColor;
        private Gateway gateway;
        public MainWindow()
        {
            InitializeComponent();
            gateway = new Gateway();
            ReadAssets();
            CreateGameField();
            gateway.MapUpdated += UpdateGameField;
            StreamReader reader = null;
            string content = "";
            try
            {
                reader = new StreamReader(@"Assets/map.json");
                content = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            if (String.IsNullOrEmpty(content))
            {
                MessageBox.Show("Вкрали карту");
                return;
            }
            if (!gateway.LoadMap(content)){
                MessageBox.Show("Ошибка при загрузке карты");
                return;
            }
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
            rabbitImage = new BitmapImage();
            rabbitImage.BeginInit();
            rabbitImage.UriSource = new Uri(@"Assets/rabbit.png", UriKind.Relative);
            rabbitImage.EndInit();
            wolfImage = new BitmapImage();
            wolfImage.BeginInit();
            wolfImage.UriSource = new Uri(@"Assets/wolf.png", UriKind.Relative);
            wolfImage.EndInit();
            huntsmanImage = new BitmapImage();
            huntsmanImage.BeginInit();
            huntsmanImage.UriSource = new Uri(@"Assets/hunter.png", UriKind.Relative);
            huntsmanImage.EndInit();
            grassColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#078b07");
            waterColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#064295");
            dirtColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#72493a");
            stoneColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#7d7676");
            treeColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#004301");
        }
        private void UpdateGameField(Object? sender, BL.Special.MapUpdateEventParameters p)
        {
            foreach (Node node in p.Nodes.ToList())
            {
                switch ((int)node.Surface)
                {
                    case 0:
                        (GameField.Children[node.Y * 40 + node.X] as Border).Background = grassColor;
                        break;
                    case 1:
                        (GameField.Children[node.Y * 40 + node.X] as Border).Background = waterColor;
                        break;
                    case 2:
                        (GameField.Children[node.Y * 40 + node.X] as Border).Background = dirtColor;
                        break;
                    case 3:
                        (GameField.Children[node.Y * 40 + node.X] as Border).Background = stoneColor;
                        break;
                    case 4:
                        (GameField.Children[node.Y * 40 + node.X] as Border).Background = treeColor;
                        break;
                    default:
                        break;
                }
                if (node.Unit != null)
                {
                    if (((GameField.Children[node.Y * 40 + node.X] as Border).Child as Image).Source != null)
                    {
                        ((GameField.Children[node.Y * 40 + node.X] as Border).Child as Image).Source = null;
                    }
                    switch (node.Unit.UnitType)
                    {
                        case nameof(Rabbit):
                            ((GameField.Children[node.Y * 40 + node.X] as Border).Child as Image).Source = rabbitImage;
                            break;
                        case nameof(Wolf):
                            ((GameField.Children[node.Y * 40 + node.X] as Border).Child as Image).Source = wolfImage;
                            break;
                        case nameof(Huntsman):
                            ((GameField.Children[node.Y * 40 + node.X] as Border).Child as Image).Source = huntsmanImage;
                            break;
                        default:
                            MessageBox.Show("Ошибка имени юнита");
                            return;
                    }
                }
            }
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "JSON files (*.json) | *.json";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == false)
            {
                System.Windows.MessageBox.Show("Произошла ошибка при открытии диалогового окна открытия файла.");
                return;
            }
            string path = dlg.FileName;
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string content = reader.ReadToEnd();
                    gateway.LoadMap(content);
                }
            }
            catch (ArgumentException)
            {
                System.Windows.MessageBox.Show("Произошла ошибка." +
                    "Проверьте правильность пути к файлу.");
                return;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "JSON files (*.json) | *.json";
            dlg.DefaultExt = "txt";
            dlg.AddExtension = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == false)
            {
                System.Windows.MessageBox.Show("Произошла ошибка при открытии диалогового окна сохраниения файла.");
                return;
            }
            string path = dlg.FileName;
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(gateway.SaveMap());
            }
        }
        private void StartContinuouslyButton_Click(object sender, RoutedEventArgs e)
        {
            gateway.StartContinuousExecution(Int32.Parse(TurnTimeComboBox.Text[0].ToString()));
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            gateway.StopContinuousExecution();
        }
        private void PassOneTurnButton_Click(object sender, RoutedEventArgs e)
        {
            gateway.ExecuteOneTurn();
        }
        private void SendCommandsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}