using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp13
{
    public partial class MainWindow : Window
    {
        private Brush _confirmedFill = Brushes.LightGray;
        private double _confirmedSize = 150;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            this.Closing += MainWindow_Closing;
        }

        private void ApplyTemporaryPreview(Action applyChange, Action revertChange, string message)
        {
            applyChange();

            var result = MessageBox.Show(
                message + "\nПрименить изменения?",
                "Предпросмотр",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ConfirmChanges();
                MessageBox.Show("Изменения успешно сохранены!", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                revertChange();
                MessageBox.Show("Изменения отменены. Возврат к исходному виду.", "Отмена",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ConfirmChanges()
        {
            _confirmedFill = MyShape.Fill;
            _confirmedSize = MyShape.Width;
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            Brush tempColor = new SolidColorBrush(
                Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));

            ApplyTemporaryPreview(
                applyChange: () => MyShape.Fill = tempColor,
                revertChange: () => MyShape.Fill = _confirmedFill,
                message: "Цвет фигуры временно изменён."
            );
        }

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            double newSize = MyShape.Width == 150 ? 250 : 150;

            ApplyTemporaryPreview(
                applyChange: () => { MyShape.Width = newSize; MyShape.Height = newSize; },
                revertChange: () => { MyShape.Width = _confirmedSize; MyShape.Height = _confirmedSize; },
                message: $"Размер временно изменён на {newSize}x{newSize}."
            );
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            bool ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if (e.Key == Key.F1)
            {
                Help_Click(sender, null);
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.D1) 
            {
                ChangeColor_Click(sender, null);
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.D2) 
            {
                ChangeSize_Click(sender, null);
                e.Handled = true;
            }
        }

        private void About_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show("WPF Приложение: Меню и Предпросмотр\nВерсия 1.0",
                            "О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        private void Help_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show("Горячие клавиши:\nCtrl+1 - Цвет\nCtrl+2 - Размер\nF1 - Справка",
                            "Справка", MessageBoxButton.OK, MessageBoxImage.Information);

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите выйти?\nВсе изменения будут потеряны.",
                "Закрытие приложения",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}