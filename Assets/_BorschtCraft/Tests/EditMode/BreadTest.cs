using BorschtCraft.Food;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

public class BreadTest
{
    private BreadStack breadStack;
    private SaloStack _saloStack;
    private GarlicStack _garlicStack;
    private OnionStack _onionStack;
    private HorseradishStack _horseradishStack;
    private MustardStack _mustardStack;

    //private IConsumed bread;

    [Test]
    public void CreateConsumables()
    {
        breadStack = ConsumeAbstractFactory.CreateConsumable<BreadStack>(10);
        Assert.IsNotNull(breadStack);

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

        var succeed = breadStack.TryConsume(null, out var bread);
        Assert.IsNotNull(bread);
        Assert.AreEqual(10, bread.Price);
        Assert.IsInstanceOf<BreadRaw>(bread);
        Assert.AreEqual(0, bread.Ingredients.Count);

        bread = (bread as BreadRaw).Cook();
        Assert.IsInstanceOf<BreadCooked>(bread);
        Assert.AreEqual(10, bread.Price);
        Assert.AreEqual(1, bread.Ingredients.Count);

        succeed = _garlicStack.TryConsume(bread, out bread);
        Assert.IsTrue(succeed);
        Assert.AreEqual(13, bread.Price);
        Assert.IsInstanceOf<Garlic>(bread);
        Assert.AreEqual(2, bread.Ingredients.Count);

        succeed = _saloStack.TryConsume(bread, out bread);
        Assert.IsTrue(succeed);
        Assert.AreEqual(18, bread.Price);
        Assert.IsInstanceOf<Salo>(bread);
        Assert.AreEqual(3, bread.Ingredients.Count);

        LogAssert.Expect(LogType.Warning, "[WARNING] OnionStack: Cannot decorate Salo by OnionStack");

        succeed = _onionStack.TryConsume(bread, out bread);
        Assert.IsFalse(succeed);
        Assert.IsInstanceOf<Salo>(bread);
        Assert.AreEqual(18, bread.Price);
        Assert.AreEqual(3, bread.Ingredients.Count);

        succeed = _horseradishStack.TryConsume(bread, out bread);
        Assert.IsTrue(succeed);
        Assert.AreEqual(19, bread.Price);
        Assert.IsInstanceOf<Horseradish>(bread);
        Assert.AreEqual(4, bread.Ingredients.Count);

        succeed = _mustardStack.TryConsume(bread, out bread);
        Assert.IsTrue(succeed);
        Assert.AreEqual(23, bread.Price);
        Assert.IsInstanceOf<Mustard>(bread);
        Assert.AreEqual(5, bread.Ingredients.Count);
    }
}
