using UnityEngine;

public class Tornado : DisasterObject
{
    public override void DisasterLifeCycle()
    {
        StateLabel state = Manager.StateController.GetRegionalState(transform.position).StateLabel;

        Debug.Log($"current tornado life: {Disasterlife}");
        if (state == StateLabel.POLLUTED)
        {
            Disasterlife -= 1;
        }
        else if (state == StateLabel.NORMAL)
        {
            Disasterlife -= 3;
        }
        else if (state == StateLabel.SAFE)
        {
            Disasterlife -= 5;
        }

        if (Disasterlife > 0)
        {
            Invoke(nameof(DisasterLifeCycle), 1f);
        } 
        else
        {
            // for the manager
            //gameObject.transform.parent.gameObject.SetActive(false);

            gameObject.GetComponent<Photon.Pun.PhotonView>()
                .RPC(nameof(SetGObjActive), Photon.Pun.RpcTarget.All, false);
            //Photon.Pun.PhotonNetwork.Destroy(gameObject);
        }
    }
}