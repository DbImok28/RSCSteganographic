using RSCSteganographicMethod.ViemModules;

namespace RSCSteganographicMethod.Infrastructure.Services
{
    public class VMLocator
    {
        public MainViewModel MainVM => App.VMService.MainVM;
    }
}
