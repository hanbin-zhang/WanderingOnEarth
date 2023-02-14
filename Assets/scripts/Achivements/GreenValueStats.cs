using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenValueStats : MonoBehaviour
{

    public TMPro.TMP_Text notificationText;
    public float displayTime = 5f;

    private void Start()
    {
        
        notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = message;
        Invoke(nameof(CloseNotification), displayTime);
    }

    private void CloseNotification()
    {
        notificationText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        float sumGreenValue = 0;
        NaturalObject[] naObjs = FindObjectsOfType<NaturalObject>();
        if (naObjs.Length > 0 || naObjs != null)
        {
            foreach (NaturalObject obj in naObjs)
            {
                sumGreenValue += obj.GetCurrentGreenValue();
            }

        }
        //Debug.Log(sumGreenValue);
        if (sumGreenValue > 10.0f)
        {
            ShowNotification("Reach Green Value ten!");
        }

    }
}
