using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class AchivementsManager : MonoBehaviour
{  

    public GameObject AchivePanel;
    [HideInInspector] public bool isActive = false;
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text desc;
    public int showTime = 5;

    [HideInInspector] public float sumGreenValue = 0.0f;


    // achivement 01
    [HideInInspector] public float achive01threshold = 10.0f;
    [HideInInspector] public int achive01code = 0;


    // achivement 02
    [HideInInspector] public int achive02threshold = 3;
    [HideInInspector] public int achive02code = 0;

    // achivement 03
    [HideInInspector] public int achive03threshold = 3;
    [HideInInspector] public int achive03code = 0;

    private void Start()
    {
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
        // load achived achivements
        /*achive01code = PlayerPrefs.GetInt("Achive01");
        achive02code = PlayerPrefs.GetInt("Achive02");
        achive03code = PlayerPrefs.GetInt("Achive03");*/

        if (!isActive & sumGreenValue >= achive01threshold & achive01code != 101)
        {
            isActive = true;
            StartCoroutine(LoadAchive01());
        }

        if (GameObjectTracker.objectCount.ContainsKey(typeof(TreeObject).Name))
        {
            if (!isActive & GameObjectTracker.objectCount[typeof(TreeObject).Name] >= achive02threshold && achive02code != 102)
            {
                isActive = true;
                StartCoroutine(LoadAchive02());
            }
        }


        if (GameObjectTracker.objectCount.ContainsKey(typeof(DeerObject).Name))
        {
            if (!isActive & GameObjectTracker.objectCount[typeof(DeerObject).Name] >= achive03threshold && achive03code != 103)
            {
                isActive = true;
                StartCoroutine(LoadAchive03());
            }
        }

        if (GameObjectTracker.collected.Count > 0 && !isActive)
        {
            isActive = true;
            int ID = GameObjectTracker.collected.FirstOrDefault().Key;
            string desc = GameObjectTracker.collected.FirstOrDefault().Value;
            GameObjectTracker.collected.Remove(ID);

            GameObjectTracker.updatedcollected.Add(ID);

            StartCoroutine(LoadAchiveCollect(ID, desc));
        }
    }
        

    IEnumerator LoadAchive01()
    {
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

    IEnumerator LoadAchive02()
    {
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

    IEnumerator LoadAchive03()
    {
        achive03code = 103;
        PlayerPrefs.SetInt("Achive03", achive03code);

        AchivePanel.SetActive(true);

        title.text = "deers";
        desc.text = achive03threshold + " deers breeded";

        yield return new WaitForSeconds(showTime);
        AchivePanel.SetActive(false);
        title.text = "";
        desc.text = "";
        isActive = false;
    }

    IEnumerator LoadAchiveCollect(int collectableID, string collectableDesc)
    {
        AchivePanel.SetActive(true);
        title.text = "Collection " + collectableID + " Collected!";
        desc.text = collectableDesc;

        yield return new WaitForSeconds(showTime);
        AchivePanel.SetActive(false);
        title.text = "";
        desc.text = "";
        isActive = false;

    }
}
