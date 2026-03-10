using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Factory_Method
{
    // ==========================================
    // 1. ПРОДУКТЫ (Фигуры)
    // ==========================================
    public interface IMyShape
    {
        UIElement GetElement();
    }

    public class MyCircle : IMyShape
    {
        private Ellipse _ellipse;
        public MyCircle(Brush color)
        {
            _ellipse = new Ellipse { Width = 80, Height = 80, Fill = color, Margin = new Thickness(10) };
        }
        public UIElement GetElement() => _ellipse;
    }

    public class MySquare : IMyShape
    {
        private Rectangle _rectangle;
        public MySquare(Brush color)
        {
            _rectangle = new Rectangle { Width = 80, Height = 80, Fill = color, Margin = new Thickness(10) };
        }
        public UIElement GetElement() => _rectangle;
    }

    public class MyTriangle : IMyShape
    {
        private Polygon _polygon;
        public MyTriangle(Brush color)
        {
            _polygon = new Polygon
            {
                Points = new PointCollection { new Point(40, 0), new Point(0, 80), new Point(80, 80) },
                Fill = color,
                Margin = new Thickness(10)
            };
        }
        public UIElement GetElement() => _polygon;
    }

    // ==========================================
    // 2. АБСТРАКТНАЯ ФАБРИКА И КОНКРЕТНЫЕ ФАБРИКИ
    // ==========================================
    public interface IShapeAbstractFactory
    {
        IMyShape CreateCircle();
        IMyShape CreateSquare();
        IMyShape CreateTriangle();
    }

    public class RedThemeFactory : IShapeAbstractFactory
    {
        public IMyShape CreateCircle() => new MyCircle(Brushes.Red);
        public IMyShape CreateSquare() => new MySquare(Brushes.Red);
        public IMyShape CreateTriangle() => new MyTriangle(Brushes.Red);
    }

    public class BlueThemeFactory : IShapeAbstractFactory
    {
        public IMyShape CreateCircle() => new MyCircle(Brushes.Blue);
        public IMyShape CreateSquare() => new MySquare(Brushes.Blue);
        public IMyShape CreateTriangle() => new MyTriangle(Brushes.Blue);
    }

    public class GreenThemeFactory : IShapeAbstractFactory
    {
        public IMyShape CreateCircle() => new MyCircle(Brushes.Green);
        public IMyShape CreateSquare() => new MySquare(Brushes.Green);
        public IMyShape CreateTriangle() => new MyTriangle(Brushes.Green);
    }

    // ==========================================
    // 3. КЛИЕНТСКИЙ КОД (UI)
    // ==========================================
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                string colorStr = selectedItem.Tag.ToString();

                // Выбираем нужную фабрику в зависимости от выбранной темы
                IShapeAbstractFactory factory = colorStr switch
                {
                    "Red" => new RedThemeFactory(),
                    "Blue" => new BlueThemeFactory(),
                    "Green" => new GreenThemeFactory(),
                    _ => new RedThemeFactory()
                };

                GenerateShapes(factory);
            }
        }

        private void GenerateShapes(IShapeAbstractFactory factory)
        {
            DrawingArea.Children.Clear(); // Удаляем старые фигуры

            // Теперь клиентский код работает только с интерфейсом Абстрактной Фабрики
            // Ему не нужно знать про кисти (Brush) и конкретные цвета.
            DrawingArea.Children.Add(factory.CreateCircle().GetElement());
            DrawingArea.Children.Add(factory.CreateSquare().GetElement());
            DrawingArea.Children.Add(factory.CreateTriangle().GetElement());
        }
    }
}