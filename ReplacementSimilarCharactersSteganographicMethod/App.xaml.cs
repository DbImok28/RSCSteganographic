using RSCSteganographicMethod.Infrastructure.Services;
using System.Windows;

namespace RSCSteganographicMethod
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static VMService VMService { get; } = new VMService();
    }
}
