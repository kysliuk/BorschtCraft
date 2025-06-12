using BorschtCraft.Food;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

public class BreadTest
{
    private BreadStack _breadStack;
    private SaloStack _saloStack;
    private GarlicStack _garlicStack;
    private OnionStack _onionStack;
    private HorseradishStack _horseradishStack;
    private MustardStack _mustardStack;

    private IConsumed _bread;

    [Test]
    public void CreateConsumables()
    {
        _breadStack = ConsumeAbstractFactory.CreateConsumable<BreadStack>(10);
        Assert.IsNotNull(_breadStack);

        _saloStack = ConsumeAbstractFactory.CreateConsumable<SaloStack>(5);
        Assert.IsNotNull(_saloStack);

        _garlicStack = ConsumeAbstractFactory.CreateConsumable<GarlicStack>(3);
        Assert.IsNotNull(_garlicStack);

        _onionStack = ConsumeAbstractFactory.CreateConsumable<OnionStack>(2);
        Assert.IsNotNull(_onionStack);

        _mustardStack = ConsumeAbstractFactory.CreateConsumable<MustardStack>(4);
        Assert.IsNotNull(_mustardStack);

        _horseradishStack = ConsumeAbstractFactory.CreateConsumable<HorseradishStack>(1);
        Assert.IsNotNull(_horseradishStack);
    }

    [Test]
    public void BreadTestSimplePasses()
    {
        CreateConsumables();

        _bread = _breadStack.Consume();
        Assert.IsNotNull(_bread);
        Assert.AreEqual(10, _bread.Price);
        Assert.IsInstanceOf<BreadRaw>(_bread);

        _bread = (IConsumed)(_bread as BreadRaw).Cook();
        Assert.IsInstanceOf<BreadCooked>(_bread);
        Assert.AreEqual(10, _bread.Price);

        _bread = _garlicStack.Consume(_bread);
        Assert.AreEqual(13, _bread.Price);
        Assert.IsInstanceOf<Garlic>(_bread);

        _bread = _saloStack.Consume(_bread);
        Assert.AreEqual(18, _bread.Price);
        Assert.IsInstanceOf<Salo>(_bread);

        LogAssert.Expect(LogType.Error, "[ERROR] OnionStack: cannot be consumed for Salo. Only BreadCooked is allowed.");

        _bread = _onionStack.Consume(_bread);
        Assert.AreEqual(18, _bread.Price);
        Assert.IsInstanceOf<Salo>(_bread);

        _bread = _horseradishStack.Consume(_bread);
        Assert.AreEqual(19, _bread.Price);
        Assert.IsInstanceOf<Horseradish>(_bread);

        _bread = _mustardStack.Consume(_bread);
        Assert.AreEqual(23, _bread.Price);
        Assert.IsInstanceOf<Mustard>(_bread);
    }
}
