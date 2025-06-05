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

    [Test]
    public void BreadTestSimplePasses()
    {
        
    }

    private void CreateConsumables()
    {
        _breadStack = BreadFactory.CreateConsumable<BreadStack>(10);
        _saloStack = BreadFactory.CreateConsumable<SaloStack>(10);
        _garlicStack = BreadFactory.CreateConsumable<GarlicStack>(10);
        _onionStack = BreadFactory.CreateConsumable<OnionStack>(10);
        _mustardStack = BreadFactory.CreateConsumable<MustardStack>(10);
        _horseradishStack = BreadFactory.CreateConsumable<HorseradishStack>(10);
    }
}
