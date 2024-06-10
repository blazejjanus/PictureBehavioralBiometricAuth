using PictureBehavioralBiometricAuth.Db.Models;
using System.Collections.Generic;
using System.Drawing;

namespace PictureBehavioralBiometricAuth.Models {
    internal class Row {
        public List<Rectangle> Cells { get; private set; } = new List<Rectangle>();
        public int RowIndex { get; }

        public Row(int rowIndex, AuthImageModel image) {
            RowIndex = rowIndex;
            CalculateCells(image);
        }

        private void CalculateCells(AuthImageModel image) {
            for (int i = 0; i < image.Width; i += image.GridCellSize) {
                Cells.Add(new Rectangle(i, RowIndex * image.GridCellSize, image.GridCellSize, image.GridCellSize));
            }
        }

        public bool IsPointInCell(AuthPointModel point) {
            return GetCellIndex(point) != -1;
        }

        public int GetCellIndex(AuthPointModel point) {
            for (int i = 0; i < Cells.Count; i++) {
                if (Cells[i].Contains(point.X, point.Y)) return i;
            }
            return -1;
        }
    }
}
