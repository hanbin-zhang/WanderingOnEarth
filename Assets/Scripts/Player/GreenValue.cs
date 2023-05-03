using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenValue : MonoBehaviour
{
    public TMPro.TMP_Text greenValueDisplay;

    [HideInInspector]
    private int currentRegion;

    private void Start() {
        UpdateGreenValueDisplay();
        Manager.EventController.Get<OnEvolveEvent>()?.AddListener((_) => {
            UpdateGreenValueDisplay();
            //UpdateRegionalGreenValueDisplay();
        });  

        //currentRegion = Manager.StateController.GetRegionalIndex(transform.position);
        //CheckRegionIndex();
    }

    private void UpdateGreenValueDisplay() {
        int globalGreenValue = Manager.GameObjectManager.GetGlobalGreenValue();
        greenValueDisplay.text = $"{globalGreenValue}";
        if (globalGreenValue == 100) {
            Manager.EventController.Get<OnGreenValueReach100Event>()?.Notify();
        }
    }

    private void UpdateRegionalGreenValueDisplay()
    {
        int regionalGreenValue = Manager.GameObjectManager.GetRegionalGreenValue(transform.position);
        greenValueDisplay.text = $"{regionalGreenValue}";
    }

    private void CheckRegionIndex()
    {
        int index = Manager.StateController.GetRegionalIndex(transform.position);
        if (index != currentRegion)
        {
            UpdateRegionalGreenValueDisplay();
            currentRegion = index;
        }
        Invoke(nameof(CheckRegionIndex), 1f);
    }

    
}
