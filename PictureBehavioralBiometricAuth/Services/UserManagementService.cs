using PictureBehavioralBiometricAuth.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureBehavioralBiometricAuth.Services {
    public class UserManagementService {
        private readonly ApplicationContext _context;

        public UserManagementService(ApplicationContext context) {
            _context = context;
        }

        public List<UserModel> GetUsers() {
            return _context.DbContext.Users.ToList();
        }

        public List<UserModel> SearchUsers(string search) {
            return _context.DbContext.Users.Where(x => IsSimilarSimple(x.Username, search, 75)).ToList();
        }

        public UserModel? GetUser(string username) {
            return _context.DbContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public void AddUser(UserModel user) {
            if (_context.DbContext.Users.Any(x => x.Username == user.Username)) throw new Exception("User with same username already exists!");
            _context.DbContext.Users.Add(user);
            _context.DbContext.SaveChanges();
        }

        public void DeleteUser(string username) {
            var user = _context.DbContext.Users.FirstOrDefault(x => x.Username == username);
            if (user == null) throw new Exception("User does not exist!");
            _context.DbContext.Users.Remove(user);
            _context.DbContext.SaveChanges();
        }

        private static bool IsSimilarSimple(string str1, string str2, int similarityToPass) {
            return GetSimilaritySimple(str1, str2) >= similarityToPass;
        }

        private static int GetSimilaritySimple(string str1, string str2) {
            if (str1 == null || str2 == null) return 0;
            if (str1.Length != str2.Length) return 0;
            if (str1 == str2) return 1;
            int equalChars = 0;
            for (int i = 0; i < str1.Length; i++) {
                if (str1[i] == str2[i]) equalChars++;
            }
            return (equalChars / str1.Length) * 100;
        }
    }
}
