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

    public float sumGreenValue = 0.0f;

    // achivement 01
    public float achive01threshold = 10.0f;
    public int achive01code = 0;
    private void Start()
    {
        PlayerPrefs.SetInt("Achive01", 0);
        AchivePanel.gameObject.SetActive(false);
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
       
        if (sumGreenValue >= achive01threshold && achive01code != 101)
        {
            StartCoroutine(LoadAchive01());
        }
    }

    IEnumerator LoadAchive01()
    {
        isActive = true;
        achive01code = 101;
        PlayerPrefs.SetInt("Achive01", achive01code);
        AchivePanel.SetActive(true);
        title.text = "Green Value increased!";
        Debug.Log("title");
        desc.text = "Green value reaches " + achive01threshold;
        Debug.Log("dsc");

        
        Debug.Log("");

        yield return new WaitForSeconds(7);
        AchivePanel.SetActive(false);
        title.text = "";
        desc.text = "";
        isActive = false;
    }
}
