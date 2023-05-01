using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct Slot
{
    public GameObject activeIndicator;
    public GameObject image;
}

public class PlayerPlanting : MonoBehaviourPunCallbacks
{
    public List<LiveObject> liveObjects;
    private LiveObject currentLiveObject;

    [HideInInspector]
    private int currentSlotIndex = 0;
    public TMP_Text currentLiveObjectName;
    public GameObject inventory;

    [HideInInspector]
    public List<Slot> inventorySlots = new List<Slot>();
    [HideInInspector]
    private Slot currentSlot;

    [HideInInspector]
    public static List<Vector3> plantTrees = new List<Vector3>();

    public GameObject collectMessagePanel;
    public TMP_Text collectMessageText;
    public GameObject notificationPanel;
    public Button LeaveButton;
    public TMP_Text notificationText;
    private Queue<string> notificationQueue;   
    private int collectIndex;

    public void SwitchSlot(int index)
    {
        if(index>collectIndex) return;
        currentSlotIndex = index;
        currentLiveObject = liveObjects[index];
        currentLiveObjectName.text = currentLiveObject.name;
        currentSlot = inventorySlots[index];
        inventorySlots.ForEach((slot) => slot.activeIndicator.GetComponent<Image>().color = Color.black);
    }

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        notificationQueue = Manager.EventController.NotificationQueue;

        foreach (Transform slot in inventory.transform)
        {
            GameObject obj = slot.gameObject;
            inventorySlots.Add(new Slot()
            {
                activeIndicator = obj.transform.GetChild(0).gameObject,
                image = obj.transform.GetChild(1).gameObject,
            });
        }

        inventorySlots.ForEach((slot) => {
            slot.activeIndicator.SetActive(true);
            slot.activeIndicator.GetComponent<Image>().color = Color.black;           
            slot.image.SetActive(false);
        });
        collectIndex = 3;
        for (int i = 0; i < inventorySlots.Count; i++){
            if (i<=collectIndex){
                inventorySlots[i].image.SetActive(true);
            }
        }
        SwitchSlot(0);
    }
    
    void Start()
    {
        LeaveButton.onClick.AddListener(ButtonClicked);
    }

    void Update()
    {
        HandlePlanting();
        HandleCollectable();
    }

    public void HandlePlanting(bool keyPressed = false)
    {

        Vector3 forward = transform.position + transform.forward * 2;
        bool hasRoom = IsValid(forward, out var plantingPosition);
        bool isPlantable = currentLiveObject.IsPlantable(transform.position, out var reason);
        if (hasRoom && keyPressed) {
            if (isPlantable) {
                Manager.PlantingController.Plant(currentLiveObject, plantingPosition, transform.rotation);
            } else {
                ShowNotification(reason, 2f);
            }
        }
        //Debug.LogError($"hasRoom: {hasRoom}, isPlantable: {isPlantable}");
        currentSlot.activeIndicator.GetComponent<Image>().color = hasRoom && isPlantable ? Color.green : Color.red;
    }

    private void ShowNotification(string text, float time)
    {
        notificationQueue.Enqueue(text); 
        if (notificationQueue.Count == 1) { 
            ProcessNotificationQueue(time); 
        }
    }
    private void ProcessNotificationQueue(float time) 
    { 
        if (notificationQueue.Count > 0) { 
            notificationText.text = notificationQueue.Peek(); 
            notificationPanel.SetActive(true); 
            Manager.Invoke(() => { 
                notificationQueue.Dequeue(); 
                ProcessNotificationQueue(time); 
            }, time, this); } 
        else { 
            notificationPanel.SetActive(false); 
            notificationText.text = ""; 
        } 
    }

    private bool IsValid(Vector3 rayOrigin1, out Vector3 point)
    {
        rayOrigin1.y = 1000;
        bool valid = false;
        point = default;
        Ray ray = new Ray(rayOrigin1, Vector3.down);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            point = hit.point;
            Collider[] colliders = Physics.OverlapSphere(point, 1f);
            valid = (hit.collider.gameObject.tag == "Ground"  && colliders.Length <= 1) || (hit.collider.gameObject.tag == "MiniMap"  && colliders.Length <= 2);
            // Debug.LogError($"colliders.Length: {colliders.Length}, hit tag: {hit.collider.gameObject.tag}");
            // Debug.LogError($"valid: {valid}, !Cursor.visible: {!Cursor.visible}");
        }
        return valid && !Cursor.visible;
    }

    public void HandleCollectable(bool keyPressed = false)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        bool hasChest = false;
        string collectText = "";
        foreach (Collider collider in colliders)
        {
            if (BaseObject.Is<Chest>(collider.gameObject, out var _))
            {
                hasChest = true;
                collectText = "Press C To Collect";
                if (keyPressed)
                {
                    Manager.GameObjectManager.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                    collectIndex++;
                    inventorySlots[collectIndex].image.SetActive(true);
                    string text = $"{liveObjects[collectIndex].name} collected!";
                    ShowNotification(text, 2f);
                    
                }
            }
            
            if (BaseObject.Is<SpaceShip>(collider.gameObject, out var _))
            {
                hasChest = true;
                collectText = "Press C To Leave";
                if (keyPressed)
                {
                    Manager.GameObjectManager.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                    string text = "SpaceShip: quit the game";
                    ShowNotification(text, 2f);
                    LeaveButton.onClick.Invoke();
                    break;
                }
            }
        }

        collectMessageText.text = collectText;
        collectMessagePanel.SetActive(hasChest);
    }
    
    void ButtonClicked()
    {
        Debug.LogFormat("leave the game"); 
    }
}

