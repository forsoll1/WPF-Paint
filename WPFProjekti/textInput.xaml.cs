using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFProjekti
{
    /// <summary>
    /// Interaction logic for textInput.xaml
    /// </summary>
    public partial class textInput : Window
    {
        public textInput(Color color, double textSize)
        {
            InitializeComponent();
            previewText.FontSize = textSize;
            previewText.Foreground = new SolidColorBrush(color);
        }

        private void acceptBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void inputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            previewText.Text = inputText.Text;
        }
    }
}
