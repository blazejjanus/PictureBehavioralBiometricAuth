using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PictureBehavioralBiometricAuth.Db.Models {
    public class AuthImageRegionModel {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("AuthImageModelId")]
        public AuthImageModel AuthImageModel { get; set; } = new AuthImageModel();
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public bool ContainsPoint(AuthPointModel point) {
            return point.X >= X && point.X <= X + Width && point.Y >= Y && point.Y <= Y + Height;
        }
    }
}
