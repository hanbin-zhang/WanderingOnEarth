using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class AchivementsManager : MonoBehaviour
{  

    public GameObject AchivePanel;
    public bool isActive = false;
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text desc;
    public int showTime = 5;
    private Mutex mutex = new();

    public float sumGreenValue = 0.0f;


    // achivement 01
    public float achive01threshold = 10.0f;
    public int achive01code = 0;


    // achivement 02
    public int achive02threshold = 3;
    public int achive02code = 0;

    // achivement 03
    // collection

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
            StartCoroutine(LoadAchive02());
        } else if (GameObjectTracker.collected.Count > 0) {
            int ID = GameObjectTracker.collected.FirstOrDefault().Key;
            string desc = GameObjectTracker.collected.FirstOrDefault().Value;
            GameObjectTracker.collected.Remove(ID);

            GameObjectTracker.updatedcollected.Add(ID);
            
            StartCoroutine(LoadAchive03(ID, desc));

            
        }
    }

    IEnumerator LoadAchive01()
    {
        achive01code = 101;
        PlayerPrefs.SetInt("Achive01", achive01code);
        lock (AchivePanel)
        {   
            mutex.WaitOne();
            isActive = true;

           
            AchivePanel.SetActive(true);

            title.text = "Green Value increased!";
            desc.text = "Green value reaches " + achive01threshold;


            Thread.Sleep(5000);
            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";
            isActive = false;
            mutex.ReleaseMutex();
            yield return null;
        }
        
    }

    IEnumerator LoadAchive02()
    {

        achive02code = 102;
        PlayerPrefs.SetInt("Achive02", achive02code);
        lock (AchivePanel)
        {
            mutex.WaitOne();
            Debug.Log(mutex.ToString());
            //if (isActive == true) yield break;
            isActive = true;

            

            AchivePanel.SetActive(true);

            title.text = "Always trees!";
            desc.text = achive02threshold + " trees planted";

            yield return new WaitForSeconds(showTime);
            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";
            isActive = false;
            mutex.ReleaseMutex();

        }

    }

    IEnumerator LoadAchive03(int collectableID, string collectableDesc)
    {
       
        lock (AchivePanel)
        {
            mutex.WaitOne();
            //if (isActive == true) yield break;
            isActive = true;
            Debug.Log(AchivePanel.activeSelf);
            AchivePanel.SetActive(true);
            Debug.Log(collectableID);
            title.text = "Collection " + collectableID + " Collected!";
            desc.text = collectableDesc;

            yield return new WaitForSeconds(showTime);
            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";
            isActive = false;
            mutex.ReleaseMutex();
        }

    }
}