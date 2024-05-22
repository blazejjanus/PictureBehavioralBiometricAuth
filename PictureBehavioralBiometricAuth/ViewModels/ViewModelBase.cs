using Prism.Mvvm;

namespace PictureBehavioralBiometricAuth.ViewModels;

public class ViewModelBase : BindableBase {
    protected readonly ApplicationContext _context;

    public ViewModelBase(ApplicationContext context) {
        _context = context;
    }
}
