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
    // 2. ФАБРИЧНЫЕ МЕТОДЫ (Создатели)
    // ==========================================
    public abstract class ShapeCreator
    {
        // Тот самый фабричный метод
        public abstract IMyShape CreateShape(Brush color);
    }

    public class CircleCreator : ShapeCreator
    {
        public override IMyShape CreateShape(Brush color) => new MyCircle(color);
    }

    public class SquareCreator : ShapeCreator
    {
        public override IMyShape CreateShape(Brush color) => new MySquare(color);
    }

    public class TriangleCreator : ShapeCreator
    {
        public override IMyShape CreateShape(Brush color) => new MyTriangle(color);
    }

    // ==========================================
    // 3. КЛИЕНТСКИЙ КОД (UI)
    // ==========================================
    public partial class MainWindow : Window
    {
        private List<ShapeCreator> _creators;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализируем список создателей
            _creators = new List<ShapeCreator>
            {
                new CircleCreator(),
                new SquareCreator(),
                new TriangleCreator()
            };
        }

        private void ColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                string colorStr = selectedItem.Tag.ToString();
                Brush brush = colorStr switch
                {
                    "Red" => Brushes.Red,
                    "Blue" => Brushes.Blue,
                    "Green" => Brushes.Green,
                    _ => Brushes.Black
                };

                GenerateShapes(brush);
            }
        }

        private void GenerateShapes(Brush color)
        {
            DrawingArea.Children.Clear(); // Удаляем старые фигуры

            // Клиентский код перебирает создателей и вызывает фабричный метод
            foreach (var creator in _creators)
            {
                IMyShape shape = creator.CreateShape(color);
                DrawingArea.Children.Add(shape.GetElement());
            }
        }
    }
}