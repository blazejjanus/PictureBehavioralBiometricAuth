using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace PictureBehavioralBiometricAuth.Components.Controls {
    public partial class ImageAuthenticationInput : UserControl {
        public const int GRID_CELL_SIZE = 40;
        private bool _displayGrid = false;
        public bool DisplayGrid {
            get => _displayGrid;
            set {
                if(value != _displayGrid) {
                    _displayGrid = value;
                    foreach (var line in _verticalLines) {
                        line.IsVisible = value;
                    }
                    foreach (var line in _horizontalLines) {
                        line.IsVisible = value;
                    }
                }
            }
        }

        private List<Line> _verticalLines = new List<Line>();
        private List<Line> _horizontalLines = new List<Line>();

        public ImageAuthenticationInput() {
            InitializeComponent();
            this.PointerPressed += UserControl_PointerPressed;
            CreateGrid();
        }

        private void CreateGrid() {
            DisplayGrid = true;
            int numVerticalLines = (int)Width / GRID_CELL_SIZE;
            int numHorizontalLines = (int)Height / GRID_CELL_SIZE;
            for (int i = 0; i < numVerticalLines; i++) {
                var line = new Line {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    StartPoint = new Point(i * GRID_CELL_SIZE, 0),
                    EndPoint = new Point(i * GRID_CELL_SIZE, Height),
                };
                line.IsVisible = DisplayGrid;
                _verticalLines.Add(line);
                this.Draw.Children.Add(line);
            }
            for (int i = 0; i < numHorizontalLines; i++) {
                var line = new Line {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    StartPoint = new Point(0, i * GRID_CELL_SIZE),
                    EndPoint = new Point(Width, i * GRID_CELL_SIZE),
                };
                line.IsVisible = DisplayGrid;
                _horizontalLines.Add(line);
                this.Draw.Children.Add(line);
            }
        }

        private void UserControl_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
            var click = e.GetPosition(this);
            Debug.WriteLine($"UserControl pressed! X:{click.X}, Y:{click.Y}");
        }
    }
}
