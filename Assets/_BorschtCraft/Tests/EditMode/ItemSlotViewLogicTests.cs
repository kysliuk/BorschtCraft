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
// Assuming MockConsumed, MockCookableConsumed, MockCookedConsumed are available from previous tests or defined here.

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
    public object SignalBusStub { get; } // Keep as object for flexibility if SignalBus type is an issue
    public bool IsVisible { get; private set; }
    public int SetVisibilityCount { get; private set; } = 0;

    // Constructor used by StubDiContainer in ViewModelFactoryTests
    public StubConsumedViewModel(IConsumed model, SignalBus signalBus) // Matching factory call pattern
    {
        Model = model;
        SignalBusStub = signalBus; // Store it, even if it's null in some test setups
    }
    public void SetVisibility(bool visible) { IsVisible = visible; SetVisibilityCount++; }
    public void OnSlotClicked() {}
    public void OnActionSpecificToThisVM() {}
}

// Stub for DiContainer - very basic for ViewModelFactory tests
public class StubDiContainerForFactoryTests // Renamed to avoid conflict if other DiContainer stubs exist
{
    public System.Func<Type, object[], object> InstantiateFunc { get; set; }

    // This matches the DiContainer method used by ViewModelFactory
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
        var layers = _processor.GetLayersToDisplay(null);
        Assert.IsEmpty(layers);
    }

    [Test]
    public void GetLayers_SingleItem_ReturnsItemItself()
    {
        var item = new LayeredTestItem("Single", 10);
        var layers = _processor.GetLayersToDisplay(item);
        Assert.AreEqual(1, layers.Count);
        Assert.Contains(item, layers);
    }

    [Test]
    public void GetLayers_WrappedItems_ReturnsAllLayersInOrder()
    {
        var bottom = new LayeredTestItem("Bottom", 5);
        var middle = new LayeredTestItem("Middle", 3, bottom);
        var top = new LayeredTestItem("Top", 2, middle);

        var layers = _processor.GetLayersToDisplay(top);
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

        var layers = _processor.GetLayersToDisplay(topCooked);

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

        var layers = _processor.GetLayersToDisplay(topCookable);
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
        var layers = _processor.GetLayersToDisplay(current);
        Assert.LessOrEqual(layers.Count, 10);
    }
}

[TestFixture]
public class ViewModelFactoryTests
{
    private ViewModelFactory _factory;
    private StubDiContainerForFactoryTests _stubDiContainer;
    // SignalBus will be null for these tests as Zenject.SignalBus is hard to stub without proper test framework
    private List<ConsumedViewModelMapping> _mappings;

    [SetUp]
    public void SetUp()
    {
        _stubDiContainer = new StubDiContainerForFactoryTests();
        _mappings = new List<ConsumedViewModelMapping>();
        // Passing null for Zenject.SignalBus to allow compilation and test other factory logic.
        // ViewModelFactory's constructor expects Zenject.SignalBus, not a custom stub.
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
                // args[0] is itemLayer, args[1] is signalBus (which is null in this test setup)
                return new StubConsumedViewModel(args[0] as IConsumed, args[1] as SignalBus);
            }
            return null;
        };

        var vm = _factory.CreateViewModel(item) as StubConsumedViewModel;
        Assert.IsNotNull(vm);
        Assert.AreEqual(item, vm.Model);
        Assert.IsNull(vm.SignalBusStub, "SignalBus should be null as passed to factory constructor in test");
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
        _stubDiContainer.InstantiateFunc = (type, args) => new object(); // Returns plain object

        var vm = _factory.CreateViewModel(item);
        Assert.IsNull(vm);
    }
}

[TestFixture]
public class ItemSlotViewManagerTests
{
    private ItemSlotViewManager _viewManager;
    private StubItemLayerProcessor _stubItemLayerProcessor;
    private StubViewModelFactory _stubViewModelFactory;
    private Dictionary<Type, IManagedConsumedView> _childViews;
    private Transform _mockSlotTransform;

    public class StubItemLayerProcessor : IItemLayerProcessor
    {
        public Func<IConsumed, List<IConsumed>> GetLayersToDisplayFunc { get; set; } = item => new List<IConsumed>();
        public List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem) => GetLayersToDisplayFunc(overallRootItem);
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

        _viewManager.Initialize(_mockSlotTransform, _childViews, _stubItemLayerProcessor, _stubViewModelFactory);
    }

    [TearDown]
    public void TearDown()
    {
        if (_mockSlotTransform != null && _mockSlotTransform.gameObject != null) GameObject.DestroyImmediate(_mockSlotTransform.gameObject);
    }

    [Test]
    public void Initialize_DetachesViewModelsFromChildViews()
    {
        var view1 = new StubManagedConsumedView();
        _childViews[typeof(LayeredTestItem)] = view1;
        _viewManager.Initialize(_mockSlotTransform, _childViews, _stubItemLayerProcessor, _stubViewModelFactory);
        Assert.AreEqual(1, view1.DetachCalledCount);
    }

    [Test]
    public void DisplayItem_NullItem_ClearsAndReturns()
    {
        var view1 = new StubManagedConsumedView();
        var vm1 = new StubConsumedViewModel(new LayeredTestItem("Old",1), null);
        view1.AttachViewModel(vm1);
        _childViews[typeof(LayeredTestItem)] = view1;

        _viewManager.DisplayItem(null, null);
        Assert.AreEqual(1, view1.DetachCalledCount);
        Assert.IsNull(view1.AttachedViewModel);
    }

    [Test]
    public void DisplayItem_ProcessorReturnsLayers_FactoryCreatesVMs_ViewsAreAttached()
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        var itemLayer2 = new CookableLayeredItem("Layer2", 5);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = item => new List<IConsumed> { itemLayer1, itemLayer2 };

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
    public void DisplayItem_ProcessorReturnsLayer_NoViewForLayer_LogsWarning()
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = item => new List<IConsumed> { itemLayer1 };
        Assert.DoesNotThrow(() => _viewManager.DisplayItem(new LayeredTestItem("Root",1), new LayeredTestItem("Root",1)));
    }

    [Test]
    public void DisplayItem_ProcessorReturnsLayer_FactoryReturnsNullVM_ViewNotAttached()
    {
        var itemLayer1 = new LayeredTestItem("Layer1", 10);
        _stubItemLayerProcessor.GetLayersToDisplayFunc = item => new List<IConsumed> { itemLayer1 };
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
