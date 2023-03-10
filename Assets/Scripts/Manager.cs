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


    private PlantingController plantingController;
    public PlantingController PlantingController => plantingController;

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

        //plantingController = new PlantingController(new LocalPlantingAdatper(eventController));

    }

    public void Init(GameLoop gameLoop) {
        this.gameLoop = gameLoop;
    }

}

/*public class LocalPlantingAdatper : PlantingInterface {
    private EventController eventController;
    public LocalPlantingAdatper(EventController eventController) {
        this.eventController = eventController;
    }

    public override GameObject Instantiate(String name, Vector3 pos) {
        eventController.Get<OnPlantEvent>().Notify(pos);
        return GameObject.Instantiate(name, pos, Qu.idenity);
    }
    
    public override void Destroy(String name, Vector3 pos) {

    }
}

public class RemotePlantingAdatper : PlantingInterface {
    public override GameObject Instantiate(String name, Vector3 pos) {
        PhotonNetwork.Instantiate(name, pos, Qu.idenity);
        return null;
    }
    public override void Destroy(String name, Vector3 pos) {

    }
}
*/

