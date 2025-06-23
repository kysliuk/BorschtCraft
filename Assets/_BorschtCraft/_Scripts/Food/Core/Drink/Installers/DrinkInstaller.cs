using Zenject;

namespace BorschtCraft.Food
{
    public class DrinkInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.DeclareSignal<GlassFilledSignal>();
            _container.DeclareSignal<FillGlassSignal>();
        }

        public DrinkInstaller(DiContainer container) : base(container)
        {
        }
    }
}
