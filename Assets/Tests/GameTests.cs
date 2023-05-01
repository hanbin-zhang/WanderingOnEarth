using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;



public class GameTest
{
    [Test] 
    public void BoundaryManagerTest()
    {   
        // how to implement
        Assert.AreEqual(true, true);

    }
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
    [Test]
    public void AchievementTest()
    {
        TextMeshPro text = new GameObject().AddComponent<TextMeshPro>();
        var testObject = new GameObject().AddComponent<AchievementManager>();

        var testPanel = new GameObject();
        testPanel.SetActive(false);

        testObject.notificationText = text;
        testObject.notificationPanel = testPanel;

        Manager.Destroy();
        Manager.Init(new MonoBehaviour());

        Manager.EventController.Get<OnGreenValueReach100Event>()?.Notify();
        Assert.That(testPanel.activeSelf, Is.EqualTo(false));
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PlantCondTest()
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
    public void EvolveCondTest()
    {
        Vector3 pos = new Vector3(1, 1, 1);
        GameObject treePrefab = Resources.Load<GameObject>("Tree");
        GameObject tree = GameObject.Instantiate(treePrefab, pos, Quaternion.identity);

        LiveObject treeObject = tree.GetComponent<LiveObject>();
        string evolveCond;
        Assert.AreEqual(treeObject.IsEvolveConditionSatisfied(out evolveCond), true);
        Assert.AreEqual(evolveCond, "");

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EvolveCycleTest()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        Vector3 pos = new Vector3(1, 1, 1);
        GameObject treePrefab = Resources.Load<GameObject>("Tree");
        GameObject tree = GameObject.Instantiate(treePrefab, pos, Quaternion.identity);

        LiveObject treeObject = tree.GetComponent<LiveObject>();

        Assert.AreEqual(treeObject.age, 0);
        yield return null;
        /*treeObject.Invoke(nameof(LiveObject.InvokeEvolve), 0f);
        yield return new WaitForSeconds(treeObject.growingTime[treeObject.age]);
        Assert.AreEqual(treeObject.age, 1);*/
    }

    [UnityTest]
    public IEnumerator CollectionTest(){
        // GameManager game = new GameManager();
        // int collectItem = game.player.GetComponent<PlayerPlanting>().collectIndex;
        Vector3 pos = new Vector3(1, 1, 1);
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        GameObject player = GameObject.Instantiate(playerPrefab, pos, Quaternion.identity);      
        int collectItem = player.GetComponent<PlayerPlanting>().collectIndex;
        Assert.AreEqual(collectItem, 0);
        yield return null;
    }

    [Test]
    public void TaskChangeTest(){
       Assert.AreEqual(true, true);
    }
    [Test]
    public void GreenValueTest(){
       Assert.AreEqual(true, true);
    }
}
