using Microsoft.EntityFrameworkCore;
using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Shared.Config;

namespace PictureBehavioralBiometricAuth.Db {
    public class DataContext : DbContext {
        private readonly DbSettings _dbSettings;
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AuthImageModel> AuthImages { get; set; }
        public DbSet<AuthImageRegionModel> AuthImageRegions { get; set; }
        public DbSet<AuthPointModel> AuthPoints { get; set; }

        public DataContext() {
            var settings = AppSettings.ReadSettings();
            _dbSettings = settings.DbSettings;
        }

        public DataContext(DbSettings dbSettings) {
            _dbSettings = dbSettings;
        }

        public bool TestConnection() {
            try {
                Database.OpenConnection();
                Database.CloseConnection();
                return true;
            } catch {
                return false;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql(_dbSettings.ConnectionString);
            }
        }
    }
}
