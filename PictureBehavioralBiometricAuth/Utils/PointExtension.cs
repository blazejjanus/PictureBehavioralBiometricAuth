using Avalonia;
using PictureBehavioralBiometricAuth.Db.Models;

namespace PictureBehavioralBiometricAuth.Utils {
    internal static class PointExtension {
        public static AuthPointModel GetAuthPoint(this Point point) {
            return new AuthPointModel {
                X = (int) point.X,
                Y = (int) point.Y
            };
        }
    }
}
