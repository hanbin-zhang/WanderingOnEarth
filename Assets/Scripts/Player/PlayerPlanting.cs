using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Transactions;
using static UnityEditor.PlayerSettings;

public class PlayerPlanting : MonoBehaviourPunCallbacks
{   
    public GameObject crossHair;
    // public GameObject tree;
    // public GameObject deer;


    public GameObject[] objs;
    private int objIndex = 0;
    public TMP_Text selectedObjName;
    //public TMP_Text pressTime;

    public GameObject PlantingCondPanel;
    public TMPro.TMP_Text PlantingCondText;

    private float startTime;
    private GameObject newObj;

    [HideInInspector] public static bool preview;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Plant();

        SelectObj();
        SetText();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Manager.Instance.EventController.Get<OnLandPrepEvent>()?.Notify(this.transform.position);
        }
    }

    public static List<Vector3> plantTrees = new List<Vector3>();

    public static void PlantObj(string name, Vector3 pos, Quaternion rotation)
    {
        if (name == "TreeMain")
        {
            plantTrees.Add(pos);
        }
        GameObject gameObject = PhotonNetwork.Instantiate(name, pos, rotation);
        Manager.Instance.EventController.Get<OnPlantEvent>()?.Notify(pos, gameObject.GetComponent<NaturalObject>());
    }

    private void turnOffPanel()
    {
        PlantingCondPanel.SetActive(false);
    }
    private void Plant()
    {
        
        Vector3 plantingPosition = transform.position + transform.forward * 2;

        if (IsValid(plantingPosition, out Vector3 plantPoint)) 
        {

            if (Input.GetMouseButtonDown(0))
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
            valid = hit.collider.gameObject.name == "Terrain1" && colliders.Length <= 1;
        }
       
        crossHair.GetComponent<Image>().color = valid ? Color.green : Color.red;
        
        return valid && !Cursor.visible || startTime != 0;
    }

    private void SelectObj()
    {
        
        //if (Input.GetAxis("Mouse ScrollWheel"))
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
