using Zenject;

namespace BorschtCraft
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}
