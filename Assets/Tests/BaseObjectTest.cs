using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseObjectTest
{
   
    // A Test behaves as an ordinary method
    [Test]
    public void BaseObjectTestSimplePasses()
    {
        Manager.Init(new MonoBehaviour());
        // Use the Assert class to test conditions
        Assert.That(1, Is.EqualTo(1));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BaseObjectTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
