using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureBehavioralBiometricAuth.Db.Models {
    public class AuthImageModel {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public List<AuthImageRegionModel> Regions { get; set; } = new List<AuthImageRegionModel>();
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int GridCellSize { get; set; } = 0;

        
    }
}
