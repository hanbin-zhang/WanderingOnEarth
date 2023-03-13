using System;
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
        };

        manager.stateController = new StateController(manager.eventController)
        {          
            new PollutedState(),
            new NormalState(),
            new SafeState(),
        };

    }

}