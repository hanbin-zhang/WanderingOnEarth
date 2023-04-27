using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class Manager
{
    private static readonly Lazy<Manager> self = new Lazy<Manager>(() => new Manager());
    private static Manager manager => self.Value;

    public static Manager Instance => manager;

    private MonoBehaviour hook;
    public static MonoBehaviour GameLoop => manager.hook;

    private EventController eventController;
    public static EventController EventController => manager.eventController;

    private StateController stateController;
    public static StateController StateController => manager.stateController;

    private PlantingController plantingController;
    public static PlantingController PlantingController => manager.plantingController;

    private GameObjectManager gameObjectManager;
    public static GameObjectManager GameObjectManager => manager.gameObjectManager;

    private static bool hasInit;

    private Manager()
    {
        // dont change this
    }

    public static void Init(MonoBehaviour hook)
    {
        manager.hook = hook;

        if (hasInit) return;

        manager.eventController = new EventController() {
            new OnLandPrepEvent(),
            new OnPlantEvent(),
            new OnWaterEvent(),
            new OnStateChangeEvent(),
            new OnEvolveEvent(),
            new OnGreenValueReach100Event(),
        };

        manager.stateController = new StateController(manager.eventController) {
            new PollutedState(),
            new NormalState(),
            new SafeState(),
        };

        manager.plantingController = new PlantingController(manager.eventController, new RemotePlantingAdapter());

        manager.gameObjectManager = new GameObjectManager(manager.eventController, manager.stateController);

        hasInit = true;
    }

    public static void Invoke(Action action, float time, MonoBehaviour mono)
    {
        if (time > 0f)
        {
            mono.StartCoroutine(InvokeAction(action, time));
        }
        else
        {
            action.Invoke();
        }
    }

    static IEnumerator InvokeAction(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }


}