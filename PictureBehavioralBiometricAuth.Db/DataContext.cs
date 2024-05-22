using Microsoft.EntityFrameworkCore;
using PictureBehavioralBiometricAuth.Db.Models;

namespace PictureBehavioralBiometricAuth.Db {
    public class DataContext : DbContext {
        private readonly DbSettings _dbSettings;
        public DbSet<UserModel> Users { get; set; }

        public DataContext() {
            _dbSettings = DbSettings.ReadDbSettings();
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
