using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementNote : MonoBehaviour
{
    public GameObject panel;
    public Text notificationText;
    public float displayTime = 5f;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        panel.SetActive(true);
        notificationText.text = message;
        Invoke("CloseNotification", displayTime);
    }

    private void CloseNotification()
    {
        panel.SetActive(false);
    }
}
