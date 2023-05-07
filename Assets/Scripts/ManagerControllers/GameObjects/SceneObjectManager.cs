using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    public GameObject tornado;

    public Vector3 RandomTerrainPosition(float yHeight)
    {
        Vector3 terrainSize = Terrain.activeTerrain.terrainData.size;

        float randomX = Random.Range(0,terrainSize.x);
        float randomZ = Random.Range(0, terrainSize.z);
        float terrainHeight = Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 0f, randomZ));
        Vector3 position = new Vector3(randomX, terrainHeight + yHeight, randomZ);

        // Check if position is within terrain bounds
        if (position.x < 0f || position.x > terrainSize.x || position.z < 0f || position.z > terrainSize.z)
        {
            // Regenerate position if it's outside of terrain bounds
            position = RandomTerrainPosition(yHeight);
        }

        return position;
    }


    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
            return;
        }
        /*Manager.EventController.Get<OnStateChangeEvent>().AddListener((msg) => {
            TornadoControl(StateLabel.POLLUTED, true);                  
        });
        TornadoControl(StateLabel.POLLUTED, true);*/

        Vector3 po = new Vector3(200f, 450f, 200f);

        GameObject sceneObject = PhotonNetwork.Instantiate(tornado.name, po, Quaternion.identity);
        //sceneObject.SetActive(false);
        ActivateDisaster(sceneObject, 0f);

    }

    public void ActivateDisaster(GameObject disasterObj, float timeWait)
    {
        Manager.Invoke(() =>
        {
            Vector3 randomPos = RandomTerrainPosition(3f);

            Debug.Log($"random pos {randomPos}");
            disasterObj.transform.position = randomPos;

            disasterObj.SetActive(true);

            disasterObj.GetComponent<PhotonView>()
            .RPC(nameof(DisasterObject.SetGObjActive), RpcTarget.Others, true);

            disasterObj.GetComponent<DisasterObject>()
                .StartDisasterLifeCycle(this);
        }, timeWait, this);
    }

    private void Appear(GameObject obj, Vector3 pos) {
        PhotonNetwork.Instantiate(obj.name, pos, Quaternion.identity);      
    }

    private void Appear(GameObject obj, Vector3 pos, float time) {
        PhotonView photonView = PhotonView.Get(this);
        GameObject sceneObject = PhotonNetwork.Instantiate(obj.name, pos, Quaternion.identity);
        Manager.Invoke(() => {
            // TODO: everyone need to remove, use RPC ?
            Debug.LogError(ToString());
            RPCRemove(sceneObject);
            //Manager.GameObjectManager.Remove(sceneObject); 
            // photonView.RPC(nameof(RPCRemove), RpcTarget.Others, sceneObject);
            // PhotonNetwork.SendAllOutgoingCommands();
            PhotonNetwork.Destroy(sceneObject);
        }, time, this);
    }

    [PunRPC]
    private void RPCRemove(GameObject sceneObject){
        Manager.GameObjectManager.Remove(sceneObject);
    }

    // private void Appear(GameObject obj, Vector3 pos, float time) {
    //     GameObject sceneObject = Instantiate(obj, pos, Quaternion.identity);
    //     Manager.Invoke(() => {
    //         // TODO: everyone need to remove, use RPC ?
    //         Manager.GameObjectManager.Remove(sceneObject); 
    //         PhotonNetwork.Destroy(sceneObject);
    //     }, time, this);
    // }

    private void TornadoControl(StateLabel lastState, bool fromListener=false){
        // List<Vector3> pos = new List<Vector3>{
        //     new Vector3(10, 10f, 10),
        //     new Vector3(400, 10f, 10),
        //     new Vector3(10, 10f, 400),
        //     new Vector3(400, 10f, 400),
        // };        

        //List<StateProperty> states = Manager.StateController.StatesProperty;
        Vector3 po = new Vector3(200f, 450f, 200f);
        StateLabel state = Manager.StateController.StatesProperty[3].state;
        if (fromListener || state == lastState){          
            if(state == StateLabel.POLLUTED){
                Appear(tornado, po, 1020f);               
                Manager.Invoke(() => {
                    TornadoControl(StateLabel.POLLUTED, false);
                }, 1030, this);
            }
            if(state == StateLabel.NORMAL){
                Appear(tornado, po, 10f);
                Manager.Invoke(() => {
                    TornadoControl(StateLabel.NORMAL, false);
                }, 60, this);
            }
            if(state == StateLabel.SAFE){
                Appear(tornado, po, 5f);
                Manager.Invoke(() => {
                    TornadoControl(StateLabel.SAFE, false);
                }, 120, this);
            }
        }        
    }
}
