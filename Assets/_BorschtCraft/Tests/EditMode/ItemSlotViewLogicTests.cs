// --- Start of C# code for ItemSlotViewLogicTests.cs ---
using NUnit.Framework;
using BorschtCraft.Food;
using BorschtCraft.Food.UI;
using BorschtCraft.Food.UI.DisplayLogic;
using BorschtCraft.Food.UI.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System; // For Type
// Using Zenject specific types for constructor matching in tests where necessary.
using Zenject;


// --- Mock/Stub Implementations (minimal for these tests) ---

public class LayeredTestItem : Consumed
{
    public LayeredTestItem(string name, int price, IConsumed wrapped = null) : base(price, wrapped)
    {
        Name = name;
    }
    public string Name { get; }
    public override string ToString() => Name;
}

public class CookableLayeredItem : LayeredTestItem, ICookable
{
    public CookableLayeredItem(string name, int price, IConsumed wrapped = null) : base(name, price, wrapped) {}
    public float CookingTime { get; set; } = 1f;
    public IConsumed Cook() { return new CookedLayeredItem("Cooked_" + Name, Price, WrappedItem); }
}

public class CookedLayeredItem : LayeredTestItem, ICooked
{
    public CookedLayeredItem(string name, int price, IConsumed wrapped = null) : base(name, price, wrapped) {}
}

// Added BreadRaw stub as required by new tests
public class BreadRaw : Consumed, ICookable
{
    public BreadRaw(int price, IConsumed wrapped = null) : base(price, wrapped) { Name = "BreadRaw"; }
    public string Name { get; set; }
    public float CookingTime { get; set; } = 1f;
    public IConsumed Cook() { return new CookedLayeredItem("CookedBread", Price, WrappedItem); } // Assuming CookedLayeredItem can represent cooked bread
    public override string ToString() => Name;
}

// MockCookedConsumed stub for BreadRaw's Cook() method, if not using CookedLayeredItem
public class MockCookedConsumed : Consumed, ICooked // Make sure this derives from Consumed or a common base
{
    public MockCookedConsumed() : base(0, null) {} // Base constructor call
    public string Name { get; set; } = "MockCooked";
    // ICooked members if any
}


public class StubManagedConsumedView : IManagedConsumedView
{
    public IConsumedViewModel AttachedViewModel { get; private set; }
    public bool IsVisible { get; private set; }
    public int DetachCalledCount { get; private set; } = 0;
    public Type ConsumedModelType { get; set; }

    public void AttachViewModel(IConsumedViewModel viewModel)
    {
        AttachedViewModel = viewModel;
    }

    public void DetachViewModel()
    {
        AttachedViewModel = null;
        IsVisible = false;
        DetachCalledCount++;
    }
    public Type GetConsumedModelType() => ConsumedModelType;
}

public class StubConsumedViewModel : IConsumedViewModel
{
    public IConsumed Model { get; }
    public object SignalBusStub { get; }
    public bool IsVisible { get; private set; }
    public int SetVisibilityCount { get; private set; } = 0;

    public StubConsumedViewModel(IConsumed model, SignalBus signalBus)
    {
        Model = model;
        SignalBusStub = signalBus;
    }
    public void SetVisibility(bool visible) { IsVisible = visible; SetVisibilityCount++; }
    public void OnSlotClicked() {}
    public void OnActionSpecificToThisVM() {}
}

public class StubDiContainerForFactoryTests
{
    public System.Func<Type, object[], object> InstantiateFunc { get; set; }
    public object Instantiate(Type type, IEnumerable<object> extraArgs)
    {
        return InstantiateFunc?.Invoke(type, extraArgs.ToArray());
    }
}


[TestFixture]
public class ItemLayerProcessorTests
{
    private ItemLayerProcessor _processor;

    [SetUp]
    public void SetUp()
    {
        _processor = new ItemLayerProcessor();
    }

    [Test]
    public void GetLayers_NullItem_ReturnsEmptyList()
    {
        // Pass a default SlotType, e.g., Unknown, as it's now required
        var layers = _processor.GetLayersToDisplay(null, SlotType.Unknown);
        Assert.IsEmpty(layers);
    }

    [Test]
    public void GetLayers_SingleItem_ReturnsItemItself()
    {
        var item = new LayeredTestItem("Single", 10);
        var layers = _processor.GetLayersToDisplay(item, SlotType.Unknown); // Added SlotType.Unknown
        Assert.AreEqual(1, layers.Count);
        Assert.Contains(item, layers);
    }

    [Test]
    public void GetLayers_WrappedItems_ReturnsAllLayersInOrder()
    {
        var bottom = new LayeredTestItem("Bottom", 5);
        var middle = new LayeredTestItem("Middle", 3, bottom);
        var top = new LayeredTestItem("Top", 2, middle);

        var layers = _processor.GetLayersToDisplay(top, SlotType.Unknown); // Added SlotType.Unknown
        Assert.AreEqual(3, layers.Count);
        Assert.AreEqual(top, layers[0]);
        Assert.AreEqual(middle, layers[1]);
        Assert.AreEqual(bottom, layers[2]);
    }

    [Test]
    public void GetLayers_CookedTop_CookableMiddle_RawBottom_SuppressesCookableMiddle()
    {
        var bottom = new LayeredTestItem("BottomRaw", 5);
        var middleCookable = new CookableLayeredItem("MiddleCookable", 3, bottom);
        var topCooked = new CookedLayeredItem("TopCooked", 2, middleCookable);

        var layers = _processor.GetLayersToDisplay(topCooked, SlotType.Cooking); // Added SlotType.Cooking

        Assert.AreEqual(2, layers.Count, "Should display top and bottom, suppress middle.");
        Assert.Contains(topCooked, layers);
        Assert.Contains(bottom, layers);
        Assert.IsFalse(layers.Contains(middleCookable));
    }

    [Test]
    public void GetLayers_CookableTop_RawBottom_ShowsBoth()
    {
        var bottom = new LayeredTestItem("BottomRaw", 5);
        var topCookable = new CookableLayeredItem("TopCookable", 3, bottom);

        var layers = _processor.GetLayersToDisplay(topCookable, SlotType.Cooking); // Added SlotType.Cooking
        Assert.AreEqual(2, layers.Count);
        Assert.Contains(topCookable, layers);
        Assert.Contains(bottom, layers);
    }

    [Test]
    public void GetLayers_MaxLayerSafetyLimit()
    {
        IConsumed current = new LayeredTestItem("Leaf", 1);
        for (int i = 0; i < 15; i++)
        {
            current = new LayeredTestItem("Layer" + i, 1, current);
        }
        var layers = _processor.GetLayersToDisplay(current, SlotType.Unknown); // Added SlotType.Unknown
        Assert.LessOrEqual(layers.Count, 10);
    }

    // New tests for BreadRaw filtering
    [Test]
    public void GetLayers_ReleasingSlot_WithBreadRaw_FiltersBreadRaw()
    {
        var breadRaw = new BreadRaw(5);
        var otherItem = new LayeredTestItem("Other", 3, breadRaw);
        var topItem = new LayeredTestItem("Top", 2, otherItem);

        var layers = _processor.GetLayersToDisplay(topItem, SlotType.Releasing);

        Assert.IsFalse(layers.Any(layer => layer is BreadRaw), "BreadRaw should be filtered out for Releasing slots.");
        Assert.IsTrue(layers.Contains(topItem), "TopItem should still be present.");
        Assert.IsTrue(layers.Contains(otherItem), "OtherItem should still be present.");
        Assert.AreEqual(2, layers.Count);
    }

    [Test]
    public void GetLayers_ReleasingSlot_OnlyBreadRaw_ReturnsEmpty()
    {
        var breadRaw = new BreadRaw(5);
        var layers = _processor.GetLayersToDisplay(breadRaw, SlotType.Releasing);
        Assert.IsEmpty(layers, "Should be empty if only BreadRaw in Releasing slot.");
    }

    [Test]
    public void GetLayers_CookingSlot_WithBreadRaw_DoesNotFilterBreadRawBasedOnReleasingRule()
    {
        var breadRaw = new BreadRaw(5);
        var topItem = new LayeredTestItem("Top", 2, breadRaw);

        var layers = _processor.GetLayersToDisplay(topItem, SlotType.Cooking);

        Assert.IsTrue(layers.Any(layer => layer is BreadRaw), "BreadRaw should NOT be filtered out for Cooking slots by the releasing rule.");
        Assert.AreEqual(2, layers.Count);
    }
     [Test]
    public void GetLayers_UnknownSlot_WithBreadRaw_DoesNotFilterBreadRawBasedOnReleasingRule()
    {
        var breadRaw = new BreadRaw(5);
        var topItem = new LayeredTestItem("Top", 2, breadRaw);

        var layers = _processor.GetLayersToDisplay(topItem, SlotType.Unknown);

        Assert.IsTrue(layers.Any(layer => layer is BreadRaw), "BreadRaw should NOT be filtered out for Unknown slots by the releasing rule.");
        Assert.AreEqual(2, layers.Count);
    }
}

[TestFixture]
public class ViewModelFactoryTests // No changes needed here based on current subtask
{
    private ViewModelFactory _factory;
    private StubDiContainerForFactoryTests _stubDiContainer;
    private List<ConsumedViewModelMapping> _mappings;

    [SetUp]
    public void SetUp()
    {
        _stubDiContainer = new StubDiContainerForFactoryTests();
        _mappings = new List<ConsumedViewModelMapping>();
        _factory = new ViewModelFactory(_stubDiContainer, null, _mappings);
    }

    [Test]
    public void CreateVM_NullItem_ReturnsNull()
    {
        var vm = _factory.CreateViewModel(null);
        Assert.IsNull(vm);
    }

    [Test]
    public void CreateVM_MappingFound_InstantiatesAndReturnsVM()
    {
        var item = new LayeredTestItem("TestItem", 10);
        var mapping = new ConsumedViewModelMapping { ConsumedModelType = typeof(LayeredTestItem), ViewModelType = typeof(StubConsumedViewModel) };
        _mappings.Add(mapping);

        _stubDiContainer.InstantiateFunc = (type, args) =>
        {
            if (type == typeof(StubConsumedViewModel))
            {
                return new StubConsumedViewModel(args[0] as IConsumed, args[1] as SignalBus);
            }
            return null;
        };

        var vm = _factory.CreateViewModel(item) as StubConsumedViewModel;
        Assert.IsNotNull(vm);
        Assert.AreEqual(item, vm.Model);
        Assert.IsNull(vm.SignalBusStub);
    }

    [Test]
    public void CreateVM_NoMappingFound_ReturnsNull_LogsError()
    {
        var item = new LayeredTestItem("UnmappedItem", 10);
        var vm = _factory.CreateViewModel(item);
        Assert.IsNull(vm);
    }

    [Test]
    public void CreateVM_DiContainerFailsToInstantiate_ReturnsNull_LogsError()
    {
        var item = new LayeredTestItem("TestItem", 10);
        var mapping = new ConsumedViewModelMapping { ConsumedModelType = typeof(LayeredTestItem), ViewModelType = typeof(StubConsumedViewModel) };
        _mappings.Add(mapping);
        _stubDiContainer.InstantiateFunc = (type, args) => { throw new System.InvalidOperationException("DI fail"); };

        var vm = _factory.CreateViewModel(item);
        Assert.IsNull(vm);
    }

    [Test]
    public void CreateVM_InstantiatedObjectNotIConsumedViewModel_ReturnsNull_LogsError()
    {
        var item = new LayeredTestItem("TestItem", 10);
        var mapping = new ConsumedViewModelMapping { ConsumedModelType = typeof(LayeredTestItem), ViewModelType = typeof(StubConsumedViewModel) };
        _mappings.Add(mapping);
        _stubDiContainer.InstantiateFunc = (type, args) => new object();

        var vm = _factory.CreateViewModel(item);
        Assert.IsNull(vm);
    }
}

[TestFixture]
public class ItemSlotViewManagerTests
{
    private ItemSlotViewManager _viewManager;
    private StubItemLayerProcessor _stubItemLayerProcessor; // Updated Stub
    private StubViewModelFactory _stubViewModelFactory;
    private Dictionary<Type, IManagedConsumedView> _childViews;
    private Transform _mockSlotTransform;

    // Updated StubItemLayerProcessor to handle SlotType and store received context
    public class StubItemLayerProcessor : IItemLayerProcessor
    {
        public Func<IConsumed, SlotType, List<IConsumed>> GetLayersToDisplayFunc { get; set; } = (item, slotType) => new List<IConsumed>();
        public SlotType ReceivedSlotType { get; private set; }

        public List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem, SlotType slotContext)
        {
            ReceivedSlotType = slotContext;
            return GetLayersToDisplayFunc(overallRootItem, slotContext);
        }
    }

    public class StubViewModelFactory : IViewModelFactory
    {
        public Func<IConsumed, IConsumedViewModel> CreateViewModelFunc { get; set; } = item => null;
        public IConsumedViewModel CreateViewModel(IConsumed itemLayer) => CreateViewModelFunc(itemLayer);
    }

    [SetUp]
    public void SetUp()
    {
        _viewManager = new ItemSlotViewManager();
        _stubItemLayerProcessor = new StubItemLayerProcessor();
        _stubViewModelFactory = new StubViewModelFactory();
        _childViews = new Dictionary<Type, IManagedConsumedView>();
        _mockSlotTransform = new GameObject("MockSlotTransform").transform;

        // Initialize with a default SlotType, e.g., Cooking or Unknown
        _viewManager.Initialize(_mockSlotTransform, _childViews, _stubItemLayerProcessor, _stubViewModelFactory, SlotType.Cooking);
    }

    [TearDown]
    public void TearDown()
    {
        if (_mockSlotTransform != null && _mockSlotTransform.gameObject != null) GameObject.DestroyImmediate(_mockSlotTransform.gameObject);
    }

    [Test]
    public void Initialize_DetachesViewModelsFromChildViewsAndSetsSlotType() // Name implies SlotType check, but Initialize doesn't expose it directly
    {
        var view1 = new StubManagedConsumedView();
        _childViews[typeof(LayeredTestItem)] = view1;
        // Re-initialize to test detachment and ensure SlotType is passed (verified in DisplayItem_CallsLayerProcessorWithCorrectSlotType)
        _viewManager.Initialize(_mockSlotTransform, _childViews, _stubItemLayerProcessor, _stubViewModelFactory, SlotType.Releasing);
        Assert.AreEqual(1, view1.DetachCalledCount);
    }

    [Test]
    public void DisplayItem_CallsLayerProcessorWithCorrectSlotType()
    {
        var rootItem = new LayeredTestItem("Root", 1);
        var expectedSlotType = SlotType.Releasing;
        // Re-initialize ItemSlotViewManager with the specific SlotType for this test
        _viewManager.Initialize(_mockSlotTransform, _childViews, _stubItemLayerProcessor, _stubViewModelFactory, expectedSlotType);

        // The GetLayersToDisplayFunc on the stub will be called, and our stub now records the slotType.
        _viewManager.DisplayItem(rootItem, rootItem);

        Assert.AreEqual(expectedSlotType, _stubItemLayerProcessor.ReceivedSlotType, "SlotType passed to layer processor is incorrect.");
    }


    [Test]
    public void DisplayItem_NullItem_ClearsAndReturns() // SlotType passed to GetLayersToDisplay won't matter here
    {
        var view1 = new StubManagedConsumedView();
        var vm1 = new StubConsumedViewModel(new LayeredTestItem("Old",1), null);
        view1.AttachViewModel(vm1);
        _childViews[typeof(LayeredTestItem)] = view1;

        _viewManager.DisplayItem(null, null); // overallRootItem is also null
        Assert.AreEqual(1, view1.DetachCalledCount);
        Assert.IsNull(view1.AttachedViewModel);
    }

    [Test]
    public void DisplayItem_ProcessorReturnsLayers_FactoryCreatesVMs_ViewsAreAttached() // Uses SlotType from SetUp
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        var itemLayer2 = new CookableLayeredItem("Layer2", 5);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = (item, slotType) => new List<IConsumed> { itemLayer1, itemLayer2 };

        var vm1 = new StubConsumedViewModel(itemLayer1, null);
        var vm2 = new StubConsumedViewModel(itemLayer2, null);
        _stubViewModelFactory.CreateViewModelFunc = layer => layer == itemLayer1 ? vm1 : (layer == itemLayer2 ? vm2 : null);

        var view1 = new StubManagedConsumedView() { ConsumedModelType = typeof(LayeredTestItem) };
        var view2 = new StubManagedConsumedView() { ConsumedModelType = typeof(CookableLayeredItem) };
        _childViews[typeof(LayeredTestItem)] = view1;
        _childViews[typeof(CookableLayeredItem)] = view2;

        _viewManager.DisplayItem(new LayeredTestItem("Root", 1), new LayeredTestItem("Root", 1));

        Assert.AreEqual(vm1, view1.AttachedViewModel);
        Assert.IsTrue(vm1.IsVisible);
        Assert.AreEqual(1, vm1.SetVisibilityCount);

        Assert.AreEqual(vm2, view2.AttachedViewModel);
        Assert.IsTrue(vm2.IsVisible);
        Assert.AreEqual(1, vm2.SetVisibilityCount);
    }

    [Test]
    public void DisplayItem_ProcessorReturnsLayer_NoViewForLayer_LogsWarning() // Uses SlotType from SetUp
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = (item, slotType) => new List<IConsumed> { itemLayer1 };
        Assert.DoesNotThrow(() => _viewManager.DisplayItem(new LayeredTestItem("Root",1), new LayeredTestItem("Root",1)));
    }

    [Test]
    public void DisplayItem_ProcessorReturnsLayer_FactoryReturnsNullVM_ViewNotAttached() // Uses SlotType from SetUp
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = (item, slotType) => new List<IConsumed> { itemLayer1 };
        _stubViewModelFactory.CreateViewModelFunc = layer => null;

        var view1 = new StubManagedConsumedView() { ConsumedModelType = typeof(LayeredTestItem) };
        _childViews[typeof(LayeredTestItem)] = view1;

        _viewManager.DisplayItem(new LayeredTestItem("Root",1), new LayeredTestItem("Root",1));
        Assert.IsNull(view1.AttachedViewModel);
    }

    [Test]
    public void ClearView_DetachesViewModels()
    {
        var view1 = new StubManagedConsumedView() { ConsumedModelType = typeof(LayeredTestItem) };
        var view2 = new StubManagedConsumedView() { ConsumedModelType = typeof(CookableLayeredItem) };
        _childViews[typeof(LayeredTestItem)] = view1;
        _childViews[typeof(CookableLayeredItem)] = view2;

        view1.AttachViewModel(new StubConsumedViewModel(new LayeredTestItem("L1",1), null));
        view2.AttachViewModel(new StubConsumedViewModel(new CookableLayeredItem("L2",1), null));

        _viewManager.ClearView();

        Assert.AreEqual(1, view1.DetachCalledCount);
        Assert.IsNull(view1.AttachedViewModel);
        Assert.AreEqual(1, view2.DetachCalledCount);
        Assert.IsNull(view2.AttachedViewModel);
    }
}

// --- End of C# code for ItemSlotViewLogicTests.cs ---
