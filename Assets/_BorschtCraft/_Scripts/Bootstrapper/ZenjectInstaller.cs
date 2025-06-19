using BorschtCraft.Food;
using UnityEngine;
using Zenject;

namespace BorschtCraft
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<MonoBehaviour>().WithId("CoroutineHost").FromInstance(this).AsSingle();
        }
    }
}
