using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public GameObject inventory;

    private int objIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetHotBar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) objIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) objIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) objIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) objIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) objIndex = 4;
        
    }
}
