// --- Start of C# code for ConsumingLogicTests.cs ---
using NUnit.Framework;
using BorschtCraft.Food;
using BorschtCraft.Food.Core.Services.ConsumingService.Strategies;
using BorschtCraft.Food.Signals;
using System.Collections.Generic;
using System.Linq;
// Note: Zenject might not be available in a pure NUnit runner outside Unity.
// TestSignalBus is a manual stub. Real Zenject SignalBus might behave differently.
// using Zenject;
using UnityEngine; // For GameObject in MockItemSlot

public class TestSignalBus
{
    private Dictionary<System.Type, List<System.Delegate>> _subscriptions = new Dictionary<System.Type, List<System.Delegate>>();
    public List<object> FiredSignals = new List<object>();

    public void Subscribe<T>(System.Action<T> handler) where T : class
    {
        if (!_subscriptions.ContainsKey(typeof(T)))
        {
            _subscriptions[typeof(T)] = new List<System.Delegate>();
        }
        _subscriptions[typeof(T)].Add(handler);
    }

    public void Fire<T>(T signal) where T : class
    {
        FiredSignals.Add(signal);
        if (_subscriptions.ContainsKey(typeof(T)))
        {
            foreach (var sub in _subscriptions[typeof(T)])
            {
                ((System.Action<T>)sub)(signal);
            }
        }
    }
    public void ClearFiredSignals() { FiredSignals.Clear(); }
    public void Unsubscribe<T>(System.Action<T> handler) where T: class { /* Simplified for test stub */ }
    public void TryUnsubscribe<T>(System.Action<T> handler) where T: class { /* Simplified for test stub */ }
}

public class MockConsumable : IConsumable
{
    public System.Func<IConsumed, bool> CanDecorateFunc { get; set; } = item => false;
    public System.Func<IConsumed, IConsumed> ConsumeFunc { get; set; } = item => null;

    public bool CanDecorate(IConsumed item) => CanDecorateFunc(item);
    public IConsumed Consume(IConsumed item) => ConsumeFunc(item);
}

public class MockCantDecorateConsumable : MockConsumable, ICantDecorate { }

public class MockConsumed : IConsumed
{
    public IConsumed WrappedItem { get; set; }
    public IReadOnlyCollection<IConsumed> Ingredients { get; set; } = new List<IConsumed>().AsReadOnly();
    public int Price { get; set; }
    public virtual bool HasIngredientOfType<T>() where T : IConsumed => Ingredients.OfType<T>().Any();
    public string Name { get; set; }
    public override string ToString() => Name ?? GetType().Name;
}

public class MockCookableConsumed : MockConsumed, ICookable
{
    public float CookingTime { get; set; } = 0.01f;
    public System.Func<IConsumed> CookFunc { get; set; } = () => new MockCookedConsumed { Name = "CookedVersion" };
    public IConsumed Cook() => CookFunc();
}

public class MockCookedConsumed : MockConsumed, ICooked { }

public class MockItemSlot : IItemSlot
{
    private IConsumed _currentItem;
    public string Name { get; set; } = "MockSlot";
    public IConsumed GetCurrentItem() => _currentItem;
    public bool IsEmpty() => _currentItem == null;
    public bool TrySetItem(IConsumed newItem) { _currentItem = newItem; return true; }
    public IConsumed ReleaseItem() { var item = _currentItem; _currentItem = null; return item; }
    public GameObject GetGameObject() { return new GameObject(Name); }
    public override string ToString() => Name;
}

public class MockCombiningService : ICombiningService
{
    public System.Func<IConsumable, IConsumed> GetResultingItemFunc {get; set; }
    public System.Func<IConsumable, IItemSlot> GetDecoratedSlotFunc {get; set; }
    public System.Func<IConsumable, bool> AttemptCombinationFunc { get; set; } = decorator => false;

    public bool AttemptCombination(IConsumable decorator, out IConsumed resultingItem, out IItemSlot decoratedSlot)
    {
        bool success = AttemptCombinationFunc(decorator);
        if (success)
        {
            resultingItem = GetResultingItemFunc?.Invoke(decorator);
            decoratedSlot = GetDecoratedSlotFunc?.Invoke(decorator);
        }
        else
        {
            resultingItem = null;
            decoratedSlot = null;
        }
        return success;
    }
}

[TestFixture]
public class ConsumingLogicTests
{
    private TestSignalBus _testSignalBus;
    private InitialProductionStrategy _initialProductionStrategy;
    private DecorationStrategy _decorationStrategy;
    private MockCombiningService _mockCombiningService;
    private ConsumingService _consumingService;
    private List<IConsumptionStrategy> _strategiesList;

    [SetUp]
    public void SetUp()
    {
        _testSignalBus = new TestSignalBus();
        // Corrected: Pass the TestSignalBus instance, not the Zenject SignalBus type
        _initialProductionStrategy = new InitialProductionStrategy(_testSignalBus);

        _mockCombiningService = new MockCombiningService();
        _decorationStrategy = new DecorationStrategy(_mockCombiningService);

        _strategiesList = new List<IConsumptionStrategy> { _decorationStrategy, _initialProductionStrategy };
        _consumingService = new ConsumingService(_testSignalBus,
                                               new IItemSlot[] { new MockItemSlot { Name = "Cooking1" } },
                                               new IItemSlot[] { new MockItemSlot { Name = "Releasing1" } },
                                               _strategiesList);
        _consumingService.Initialize();
    }

    [Test]
    public void IPS_Success_ProducesCookable_FiresSignal()
    {
        var consumable = new MockConsumable
        {
            CanDecorateFunc = item => item == null,
            ConsumeFunc = item => item == null ? new MockCookableConsumed { Name = "RawFood" } : null
        };
        var slot = new MockItemSlot { Name = "EmptyCookingSlot" };
        var cookingSlots = new IItemSlot[] { slot };

        _testSignalBus.ClearFiredSignals();
        bool result = _initialProductionStrategy.TryExecute(consumable, cookingSlots, null, out var finalItem, out var finalSlot);

        Assert.IsTrue(result);
        Assert.IsNotNull(finalItem);
        Assert.IsInstanceOf<MockCookableConsumed>(finalItem);
        Assert.AreEqual(slot, finalSlot);
        Assert.AreEqual(finalItem, slot.GetCurrentItem());
        Assert.IsTrue(_testSignalBus.FiredSignals.OfType<CookItemInSlotRequestSignal>().Any(), "CookItemInSlotRequestSignal was not fired.");
    }

    [Test]
    public void IPS_Success_ProducesNonCookable_NoSignal()
    {
        var produced = new MockConsumed { Name = "NonCookable" };
        var consumable = new MockConsumable
        {
            CanDecorateFunc = item => item == null,
            ConsumeFunc = item => item == null ? produced : null
        };
        var slot = new MockItemSlot { Name = "EmptyCookingSlot" };
        var cookingSlots = new IItemSlot[] { slot };

        _testSignalBus.ClearFiredSignals();
        bool result = _initialProductionStrategy.TryExecute(consumable, cookingSlots, null, out var finalItem, out var finalSlot);

        Assert.IsTrue(result);
        Assert.AreEqual(produced, finalItem);
        Assert.IsFalse(_testSignalBus.FiredSignals.OfType<CookItemInSlotRequestSignal>().Any(), "CookItemInSlotRequestSignal was fired unexpectedly.");
    }

    [Test]
    public void IPS_Fails_CannotDecorateNull()
    {
        var consumable = new MockConsumable { CanDecorateFunc = item => false };
        var slot = new MockItemSlot();
        bool result = _initialProductionStrategy.TryExecute(consumable, new IItemSlot[] { slot }, null, out _, out _);
        Assert.IsFalse(result);
    }

    [Test]
    public void IPS_Fails_NoEmptySlot()
    {
        var consumable = new MockConsumable
        {
            CanDecorateFunc = item => item == null,
            ConsumeFunc = item => item == null ? new MockConsumed() : null
        };
        var fullSlot = new MockItemSlot();
        fullSlot.TrySetItem(new MockConsumed());
        bool result = _initialProductionStrategy.TryExecute(consumable, new IItemSlot[] { fullSlot }, null, out _, out _);
        Assert.IsFalse(result);
    }

    [Test]
    public void DS_Success_CallsCombiningService()
    {
        var consumable = new MockConsumable { Name = "Decorator" };
        var expectedItem = new MockConsumed { Name = "DecoratedItem" };
        var itemToDecorate = new MockConsumed { Name = "BaseItem" };
        var sourceSlot = new MockItemSlot { Name = "ItemSourceSlot" };
        sourceSlot.TrySetItem(itemToDecorate);

        _mockCombiningService.AttemptCombinationFunc = dec => dec == consumable;
        _mockCombiningService.GetResultingItemFunc = dec => expectedItem;
        _mockCombiningService.GetDecoratedSlotFunc = dec => sourceSlot;

        bool result = _decorationStrategy.TryExecute(consumable, null, new IItemSlot[] { sourceSlot }, out var actualItem, out var actualSlot);

        Assert.IsTrue(result);
        Assert.AreEqual(expectedItem, actualItem);
        Assert.AreEqual(sourceSlot, actualSlot);
    }

    [Test]
    public void DS_Fails_IsCantDecorate()
    {
        var cantDecorateConsumable = new MockCantDecorateConsumable();
        bool result = _decorationStrategy.TryExecute(cantDecorateConsumable, null, new IItemSlot[] { new MockItemSlot() }, out _, out _);
        Assert.IsFalse(result);
    }

    [Test]
    public void DS_Fails_CombiningServiceReturnsFalse()
    {
        var consumable = new MockConsumable { Name = "Decorator" };
        _mockCombiningService.AttemptCombinationFunc = dec => false;

        bool result = _decorationStrategy.TryExecute(consumable, null, new IItemSlot[] { new MockItemSlot() }, out var actualItem, out var actualSlot);

        Assert.IsFalse(result);
        Assert.IsNull(actualItem);
        Assert.IsNull(actualSlot);
    }

    [Test]
    public void CS_DecorationStrategyHandles()
    {
        var consumable = new MockConsumable { Name = "Decorator" };
        var decoratedItem = new MockConsumed { Name = "FinalDecorated" };
        var itemSlot = new MockItemSlot { Name = "TargetReleasingSlot" };
        itemSlot.TrySetItem(new MockConsumed { Name = "ItemToDecorate"});

        _mockCombiningService.AttemptCombinationFunc = dec => dec == consumable;
        _mockCombiningService.GetResultingItemFunc = dec => decoratedItem;
        _mockCombiningService.GetDecoratedSlotFunc = dec => itemSlot;

        var consumingService = new ConsumingService(_testSignalBus,
                               new IItemSlot[] { new MockItemSlot { Name = "Cooking1" } },
                               new IItemSlot[] { itemSlot },
                               new List<IConsumptionStrategy> { _decorationStrategy, _initialProductionStrategy });
        consumingService.Initialize();

        _testSignalBus.Fire(new ConsumableInteractionRequestSignal(consumable));

        Assert.AreEqual(decoratedItem, itemSlot.GetCurrentItem(), "Item in slot should be the decorated item.");
    }

    [Test]
    public void CS_InitialProductionStrategyHandles_WhenDecorationFails()
    {
        var producerConsumable = new MockConsumable { Name = "Producer" };
        producerConsumable.CanDecorateFunc = item => item == null;
        var producedItem = new MockConsumed { Name = "ProducedItem" };
        producerConsumable.ConsumeFunc = item => item == null ? producedItem : null;

        _mockCombiningService.AttemptCombinationFunc = dec => false; // Decoration fails

        var emptyCookingSlot = new MockItemSlot { Name = "EmptyCooking" };
        var consumingService = new ConsumingService(_testSignalBus,
                               new IItemSlot[] { emptyCookingSlot },
                               new IItemSlot[] { new MockItemSlot { Name = "ReleasingWithItem" } },
                               new List<IConsumptionStrategy> { _decorationStrategy, _initialProductionStrategy });
        consumingService.Initialize();

        _testSignalBus.Fire(new ConsumableInteractionRequestSignal(producerConsumable));

        Assert.AreEqual(producedItem, emptyCookingSlot.GetCurrentItem(), "Item in cooking slot should be the produced item.");
    }

    [Test]
    public void CS_NoStrategyHandles()
    {
        var nonHandlingConsumable = new MockConsumable { Name = "WontHandle" };
        nonHandlingConsumable.CanDecorateFunc = item => false;

        _mockCombiningService.AttemptCombinationFunc = dec => false;

        var emptyCookingSlot = new MockItemSlot { Name = "EmptyCooking" };
        var initialItemInSlot = emptyCookingSlot.GetCurrentItem();

        var consumingService = new ConsumingService(_testSignalBus,
                               new IItemSlot[] { emptyCookingSlot },
                               new IItemSlot[] { new MockItemSlot { Name = "ReleasingWithItem" } },
                               new List<IConsumptionStrategy> { _decorationStrategy, _initialProductionStrategy }); // Corrected type
        consumingService.Initialize();

        _testSignalBus.Fire(new ConsumableInteractionRequestSignal(nonHandlingConsumable));
        Assert.AreEqual(initialItemInSlot, emptyCookingSlot.GetCurrentItem(), "Cooking slot should remain unchanged.");
        // Add Unity LogAssert for warning when test runner supports it.
    }
}
// --- End of C# code for ConsumingLogicTests.cs ---
