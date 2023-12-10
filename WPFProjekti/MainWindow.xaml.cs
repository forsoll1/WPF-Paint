using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFProjekti
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color colour;

        string tool;
        double linesize;
        bool fill;

        double sX;
        double sY;
        Shape e1;
        Line l1;
        Polyline p1;

        List<UIElement> deletedObjects = new List<UIElement>();
        public MainWindow()
        {
            InitializeComponent();
            colorPicker.SelectedColor = Color.FromRgb(00,00,00);
            this.colour = colorPicker.SelectedColor.Value;
            canvas1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            update_tool();
            update_fill();
            update_linesize();
        }


        private void colorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            this.colour = colorPicker.SelectedColor.Value;
        }
        private void colorBackground_SelectedColorChanged(object sender, EventArgs e)
        {
            canvas1.Background = new SolidColorBrush(colorBackground.SelectedColor.Value);
        }

        private void draw_object(object sender, MouseButtonEventArgs e)
        {
            sX = Convert.ToDouble(e.GetPosition(canvas1).X);
            sY = Convert.ToDouble(e.GetPosition(canvas1).Y);

            if (tool == "1")
            {
                Polyline p2 = new Polyline();
                p2.Stroke = new SolidColorBrush(colour);
                p2.StrokeThickness = linesize;

                PointCollection points = new PointCollection();
                points.Add(new Point(sX, sY));

                p2.Points = points;
                canvas1.Children.Add(p2);
                this.p1 = p2;

            }

            if (tool == "2")
            {
                Line l2 = new Line();
                l2.Stroke = new SolidColorBrush(colour);
                l2.StrokeThickness = linesize;
                l2.X1 = sX;
                l2.Y1 = sY;

                canvas1.Children.Add(l2);
                this.l1 = l2;
            }

            if (tool == "3")
            {
                Rectangle e2 = new Rectangle();
                e2.Stroke = new SolidColorBrush(colour);
                e2.StrokeThickness = linesize;
                if (fill)
                {
                    e2.Fill = new SolidColorBrush(colour);
                }
                e2.Width = 1;
                e2.Height = 1;

                canvas1.Children.Add(e2);
                Canvas.SetLeft(e2, e.GetPosition(canvas1).X);
                Canvas.SetTop(e2, e.GetPosition(canvas1).Y);
                this.e1 = e2;
            }

            if (tool == "4"){
                Ellipse e2 = new Ellipse();
                e2.Stroke = new SolidColorBrush(colour);
                e2.StrokeThickness = linesize;
                if(fill)
                {
                    e2.Fill = new SolidColorBrush(colour);
                }
                e2.Width = 1;
                e2.Height = 1;

                canvas1.Children.Add(e2);
                Canvas.SetLeft(e2, e.GetPosition(canvas1).X);
                Canvas.SetTop(e2, e.GetPosition(canvas1).Y);
                this.e1 = e2;
            }
            if (tool == "5")
            {

                double newx = e.GetPosition(canvas1).X;
                double newy = e.GetPosition(canvas1).Y;
                string userText = null;

                textInput getText = new textInput(colour, linesize);
                getText.ShowDialog();

                if (getText.DialogResult == true)
                {
                    userText = getText.inputText.Text;
                }

                if (userText == null || userText.Length < 1)
                {
                    Trace.WriteLine("No text to present");
                    return;
                }

                TextBlock textBlock = new TextBlock();

                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Text = userText;
                textBlock.FontSize = linesize;
                textBlock.Foreground = new SolidColorBrush(colour);

                canvas1.Children.Add(textBlock);

                Canvas.SetLeft(textBlock, newx);
                Canvas.SetTop(textBlock, newy);
            }


        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tool == "1" && p1 != null)
            {
                p1.Points.Add(new Point(e.GetPosition(canvas1).X, e.GetPosition(canvas1).Y));
            }
            if (tool == "2" && l1 != null )
            {
                
                l1.X2 = e.GetPosition(canvas1).X;
                l1.Y2 = e.GetPosition(canvas1).Y;

            }
            if (tool == "3"|| tool == "4"){
                double newX = Convert.ToDouble(e.GetPosition(canvas1).X);
                double newY = Convert.ToDouble(e.GetPosition(canvas1).Y);
                if (e1 != null)
                {
                
                    if (newX < sX && newY < sY)
                    {
                        e1.Width = sX - newX;
                        e1.Height = sY - newY;
                        Canvas.SetTop(e1, e.GetPosition(canvas1).Y);
                        Canvas.SetLeft(e1, e.GetPosition(canvas1).X);
                    }
                    else if (newY < sY)
                    {
                        e1.Height = sY - newY;
                        Canvas.SetTop(e1, e.GetPosition(canvas1).Y);
                        e1.Width = Math.Abs(Convert.ToDouble(e.GetPosition(canvas1).X) - sX);
                    }else if(newX < sX)
                    {
                        e1.Width = sX - newX;
                        Canvas.SetLeft(e1, e.GetPosition(canvas1).X);
                        e1.Height = Math.Abs(Convert.ToDouble(e.GetPosition(canvas1).Y) - sY);
                    }
                    else
                    {
                        e1.Width = Math.Abs(Convert.ToDouble(e.GetPosition(canvas1).X) - sX);
                        e1.Height = Math.Abs(Convert.ToDouble(e.GetPosition(canvas1).Y) - sY);
                    }
                }
            }
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e1 = null;
            l1 = null;
            p1 = null;
        }

        private void lineSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update_linesize();
        }
        private void update_linesize()
        {
            ComboBoxItem comboItem = (ComboBoxItem)lineSize.SelectedItem;
            linesize = Convert.ToDouble(comboItem.Tag);
        }
        private void update_fill()
        {
            fill = fillCheck.IsChecked.Value;
        }
        private void update_tool()
        {
            ComboBoxItem comboItem = (ComboBoxItem)toolSelect.SelectedItem;
            tool = comboItem.Tag as string;
        }

        private void fillCheck_Click(object sender, RoutedEventArgs e)
        {
            update_fill();
        }

        private void toolSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update_tool();
        }

        private void menu_save_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Image";
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image documents (.png)|*.png";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dialog.FileName;

                    Rect rect = new Rect(canvas1.RenderSize);
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                      (int)rect.Bottom, 96d, 96d, PixelFormats.Default);
                    rtb.Render(canvas1);
                    BitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                    MemoryStream ms = new MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    File.WriteAllBytes(filename, ms.ToArray());
                }
                catch (Exception)
                {
                    Trace.WriteLine("Could not write to file");
                }
            }
        }

        private void menu_open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
            dl1.FileName = "MYFileSave";
            dl1.DefaultExt = ".png";
            dl1.Filter = "Image documents (.png, .jpeg, .jpg)|*.png; *.jpg; *.jpeg";
            bool? result = dl1.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dl1.FileName;
                    Uri fileUri = new Uri(@filename);

                    Image kuva = new Image();
                    BitmapImage bitmap = new BitmapImage(fileUri);
                    kuva.Width = bitmap.Width;
                    kuva.Height = bitmap.Height;
                    kuva.Source = bitmap;
                    kuva.ClipToBounds = true;

                    canvas1.Children.Add(kuva);
                    List<UIElement> testi = new List<UIElement>();
                    foreach (UIElement i in canvas1.Children){
                        testi.Add(i);
                    }

                    if (bitmap.Height > canvas1.ActualHeight)
                    {
                        baseWindow.Height = bitmap.Height + topMenu.ActualHeight;
                    }
                    if (bitmap.Width > canvas1.ActualWidth)
                    {
                        baseWindow.Width = bitmap.Width + 180;
                    }
                }
                catch
                {
                    Trace.WriteLine("Could not load image.");
                }
            }
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if(canvas1.Children.Count > 0)
            {
                deletedObjects.Add(canvas1.Children[canvas1.Children.Count - 1]);
                canvas1.Children.RemoveAt(canvas1.Children.Count - 1);
            }
        }
        private void menu_new_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            deletedObjects.Clear();
            canvas1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            colorBackground.SelectedColor = Color.FromRgb(255,255,255);
            baseWindow.Height = 435;
            baseWindow.Width = 800;
        }

        private void project_save_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Project";
            dialog.DefaultExt = ".png";
            dialog.Filter = "XAML (.xaml)|*.xaml";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dialog.FileName;
                    string strXAML = XamlWriter.Save(canvas1.Children);

                    using (FileStream fs = File.Create(filename))
                    {
                        using (StreamWriter streamwriter = new StreamWriter(fs))
                        {
                            streamwriter.Write(strXAML);
                        }
                    }
                }
                catch (Exception)
                {
                    Trace.WriteLine("Could not write to file");
                }
            }
        }

        private void project_load_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
            dl1.FileName = "Project";
            dl1.DefaultExt = ".xaml";
            dl1.Filter = "XAML (.xaml)|*.xaml;";
            bool? result = dl1.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dl1.FileName;
                    canvas1.Children.Clear();
                    DeSerializeXAML(canvas1.Children, filename);
                }
                catch
                {
                    Trace.WriteLine("Could not load image.");
                }
            }
        }
        public static void DeSerializeXAML(UIElementCollection elements, string filename)
        {
            var context = XamlReader.GetWpfSchemaContext();

            var settings = new System.Xaml.XamlObjectWriterSettings
            {
                RootObjectInstance = elements
            };

            using (var reader = new System.Xaml.XamlXmlReader(filename))
            using (var writer = new System.Xaml.XamlObjectWriter(context, settings))
            {
                System.Xaml.XamlServices.Transform(reader, writer);
            }
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (deletedObjects.Count > 0)
            {
                try
                {
                    canvas1.Children.Add(deletedObjects.Last());
                    deletedObjects.Remove(deletedObjects.Last());
                }
                catch
                {
                    Trace.WriteLine("Couldn't redo");
                }
            }
        }

        private void menu_print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog prnt = new PrintDialog();
            if (prnt.ShowDialog() == true)
            {
                    prnt.PrintVisual(canvas1, "Printing Canvas");
            }
        }

        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
