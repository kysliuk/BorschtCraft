using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using System.ComponentModel;
using Zenject;

namespace BorschtCraft.Food.FirstTable
{
    public class ConsumableInstaller : InstallerBase
    {
        private readonly int _initialPrice;
        public override void Install()
        {
            InstallConsumables();
            InstallSignals();
        }

        private void InstallConsumables()
        {
            new GenericConsumableInstaller<BreadStack, BreadRaw>().Install(_container, _initialPrice);
            new GenericConsumableInstaller<SaloStack, Salo>().Install(_container, _initialPrice);
            new GenericConsumableInstaller<GarlicStack, Garlic>().Install(_container, _initialPrice);
            new GenericConsumableInstaller<HorseradishStack, Horseradish>().Install(_container, _initialPrice);
            new GenericConsumableInstaller<MustardStack, Mustard>().Install(_container, _initialPrice);
            new GenericConsumableInstaller<OnionStack, Onion>().Install(_container, _initialPrice);
        }

        private void InstallSignals()
        {
            _container.DeclareSignal<ConsumableInteractionRequestSignal>();
        }

        public ConsumableInstaller(DiContainer container, int initialPrice) : base(container)
        {
            _initialPrice = initialPrice;
        }
    }
}
