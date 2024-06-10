using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureBehavioralBiometricAuth.Db.Models {
    public class AuthPointModel {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; } = new UserModel();
        [Required]
        public int Number { get; set; } = 0;
        [Required]
        public int X { get; set; } = 0;
        [Required]
        public int Y { get; set; } = 0;
    }
}
