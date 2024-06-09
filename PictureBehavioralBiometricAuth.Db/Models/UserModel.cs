using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureBehavioralBiometricAuth.Db.Models {
    public class UserModel {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [ForeignKey("AuthImageId")]
        public AuthImageModel AuthImage { get; set; } = new AuthImageModel();
        public List<AuthPointModel> Points { get; set; } = new List<AuthPointModel>();
        public DateTime RegistrationTime { get; set; } = DateTime.Now;
        public DateTime? LastLoginTime { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
