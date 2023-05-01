using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class StateMachineTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void StateChangeTest()
    {
        Manager.Destroy();
        Manager.Init(new MonoBehaviour());

        OnStateChangeEvent.OnStateChangeMessage stateMsg = null;

        void OnStateChange(OnStateChangeEvent.OnStateChangeMessage msg)
        {
            stateMsg = msg;
        }

        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener(OnStateChange);

        Vector3 pos = new Vector3(1, 1, 1);
        Manager.EventController.Get<OnLandPrepEvent>()?.Notify(pos);

        // test normal change
        Assert.That(Manager.StateController.GetRegionalStateProperty(pos).state, Is.EqualTo(StateLabel.NORMAL));
        Assert.That(stateMsg, Is.Not.Null);
        Assert.That(stateMsg.stateBefore, Is.EqualTo(StateLabel.POLLUTED));
        Assert.That(stateMsg.stateAfter, Is.EqualTo(StateLabel.NORMAL));

        GameObject treePrefab = Resources.Load<GameObject>("Tree");
        for (int i = 0; i < 15; i++)
        {
            GameObject tree = GameObject.Instantiate(treePrefab, pos, Quaternion.identity);
            tree.name = tree.GetComponent<BaseObject>().GetType().Name;
            Manager.GameObjectManager.Add(tree);
            Manager.EventController.Get<OnPlantEvent>()?.Notify(
                tree.transform.position, tree.transform.rotation, tree.name);
        }
       
        // test safe change
        Assert.That(Manager.StateController.GetRegionalStateProperty(pos).state, Is.EqualTo(StateLabel.SAFE));
        Assert.That(stateMsg.stateBefore, Is.EqualTo(StateLabel.NORMAL));
        Assert.That(stateMsg.stateAfter, Is.EqualTo(StateLabel.SAFE));
        Manager.EventController.Get<OnStateChangeEvent>()?.RemoveListener(OnStateChange);

    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
