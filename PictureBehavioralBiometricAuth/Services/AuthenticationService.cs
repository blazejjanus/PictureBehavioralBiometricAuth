using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureBehavioralBiometricAuth.Services {
    public class AuthenticationService {
        public const int MAX_OUT_OF_ORDER_ITEMS = 2;
        //A - variable
        public const double POINTS_IN_SAME_CELL_FACTOR = 1;
        public const double POINTS_IN_NEIGHBOUR_CELL_FACTOR = 0.5;
        public const double POINTS_IN_OTHER_CELLS_FACTOR = 0;
        //B - variable
        public const double POINTS_IN_SAME_REGION_FACTOR = 1;
        public const double POINTS_IN_DIFFERENT_REGIONS_FACTOR = 0;
        //C - variable
        public const double POINTS_IN_SAME_ORDER_FACTOR = 1;
        public const double POINTS_IN_DIFFERENT_ORDER_FACTOR = 0;

        private ApplicationContext _context;
        private UserManagementService _userManagementService;

        public AuthenticationService(ApplicationContext context, UserManagementService userManagementService) {
            _context = context;
            _userManagementService = userManagementService;
        }

        public AuthImageModel? GetAuthImage(string name) {
            var image = _context.DbContext.AuthImages.FirstOrDefault(x => x.Name == name);
            if(image != null) {
                image.Regions = _context.DbContext.AuthImageRegions.Where(x => x.AuthImageModel.Id == image.Id).ToList();
            }
            return image;
        }

        public void AddAuthImage(AuthImageModel authImage) {
            if(_context.DbContext.AuthImages.Any(x => x.Name == authImage.Name)) throw new Exception("Auth image with same name already exists!");
            _context.DbContext.AuthImages.Add(authImage);
            _context.DbContext.SaveChanges();
        }

        public bool IsAuthImageCreated(string name) {
            return _context.DbContext.AuthImages.Any(x => x.Name == name);
        }

        public bool LoginUser(string username, string imageName, List<AuthPointModel> points, out int detectedSimilarity) {
            detectedSimilarity = 0;
            var user = _userManagementService.GetUser(username);
            if(user == null) throw new Exception("User does not exist!");
            if(user.AuthImage.Name != imageName) return false;
            if(user.Points.Count != points.Count) return false;
            if (user.AuthImage.Regions == null || user.AuthImage.Regions.Count == 0) throw new Exception("Provided image does not have any regions defined.");
            var similarity = GetPointsSimilarityFactor(user, points);
            detectedSimilarity = similarity;
            return similarity >= _context.Settings.LoginPassThreshold;
        }

        public int GetPointsSimilarityFactor(UserModel user, List<AuthPointModel> points) {
            var grid = new Grid(user.AuthImage);
            double similarity = 0;
            int outOfOrderItems = 0;
            foreach(var point in points) {
                double pointSimilarity = 0;
                double a = POINTS_IN_OTHER_CELLS_FACTOR;
                double b = POINTS_IN_DIFFERENT_REGIONS_FACTOR;
                double c = POINTS_IN_DIFFERENT_ORDER_FACTOR;
                foreach(var userPoint in user.Points) {
                    if (grid.CheckPointsInSameCell(point, userPoint)) {
                        a = POINTS_IN_SAME_CELL_FACTOR;
                        b = GetPointRegionSimilarityFactor(user.AuthImage, point, userPoint);
                        c = GetPointsOrderSimilarityFactor(point, userPoint);
                        break;
                    } else if (grid.CheckPointsInNeighbourCell(point, userPoint)) {
                        a = POINTS_IN_NEIGHBOUR_CELL_FACTOR;
                        b = GetPointRegionSimilarityFactor(user.AuthImage, point, userPoint);
                        c = GetPointsOrderSimilarityFactor(point, userPoint);
                    } else {
                        continue;
                    }
                }
                if(c == POINTS_IN_DIFFERENT_ORDER_FACTOR) {
                    outOfOrderItems++;
                    if(outOfOrderItems > MAX_OUT_OF_ORDER_ITEMS) return 0;
                }
                pointSimilarity = GetPointsSimilarity(a, b, c);
                similarity += pointSimilarity;
            }
            return (int)(similarity / GetMaxPointSimilarity() * 100);
        }

        private double GetPointsSimilarity(double a, double b, double c) {
            return a * b * c;
        }

        private double GetMaxPointSimilarity() {
            return GetPointsSimilarity(POINTS_IN_SAME_CELL_FACTOR, POINTS_IN_SAME_REGION_FACTOR, POINTS_IN_SAME_ORDER_FACTOR);
        }

        private double GetPointsOrderSimilarityFactor(AuthPointModel testPoint, AuthPointModel authPoint) {
            if(testPoint.Number == authPoint.Number) {
                return POINTS_IN_SAME_ORDER_FACTOR;
            } else {
                return POINTS_IN_DIFFERENT_ORDER_FACTOR;
            }
        }

        private double GetPointRegionSimilarityFactor(AuthImageModel image, AuthPointModel testPoint, AuthPointModel authPoint) {
            if(CheckPointsInSameRegion(image, testPoint, authPoint)) {
                return POINTS_IN_SAME_REGION_FACTOR;
            } else {
                return POINTS_IN_DIFFERENT_REGIONS_FACTOR;
            }
        }

        private bool CheckPointsInSameRegion(AuthImageModel image, AuthPointModel testPoint, AuthPointModel authPoint) {
            foreach(var region in image.Regions) {
                if(region.ContainsPoint(testPoint) && region.ContainsPoint(authPoint)) return true;
            }
            return false;
        }
    }
}
