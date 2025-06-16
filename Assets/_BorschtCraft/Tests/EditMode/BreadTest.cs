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

        _bread = _breadStack.Consume(null);
        Assert.IsNotNull(_bread);
        Assert.AreEqual(10, _bread.Price);
        Assert.IsInstanceOf<BreadRaw>(_bread);
        Assert.AreEqual(0, _bread.Ingredients.Count);

        _bread = (_bread as BreadRaw).Cook();
        Assert.IsInstanceOf<BreadCooked>(_bread);
        Assert.AreEqual(10, _bread.Price);
        Assert.AreEqual(1, _bread.Ingredients.Count);

        _bread = _garlicStack.TryConsume(_bread, out var succeed);
        Assert.IsTrue(succeed);
        Assert.AreEqual(13, _bread.Price);
        Assert.IsInstanceOf<Garlic>(_bread);
        Assert.AreEqual(2, _bread.Ingredients.Count);

        _bread = _saloStack.TryConsume(_bread, out succeed);
        Assert.IsTrue(succeed);
        Assert.AreEqual(18, _bread.Price);
        Assert.IsInstanceOf<Salo>(_bread);
        Assert.AreEqual(3, _bread.Ingredients.Count);

        LogAssert.Expect(LogType.Warning, "[WARNING] OnionStack: Cannot decorate Salo by OnionStack");

        _bread = _onionStack.TryConsume(_bread, out succeed);
        Assert.IsFalse(succeed);
        Assert.AreEqual(18, _bread.Price);
        Assert.IsInstanceOf<Salo>(_bread);
        Assert.AreEqual(3, _bread.Ingredients.Count);

        _bread = _horseradishStack.TryConsume(_bread, out succeed);
        Assert.IsTrue(succeed);
        Assert.AreEqual(19, _bread.Price);
        Assert.IsInstanceOf<Horseradish>(_bread);
        Assert.AreEqual(4, _bread.Ingredients.Count);

        _bread = _mustardStack.TryConsume(_bread, out succeed);
        Assert.IsTrue(succeed);
        Assert.AreEqual(23, _bread.Price);
        Assert.IsInstanceOf<Mustard>(_bread);
        Assert.AreEqual(5, _bread.Ingredients.Count);
    }
}
