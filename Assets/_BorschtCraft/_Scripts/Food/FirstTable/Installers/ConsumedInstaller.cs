using BorschtCraft.Food.UI;
using System.ComponentModel;
using Zenject;

namespace BorschtCraft.Food.FirstTable
{
    public class ConsumedInstaller : InstallerBase
    {
        public override void Install()
        {
            InstallConsumed();
            InstallSignals();
        }

        private void InstallConsumed()
        {
            new GenericConsumedInstaller<BreadRaw>().Install(_container);
            new GenericConsumedInstaller<BreadCooked>().Install(_container);
            new GenericConsumedInstaller<Salo>().Install(_container);
            new GenericConsumedInstaller<Garlic>().Install(_container);
            new GenericConsumedInstaller<Horseradish>().Install(_container);
            new GenericConsumedInstaller<Mustard>().Install(_container);
            new GenericConsumedInstaller<Onion>().Install(_container);
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
