using Zenject;
using BorschtCraft.Food.Signals;
using UnityEngine;
using BorschtCraft.Food.UI;
using System.Collections.Generic;
using BorschtCraft.Food.Handlers;
using System.Linq;

namespace BorschtCraft.Food
{
    public class ConsumableInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindViewModelMappings();
            BindViewModels();
            BindServicesAndHandlers();
            BindSignals();

            Container.Bind<IItemSlot[]>()
                .WithId("CookingSlots")
                .FromMethod(ResolveSlotsOfType(SlotType.Cooking))
                .AsCached().Lazy();

            Container.Bind<IItemSlot[]>()
                .WithId("ReleasingSlots")
                .FromMethod(ResolveSlotsOfType(SlotType.Releasing))
                .AsCached().Lazy();
        }

        private System.Func<InjectContext, IItemSlot[]> ResolveSlotsOfType(SlotType type)
        {
            return (ctx) =>
            {
                var installers = ctx.Container.Resolve<SceneContext>().GetComponentsInChildren<ItemSlotInstaller>();

                var slots = installers
                    .Where(inst => inst != null && inst.gameObject.activeInHierarchy)
                    .Select(inst => {
                        var goContext = inst.GetComponent<GameObjectContext>();

                        if (goContext == null || goContext.Container == null)
                        {
                            Debug.LogWarning($"ItemSlotInstaller on {inst.name} is missing a GameObjectContext or its container is not ready.", inst.gameObject);
                            return null;
                        }

                        return goContext.Container.Resolve<IItemSlot>();
                    })
                    .Where(slot => slot != null && slot.Type == type) 
                    .ToArray();

                Logger.LogInfo(this, $"Successfully resolved {slots.Length} slots of type '{type}'.");
                return slots;
            };
        }


        #region Helper Binding Methods (Unchanged)
        private void BindServicesAndHandlers()
        {
            Container.BindInterfacesAndSelfTo<CookingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CombiningService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemTransferService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ConsumingService>().AsSingle().NonLazy();
            Container.Bind<MonoBehaviour>().WithId("CoroutineHost").FromInstance(this).AsSingle();
            Container.Bind<ProductionHandler>().AsSingle();
            Container.Bind<DecorationHandler>().AsSingle();
            Container.Bind<IConsumableHandler>().FromMethod(ctx => {
                var decorationHandler = ctx.Container.Resolve<DecorationHandler>();
                var productionHandler = ctx.Container.Resolve<ProductionHandler>();
                decorationHandler.SetNext(productionHandler);
                return decorationHandler;
            }).AsSingle();
        }
        private void BindViewModelMappings()
        {
            var viewModelMappings = new List<ConsumedViewModelMapping> {
                new ConsumedViewModelMapping(typeof(BreadRaw), typeof(BreadRawViewModel)),
                new ConsumedViewModelMapping(typeof(BreadCooked), typeof(BreadCookedViewModel)),
                new ConsumedViewModelMapping(typeof(Salo), typeof(ConsumedViewModel<Salo>)),
                new ConsumedViewModelMapping(typeof(Garlic), typeof(ConsumedViewModel<Garlic>)),
                new ConsumedViewModelMapping(typeof(Onion), typeof(ConsumedViewModel<Onion>)),
                new ConsumedViewModelMapping(typeof(Mustard), typeof(ConsumedViewModel<Mustard>)),
                new ConsumedViewModelMapping(typeof(Horseradish), typeof(ConsumedViewModel<Horseradish>))
            };
            Container.Bind<List<ConsumedViewModelMapping>>().FromInstance(viewModelMappings).AsSingle();
        }
        private void BindViewModels()
        {
            Container.Bind<BreadRawViewModel>().AsTransient();
            Container.Bind<BreadCookedViewModel>().AsTransient();
            Container.Bind<ConsumedViewModel<Salo>>().AsTransient();
            Container.Bind<ConsumedViewModel<Garlic>>().AsTransient();
            Container.Bind<ConsumedViewModel<Onion>>().AsTransient();
            Container.Bind<ConsumedViewModel<Mustard>>().AsTransient();
            Container.Bind<ConsumedViewModel<Horseradish>>().AsTransient();
        }
        private void BindSignals()
        {
            Container.DeclareSignal<ConsumableInteractionRequestSignal>();
            Container.DeclareSignal<CookItemInSlotRequestSignal>();
            Container.DeclareSignal<ItemCookedSignal>();
            Container.DeclareSignal<SlotClickedSignal>();
        }
        #endregion
    }
}