using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;


public class AchivementEvent
{
    public string name;
    public string description;
}


public class AchivementsManager : MonoBehaviour
{  

    public GameObject AchivePanel;
    private readonly Object APanelLock = new();
    public bool isActive = false;
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text desc;
    public int showTime = 5;

    public float sumGreenValue = 0.0f;

    [HideInInspector] public static Queue<AchivementEvent> AchiveQueue = new();
    public readonly static Object AchiveQueueLock = new();

    // achivement 01
    public float achive01threshold = 10.0f;
    public int achive01code = 0;


    // achivement 02
    public int achive02threshold = 3;
    public int achive02code = 0;
    private Thread _thread;

    // achivement 03
    // collection

    public void Enqueue(AchivementEvent achivementEvent)
    {
        lock (AchiveQueueLock)
        {
            AchiveQueue.Enqueue(achivementEvent);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Achive01", 0);
        PlayerPrefs.SetInt("Achive02", 0);
        //AchivePanel.SetActive(false);

       /* _thread = new Thread(ListenQueue);
        _thread.Start();*/
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
        Debug.Log(isActive);
        //PlayerPrefs.SetInt("Achive01", 0);
        achive01code = PlayerPrefs.GetInt("Achive01");
        achive02code = PlayerPrefs.GetInt("Achive02");

        
        if (!isActive)
        {
            if (sumGreenValue >= achive01threshold && achive01code != 101)
            {
                achive01code = 101;
                PlayerPrefs.SetInt("Achive01", achive01code);
                AchivementEvent ae = new();
                ae.name = "Green Value increased!";
                ae.description = "Green value reaches " + achive01threshold;
                //


                Enqueue(ae);
            }

            else if (GameObjectTracker.TreeCount >= achive02threshold && achive02code != 102)
            {
                StartCoroutine(LoadAchive02());
            }
            else if (GameObjectTracker.collected.Count > 0)
            {
                int ID = GameObjectTracker.collected.FirstOrDefault().Key;
                string desc = GameObjectTracker.collected.FirstOrDefault().Value;
                GameObjectTracker.collected.Remove(ID);

                GameObjectTracker.updatedcollected.Add(ID);

                StartCoroutine(LoadAchive03(ID, desc));
            }
        }

    }

    IEnumerator LoadAchive01()
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

    IEnumerator LoadAchive02()
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

    IEnumerator LoadAchive03(int collectableID, string collectableDesc)
    {

        if (isActive == true) yield break;
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
    }

    private void ListenQueue()
    {
        while (true)
        {
            AchivementEvent action = null;

            // Dequeue an action from the event queue
            lock (AchiveQueueLock)
            {
                if (AchiveQueue.Count > 0)
                {
                    action = AchiveQueue.Dequeue();
                }
            }

            if (action != null)
            {
                // Execute the action
                AchivePanel.SetActive(true);

                title.text = action.name;
                desc.text = action.description;


                Thread.Sleep(showTime);

                AchivePanel.SetActive(false);
                title.text = "";
                desc.text = "";

            }
            else
            {
                // Sleep for a short time to avoid busy waiting
                Thread.Sleep(1);
            }
        }
    }
}
