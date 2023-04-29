using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseObjectTest
{
   
    // A Test behaves as an ordinary method
    [Test]
    public void DeerTest()
    {
        Manager.Destroy();
        Manager.Init(new MonoBehaviour());
        Vector3 pos = new Vector3(1, 1, 1);
        Deer deer = new Deer();
        string plantReason;
        bool errorCond = deer.IsPlantable(pos, out plantReason);

        // test not plantable
        Assert.That(plantReason, Is.Not.EqualTo(""));
        Assert.That(errorCond, Is.EqualTo(false));
        StateProperty stateProperty = Manager.StateController.GetRegionalStateProperty(pos);
        stateProperty.SetState(StateLabel.NORMAL);
        //Manager.StateController.GetRegionalStateProperty(pos).SetState(StateLabel.NORMAL);
        //Manager.EventController.Get<OnLandPrepEvent>()?.Notify(pos);
        GameObject treePrefab = Resources.Load<GameObject>("Tree");
        for (int i = 0; i < 3; i++)
        {
            GameObject tree = GameObject.Instantiate(treePrefab, pos, Quaternion.identity);
            tree.name = tree.GetComponent<BaseObject>().GetType().Name;
            Manager.GameObjectManager.Add(tree);
        }
        bool correctCond = deer.IsPlantable(pos, out plantReason);

        // test not plantable
        Debug.Log(plantReason);
        Assert.AreEqual(plantReason, "");
        Assert.That(correctCond, Is.EqualTo(true));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator BaseObjectTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
