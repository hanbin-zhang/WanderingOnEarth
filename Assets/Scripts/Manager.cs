using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class Manager{
    private static readonly Lazy<Manager> self = new Lazy<Manager>(() => new Manager());
    public static Manager Instance => self.Value;

    private GameLoop gameLoop;
    public GameLoop GameLoop => gameLoop;

    private EventController eventController;
    public EventController EventController => eventController;

    private StateController stateController;
    public StateController StateController => stateController;

    

    private Manager() {



        eventController = new EventController() {
            new OnPlantEvent(),
            new OnWaterEvent(),
        };

        stateController = new StateController(eventController)
        {
            new PollutedState(),
            new NormalState()
        };

    }

    public void Init(GameLoop gameLoop) {
        this.gameLoop = gameLoop;
    }

}


