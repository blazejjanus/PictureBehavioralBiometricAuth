using PictureBehavioralBiometricAuth.Db.Models;
using System;
using System.Collections.Generic;

namespace PictureBehavioralBiometricAuth.Models {
    public class Grid {
        private List<Row> Rows { get; set; } = new List<Row>();

        public Grid(AuthImageModel image) {
            CreateGrid(image);
        }

        private void CreateGrid(AuthImageModel image) {
            for (int i = 0; i < image.Height; i += image.GridCellSize) {
                Rows.Add(new Row(i / image.GridCellSize, image));
            }
        }

        public (int X, int Y) GetCellIndex(AuthPointModel point) {
            for (int i = 0; i < Rows.Count; i++) {
                int cellIndex = Rows[i].GetCellIndex(point);
                if (cellIndex != -1) {
                    return (i, cellIndex);
                }
            }
            return (-1, -1);
        }

        public bool CheckPointsInSameCell(AuthPointModel point1, AuthPointModel point2) {
            var cell1 = GetCellIndex(point1);
            var cell2 = GetCellIndex(point2);
            if (cell1.X == -1 || cell1.Y == -1) throw new Exception("Point1 is not in grid!");
            if (cell2.X == -1 || cell2.Y == -1) throw new Exception("Point2 is not in grid!");
            return cell1.X == cell2.X && cell1.Y == cell2.Y;
        }

        public bool CheckPointsInNeighbourCell(AuthPointModel point1, AuthPointModel point2) {
            var cell1 = GetCellIndex(point1);
            var cell2 = GetCellIndex(point2);
            if (cell1.X == -1 || cell1.Y == -1) throw new Exception("Point1 is not in grid!");
            if (cell2.X == -1 || cell2.Y == -1) throw new Exception("Point2 is not in grid!");
            return Math.Abs(cell1.X - cell2.X) <= 1 && Math.Abs(cell1.Y - cell2.Y) <= 1;
        }
    }
}
