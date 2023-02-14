using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsManager : MonoBehaviour
{  

    public GameObject AchivePanel;
    public bool isActive = false;
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text desc;
    public int showTime = 5;

    public float sumGreenValue = 0.0f;
    private readonly object lockObject = new object();


    // achivement 01
    public float achive01threshold = 10.0f;
    public int achive01code = 0;


    // achivement 02
    public int achive02threshold = 3;
    public int achive02code = 0;

    private void Start()
    {
        PlayerPrefs.SetInt("Achive01", 0);
        PlayerPrefs.SetInt("Achive02", 0);
        AchivePanel.SetActive(false);
    }

    private void FixedUpdate()
    {   
        sumGreenValue = 0.0f;
        foreach (NaturalObject obj in GameObjectTracker.gameObjects)
        {
            sumGreenValue += obj.GetCurrentGreenValue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetInt("Achive01", 0);
        achive01code = PlayerPrefs.GetInt("Achive01");
        achive02code = PlayerPrefs.GetInt("Achive02");

        if (sumGreenValue >= achive01threshold && achive01code != 101)
        {
            StartCoroutine(LoadAchive01());
        }

        else if (GameObjectTracker.TreeCount >= achive02threshold && achive02code != 102)
        {
            Debug.Log(AchivePanel.activeSelf);
            StartCoroutine(LoadAchive02());
        }
    }

    IEnumerator LoadAchive01()
    {   lock (AchivePanel)
        {
            if (isActive == true) yield break;
            isActive = true;

            achive01code = 101;
            PlayerPrefs.SetInt("Achive01", achive01code);
            AchivePanel.SetActive(true);

            title.text = "Green Value increased!";
            desc.text = "Green value reaches " + achive01threshold;


            yield return new WaitForSeconds(showTime);
            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";
            isActive = false;
        }
        
    }

    IEnumerator LoadAchive02()
    {
        lock (AchivePanel)
        {
            if (isActive == true) yield break;
            isActive = true;

            achive02code = 102;
            PlayerPrefs.SetInt("Achive02", achive02code);

            AchivePanel.SetActive(true);

            title.text = "Always trees!";
            desc.text = achive02threshold + " trees planted";

            yield return new WaitForSeconds(showTime);
            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";
            isActive = false;
        }
        
    }
}
