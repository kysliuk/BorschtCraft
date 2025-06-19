using Zenject;

namespace BorschtCraft.Food.FirstTable
{
    public class ConsumedInstaller : InstallerBase
    {
        public override void Install()
        {
            InstallSignals();
        }

        private void InstallSignals()
        {
            GenericInstallSignal<BreadRaw>();
            GenericInstallSignal<BreadCooked>();
            GenericInstallSignal<Salo>();
            GenericInstallSignal<Garlic>();
            GenericInstallSignal<Horseradish>();
            GenericInstallSignal<Mustard>();
            GenericInstallSignal<Onion>();

        }

        private void GenericInstallSignal<T>() where T : IConsumed
        {
            _container.DeclareSignal<SlotItemChangedSignal<T>>();
        }

        public ConsumedInstaller(DiContainer container) : base(container)
        {
        }
    }
}
