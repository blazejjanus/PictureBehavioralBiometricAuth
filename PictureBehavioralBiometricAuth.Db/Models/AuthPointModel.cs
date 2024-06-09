using System.ComponentModel.DataAnnotations.Schema;

namespace PictureBehavioralBiometricAuth.Db.Models {
    public class AuthPointModel {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; } = new UserModel();
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
    }
}
