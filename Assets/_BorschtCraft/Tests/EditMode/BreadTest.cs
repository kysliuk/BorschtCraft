using BorschtCraft.Food;
using NUnit.Framework;

public class BreadTest
{
    private BreadStack _breadStack;
    private SaloStack _saloStack;
    private GarlicStack _garlicStack;
    private OnionStack _onionStack;
    private HorseradishStack _horseradishStack;
    private MustardStack _mustardStack;

    private Consumed _bread;

    [Test]
    public void CreateConsumables()
    {
        _breadStack = BreadFactory.CreateConsumable<BreadStack>(10);
        Assert.IsNotNull(_breadStack);

        _saloStack = BreadFactory.CreateConsumable<SaloStack>(5);
        Assert.IsNotNull(_saloStack);

        _garlicStack = BreadFactory.CreateConsumable<GarlicStack>(3);
        Assert.IsNotNull(_garlicStack);

        _onionStack = BreadFactory.CreateConsumable<OnionStack>(2);
        Assert.IsNotNull(_onionStack);

        _mustardStack = BreadFactory.CreateConsumable<MustardStack>(4);
        Assert.IsNotNull(_mustardStack);

        _horseradishStack = BreadFactory.CreateConsumable<HorseradishStack>(1);
        Assert.IsNotNull(_horseradishStack);
    }

    [Test]
    public void BreadTestSimplePasses()
    {
        CreateConsumables();

        _bread = BreadFactory.CreateConsumed<BreadRaw>(10, _bread);
        Assert.IsNotNull(_bread);
        Assert.AreEqual(10, _bread.Price);

        _bread = (_bread as BreadRaw).Cook();
        Assert.IsInstanceOf<BreadCooked>(_bread);
        Assert.AreEqual(10, _bread.Price);

        _bread = _saloStack.Consume<Salo>(_bread);
        Assert.AreEqual(15, _bread.Price);
        Assert.IsInstanceOf<Salo>(_bread);

        _bread = _garlicStack.Consume<Garlic>(_bread);
        Assert.AreEqual(18, _bread.Price);
        Assert.IsInstanceOf<Garlic>(_bread);

        _bread = _onionStack.Consume<Onion>(_bread);
        Assert.AreEqual(20, _bread.Price);
        Assert.IsInstanceOf<Onion>(_bread);

        _bread = _horseradishStack.Consume<Horseradish>(_bread);
        Assert.AreEqual(21, _bread.Price);
        Assert.IsInstanceOf<Horseradish>(_bread);

        _bread = _mustardStack.Consume<Mustard>(_bread);
        Assert.AreEqual(25, _bread.Price);
        Assert.IsInstanceOf<Mustard>(_bread);

        _bread = _breadStack.Consume<Mustard>(_bread);
    }
}
