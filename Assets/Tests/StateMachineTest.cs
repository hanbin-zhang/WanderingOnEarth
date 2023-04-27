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
        Manager.Init(new MonoBehaviour());

        OnStateChangeEvent.OnStateChangeMessage stateMsg = null;
        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener((msg) =>
        {
            /*Assert.That(msg.stateBefore,  Is.EqualTo(StateLabel.POLLUTED));
            Assert.That(msg.stateAfter, Is.EqualTo(StateLabel.SAFE));*/
            stateMsg = msg;
        });

        Vector3 pos = new Vector3(1, 1, 1);
        Manager.EventController.Get<OnLandPrepEvent>()?.Notify(pos);

        // test normal change
        Assert.That(Manager.StateController.GetRegionalStateProperty(pos).state, Is.EqualTo(StateLabel.NORMAL));
        Assert.That(stateMsg, Is.Not.Null);
        Assert.That(stateMsg.stateBefore, Is.EqualTo(StateLabel.POLLUTED));

        GameObject treePrefab = Resources.Load<GameObject>("Tree");
        for (int i = 0; i < 15; i++)
        {
            GameObject tree = GameObject.Instantiate(treePrefab, pos, Quaternion.identity);
            Manager.GameObjectManager.Add(tree);
        }
        List<string> keysList = new List<string>(Manager.GameObjectManager.GetAll().Keys);
        Debug.Log(string.Join(", ", keysList));
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
