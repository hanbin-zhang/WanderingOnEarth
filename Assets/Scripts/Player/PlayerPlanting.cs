using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPlanting : MonoBehaviourPunCallbacks
{       
    public GameObject[] objs;
    private int objIndex = 0;
    public TMP_Text selectedObjName;
    //public TMP_Text pressTime;

    public GameObject PlantingCondPanel;
    public TMPro.TMP_Text PlantingCondText;

    private float startTime;
    private GameObject newObj;

    [HideInInspector] public static bool preview;

    public GameObject inventory;
    public List<GameObject> slotList;
   
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        lock (GameObjectTracker.playerObjects)
        {
            GameObjectTracker.playerObjects.Add(this.gameObject);
        }

        slotList = new List<GameObject>();
        foreach (Transform slots in inventory.transform)
        {
            slotList.Add(slots.gameObject);
        }
        foreach (GameObject slot in slotList)
        {
            GameObject activeIndicator = slot.transform.GetChild(0).gameObject;
            activeIndicator.SetActive(true);
            activeIndicator.GetComponent<Image>().color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {              
        Plant();

        SelectObj();
        SetText();
       /* if (Input.GetKeyDown(KeyCode.O))
        {
            Manager.Instance.EventController.Get<OnLandPrepEvent>()?.Notify(this.transform.position);
        }*/
    }

    public static List<Vector3> plantTrees = new List<Vector3>();

    private void turnOffPanel()
    {
        PlantingCondPanel.SetActive(false);
    }

    public void Plant(bool keyPressed=false)
    {        
        Vector3 plantingPosition = transform.position + transform.forward * 2;
        
        if (IsValid(plantingPosition, out Vector3 plantPoint)) 
        {           
            if (keyPressed)
            {
                string plantCond = objs[objIndex].GetComponent<NaturalObject>().CheckPlaceCondtion();
                if (plantCond is not null)
                {
                    PlantingCondPanel.SetActive(true);
                    PlantingCondText.text = plantCond;
                    Invoke(nameof(turnOffPanel), 2);
                }
                else if (Manager.StateController.GetStateProperty(plantPoint).label == StateLabel.POLLUTED)
                {
                    PlantingCondPanel.SetActive(true);
                    PlantingCondText.text = $"unable to plant on {StateLabel.POLLUTED}";
                    Invoke(nameof(turnOffPanel), 2);
                }
                else
                {
                    if (objs[objIndex].name == "TreeMain")
                    {
                        plantTrees.Add(plantPoint);
                    }
                    Manager.EventController.Get<OnPlantEvent>()?.Notify(plantPoint, transform.rotation, objs[objIndex].name);

                    InstantiateNaObj(objs[objIndex].name, plantPoint, transform.rotation);
                }
            }
            
        }
    }

    private void InstantiateNaObj(string objName, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = PhotonNetwork.Instantiate(objName, position, rotation);
        NaturalObject naturalObject = gameObject.GetComponent<NaturalObject>();

        StateProperty stateProperty = Manager.StateController.GetStateProperty(position);

        if (naturalObject.CheckUpdateCondition(stateProperty) is null)
        {
            stateProperty.EvolvingNaObjs.Add(naturalObject);
            NaObjManager.Register(naturalObject);
        }
        else
        {
            stateProperty.PendingNaObjs.Add(naturalObject);
        }

        stateProperty.AddCount(naturalObject.GetDerivedClassName());

        for (int i = stateProperty.PendingNaObjs.Count - 1; i >= 0; i--)
        {
            if (stateProperty.PendingNaObjs[i].CheckUpdateCondition(stateProperty) is null)
            {
                stateProperty.EvolvingNaObjs.Add(stateProperty.PendingNaObjs[i]);
                stateProperty.PendingNaObjs[i].SetNewUpdateTime();
                NaObjManager.Register(stateProperty.PendingNaObjs[i]);
                stateProperty.PendingNaObjs.RemoveAt(i);
            }
        }
    }

    private bool IsValid(Vector3 rayOrigin1, out Vector3 point)
    {
        rayOrigin1.y = 1000;

        bool valid = false;
        point = default;
        Ray ray = new Ray(rayOrigin1, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            point = hit.point;
            // Check for collisions with existing objects
            Collider[] colliders = Physics.OverlapSphere(point, 1f);
            // terrain is a collider
            valid = hit.collider.gameObject.tag == "Ground" && colliders.Length <= 1;
        }
       
        slotList[objIndex].transform.GetChild(0).gameObject.GetComponent<Image>().color = valid ? Color.green : Color.red;

        return valid && !Cursor.visible || startTime != 0;
    }

    public void SetSlotActive(int index)
    {
        objIndex = index;        
        for (int i = 0; i < slotList.Count; i++)
        {
            GameObject activeIndicator = slotList[i].transform.GetChild(0).gameObject;
            activeIndicator.GetComponent<Image>().color = i == index ? Color.green : Color.black;
        }

    }

    private void SelectObj()
    {
        
       /* //if (Input.GetAxis("Mouse ScrollWheel"))
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0)
        {
            objIndex++;
            if(objIndex > objs.Length - 1)
            {
                objIndex = 0;
            }
        } else if(scroll < 0) {
            objIndex--;
            if (objIndex < 0)
            {
                objIndex = objs.Length - 1;
            }
        }
        //Debug.Log(objIndex);
*/
    }

    private void SetText()
    {
        selectedObjName.text = objs[objIndex].name;

        /*if (startTime != 0f)
        {
            pressTime.text = (Time.time - startTime).ToString("f2");
        }
        if (startTime == 0)
        {
            pressTime.text = null;
        }*/
    }

    private void DestroyPollution(Vector3 rayOrigin1)
    {/*
        rayOrigin1.y = 1000;

        bool valid = false;
        point = default;
        Ray ray = new Ray(rayOrigin1, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            point = hit.point;
            // Check for collisions with existing objects
            Collider[] colliders = Physics.OverlapSphere(point, 1f);
            // terrain is a collider
            valid = hit.collider.gameObject.name == "Terrain1" && colliders.Length <= 1;
        }

        crossHair.GetComponent<Image>().color = valid ? Color.green : Color.red;

        return valid && !Cursor.visible || startTime != 0;*/
    }
}

/*if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
                preview = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if ((Time.time - startTime) >= 0.2f)
                {
                    string plantCond = objs[objIndex].GetComponent<NaturalObject>().CheckPlaceCondtion();
                    if (plantCond is not null)
                    {
                        PlantingCondPanel.SetActive(true);
                        PlantingCondText.text = plantCond;
                        Invoke(nameof(turnOffPanel), 2);
                    }
                    else
                    {
                        //newObj = PhotonNetwork.Instantiate(objs[objIndex].name, plantPoint, transform.rotation);
                        PlantObj(objs[objIndex].name, plantPoint, transform.rotation);
                        //Debug.Log(objs[objIndex].name);
                    }

                }
                startTime = 0;
                preview = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Manager.Instance.EventController.Get<OnWaterEvent>()?.Notify();
            }
            */
