using Zenject;

namespace BorschtCraft.Food
{
    public abstract class InstallerBase
    {
        protected DiContainer _container;

        public abstract void Install();

        public InstallerBase(DiContainer container)
        {
            _container = container;
        }
    }
}
