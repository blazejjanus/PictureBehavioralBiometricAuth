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
            Cells = new List<Rectangle>();
            for (int i = 0; i < image.Width; i += image.GridCellSize) {
                Cells.Add(new Rectangle(i, RowIndex * image.GridCellSize, image.GridCellSize, image.GridCellSize));
            }
        }

        private Rectangle GetCell(int x) {
            return Cells[x - 1];
        }

        public bool IsPointInCell(int rowIndex, AuthPointModel point) {
            var cell = GetCell(rowIndex);
            return cell.Contains(point.X, point.Y);
        }

        public bool IsPointInNeighbourCell(int rowIndex, AuthPointModel point) {
            var found = false;
            if(rowIndex > 1) {
                found = IsPointInCell(rowIndex - 1, point);
            }
            if(!found && rowIndex < Cells.Count) {
                found = IsPointInCell(rowIndex + 1, point);
            }
            return found;
        }

        public int GetCellIndex(AuthPointModel point) {
            for (int i = 0; i < Cells.Count; i++) {
                if (Cells[i].Contains(point.X, point.Y)) return i;
            }
            return -1;
        }
    }
}
