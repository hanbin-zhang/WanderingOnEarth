using UnityEngine;

public class Tornado : DisasterObject
{
    public int reActivateTime;
    public override void DisasterLifeCycle()
    {
        StateLabel state = Manager.StateController.GetRegionalState(transform.position).StateLabel;

        Debug.Log($"current tornado life: {lifeCounter}");
        if (state == StateLabel.POLLUTED)
        {
            lifeCounter += 1;
        }
        else if (state == StateLabel.NORMAL)
        {
            lifeCounter += 3;
        }
        else if (state == StateLabel.SAFE)
        {
            lifeCounter += 5;
        }

        if (Disasterlife >= lifeCounter)
        {
            Invoke(nameof(DisasterLifeCycle), 1f);
        }
        else
        {
            // for the manager
            //gameObject.transform.parent.gameObject.SetActive(false);

            lifeCounter = 0;

            gameObject.GetComponent<Photon.Pun.PhotonView>()
                .RPC(nameof(SetGObjActive), Photon.Pun.RpcTarget.All, false);

            sceneManager.ActivateDisaster(gameObject, reActivateTime);
        }
    }
}