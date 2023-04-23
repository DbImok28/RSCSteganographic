using RSCSteganographicMethod.ViemModules;

namespace RSCSteganographicMethod.Infrastructure.Services
{
    public class VMService
    {
        public VMService()
        {
            MainVM = new MainViewModel();
        }
        public MainViewModel MainVM { get; private set; }
    }
}
