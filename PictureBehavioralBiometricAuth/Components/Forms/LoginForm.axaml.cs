using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PictureBehavioralBiometricAuth.Components.Forms {
    public partial class LoginForm : UserControl {
        public const int GRID_CELL_SIZE = 40;
        private bool _displayGrid = false;
        private bool _displayRegions = false;
        public bool DisplayGrid {
            get => _displayGrid;
            set {
                if (value != _displayGrid) {
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
        public bool DisplayRegions {
            get => _displayRegions;
            set {
                if (value != _displayRegions) {
                    _displayRegions = value;
                    foreach (var region in _regions) {
                        region.IsVisible = value;
                    }
                }
            }
        }
        public EventHandler<string> ErrorHandler { get; set; }
        public AuthImageModel AuthImage { get; private set; }
        private List<Rectangle> _regions = new List<Rectangle>();
        private List<Line> _verticalLines = new List<Line>();
        private List<Line> _horizontalLines = new List<Line>();
        private List<Ellipse> _drawedPoints = new List<Ellipse>();
        private List<Point> _userPoints = new List<Point>();
        private Models.Grid? _grid;

        public LoginForm() {
            InitializeComponent();
            this.PointerPressed += UserControl_PointerPressed;
        }

        public void InitImage(AuthImageModel authImage) {
            AuthImage = authImage;
            CreateGrid();
            CreateRegions();
        }

        public List<AuthPointModel> GetAuthPoints() {
            if (_userPoints.Count < 5) {
                throw new System.Exception("Not enough points selected, please select 5 points.");
            }
            var points = new List<AuthPointModel>();
            int counter = 0;
            foreach (var point in _userPoints) {
                counter++;
                points.Add(new AuthPointModel() {
                    Number = counter,
                    X = (int)point.X,
                    Y = (int)point.Y,
                });
            }
            return points;
        }

        public void ClearForm() {
            foreach (var point in _drawedPoints) {
                this.Draw.Children.Remove(point);
            }
            _drawedPoints.Clear();
        }

        public static List<AuthImageRegionModel> GetRegions() {
            var regions = new List<AuthImageRegionModel>();
            #region Image Regions Declarations
            regions.Add(new AuthImageRegionModel {
                Name = "door",
                X = 215,
                Y = 270,
                Width = 75,
                Height = 128,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "left-window",
                X = 128,
                Y = 272,
                Width = 72,
                Height = 87,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "right-window",
                X = 310,
                Y = 272,
                Width = 72,
                Height = 87,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "upper-window",
                X = 220,
                Y = 140,
                Width = 68,
                Height = 84,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "antena",
                X = 340,
                Y = 10,
                Width = 125,
                Height = 115,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "chimney",
                X = 115,
                Y = 20,
                Width = 35,
                Height = 90,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "left-tree",
                X = 0,
                Y = 127,
                Width = 107,
                Height = 173,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "right-tree",
                X = 404,
                Y = 211,
                Width = 94,
                Height = 93,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "left-fence",
                X = 13,
                Y = 368,
                Width = 117,
                Height = 70,
            });
            regions.Add(new AuthImageRegionModel {
                Name = "right-fence",
                X = 354,
                Y = 370,
                Width = 140,
                Height = 67,
            });
            #endregion
            return regions;
        }

        private void CreateGrid() {
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
            _grid = new Models.Grid(AuthImage);
        }

        private void CreateRegions() {
            foreach (var region in AuthImage.Regions) {
                var rectangle = new Rectangle {
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    Width = region.Width,
                    Height = region.Height,
                    Margin = new Thickness(region.X, region.Y, 0, 0),
                };
                rectangle.IsVisible = DisplayRegions;
                _regions.Add(rectangle);
                this.Draw.Children.Add(rectangle);
            }
        }

        private void UserControl_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
            var click = e.GetPosition(this);
            if (_userPoints.Count < 5) {
                if (CheckNewPointIsDuplicate(click)) {
                    ErrorHandler.Invoke(this, "Cannot add 2 clicks in the same cell!");
                }
                _userPoints.Add(click);
                var point = new Ellipse {
                    Fill = Brushes.Blue,
                    Width = 5,
                    Height = 5,
                    Margin = new Thickness(click.X - 2.5, click.Y - 2.5, 0, 0),
                };
                _drawedPoints.Add(point);
                this.Draw.Children.Add(point);
                Debug.WriteLine($"UserControl pressed! X:{click.X}, Y:{click.Y}");
            } else {
                ErrorHandler.Invoke(this, "Cannot add more than 5 points!");
            }
        }

        private bool CheckNewPointIsDuplicate(Point newPoint) {
            if (_grid == null) throw new Exception("Grid is not initialized!");
            foreach (var point in _userPoints) {
                if(_grid.CheckPointsInSameCell(newPoint.GetAuthPoint(), point.GetAuthPoint())) {
                    return true;
                }
            }
            return false;
        }
    }
}
