using Zenject;
using BorschtCraft.Food.Signals;
using UnityEngine;
using BorschtCraft.Food.UI;
using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEditor.Graphs;

namespace BorschtCraft.Food
{
    public class ConsumableInstaller : MonoInstaller
    {
        [RequiredMember]
        [SerializeField] ItemSlotController[] _cookingSlots;
        [RequiredMember]
        [SerializeField] ItemSlotController[] _releasingSlots;
        public override void InstallBindings()
        {
            //ViewMode mappings for consumable items
            var viewModelMappings = new ConsumedViewModelMapping[]
            {
                new ConsumedViewModelMapping(typeof(BreadRaw), typeof(BreadRawViewModel)),
                new ConsumedViewModelMapping(typeof(BreadCooked), typeof(BreadCookedViewModel)),
                new ConsumedViewModelMapping(typeof(Salo), typeof(ConsumedViewModel<Salo>)),
                new ConsumedViewModelMapping(typeof(Garlic), typeof(ConsumedViewModel<Garlic>)),
                new ConsumedViewModelMapping(typeof(Onion), typeof(ConsumedViewModel<Onion>)),
                new ConsumedViewModelMapping(typeof(Mustard), typeof(ConsumedViewModel<Mustard>)),
                new ConsumedViewModelMapping(typeof(Horseradish), typeof(ConsumedViewModel<Horseradish>))
            };

            //Bind the view model mappings to the container
            Container.Bind<ConsumedViewModelMapping[]>()
                .FromInstance(viewModelMappings).AsSingle();

            //Bind the consumable items
            Container.Bind<BreadRawViewModel>().AsTransient();
            Container.Bind<BreadCookedViewModel>().AsTransient();
            Container.Bind<ConsumedViewModel<Salo>>().AsTransient();
            Container.Bind<ConsumedViewModel<Garlic>>().AsTransient();
            Container.Bind<ConsumedViewModel<Onion>>().AsTransient();
            Container.Bind<ConsumedViewModel<Mustard>>().AsTransient();
            Container.Bind<ConsumedViewModel<Horseradish>>().AsTransient();

            //Bind the item slot controllers for cooking and releasing
            foreach (var slot in _cookingSlots) Container.QueueForInject(slot);
            Container.Bind<ItemSlotController[]>()
                    .WithId("CookingSlots")
                    .FromInstance(_cookingSlots)
                    .AsCached();

            // Bind the releasing slots
            foreach (var slot in _releasingSlots) Container.QueueForInject(slot);
            Container.Bind<ItemSlotController[]>()
                    .WithId("ReleasingSlots")
                    .FromInstance(_releasingSlots)
                    .AsCached();

            //Bind the cooking service
            Container.Bind<ICookingService>().To<CookingService>().AsSingle().NonLazy();

            //Bind Signals
            Container.DeclareSignal<ConsumableInteractionRequestSignal<BreadStack, BreadRaw>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<SaloStack, Salo>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<GarlicStack, Garlic>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<OnionStack, Onion>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<MustardStack, Mustard>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<HorseradishStack, Horseradish>>();

            Container.DeclareSignal<CookItemInSlotRequestSignal>();
            Container.DeclareSignal<ItemCookedSignal>();
        }
    }
}
