using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class Manager
{
    private static readonly Lazy<Manager> self = new Lazy<Manager>(() => new Manager());
    private static Manager manager => self.Value;

    private GameLoop gameLoop;
    public static GameLoop GameLoop => manager.gameLoop;

    private EventController eventController;
    public static EventController EventController => manager.eventController;

    private StateController stateController;
    public static StateController StateController => manager.stateController;

    private Manager()
    {
         // dont change this
        
    }

    public static void Init(GameLoop gameLoop)
    {
        manager.gameLoop = gameLoop;

        manager.eventController = new EventController() {
            new OnLandPrepEvent(),
            new OnPlantEvent(),
            new OnWaterEvent(),
            new OnLeftMouseDownEvent(),
            new OnStateChangeEvent(),
        };

        manager.stateController = new StateController(manager.eventController)
        {          
            new PollutedState(),
            new NormalState(),
            new SafeState(),
        };

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