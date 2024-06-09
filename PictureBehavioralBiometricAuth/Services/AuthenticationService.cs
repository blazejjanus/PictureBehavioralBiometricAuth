using PictureBehavioralBiometricAuth.Db.Models;
using System.Linq;

namespace PictureBehavioralBiometricAuth.Services {
    public class AuthenticationService {
        private ApplicationContext _context;

        public AuthenticationService(ApplicationContext context) {
            _context = context;
        }

        public AuthImageModel? GetAuthImage(string name) {
            var image = _context.DbContext.AuthImages.FirstOrDefault(x => x.Name == name);
            if(image != null) {
                image.Regions = _context.DbContext.AuthImageRegions.Where(x => x.AuthImageModel.Id == image.Id).ToList();
            }
            return image;
        }

        public void AddAuthImage(AuthImageModel authImage) {
            if(_context.DbContext.AuthImages.Any(x => x.Name == authImage.Name)) throw new System.Exception("Auth image with same name already exists!");
            _context.DbContext.AuthImages.Add(authImage);
            _context.DbContext.SaveChanges();
        }

        public bool IsAuthImageCreated(string name) {
            return _context.DbContext.AuthImages.Any(x => x.Name == name);
        }
    }
}
