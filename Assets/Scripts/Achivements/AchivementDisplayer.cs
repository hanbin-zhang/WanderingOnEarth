using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class AchivementDisplayer : MonoBehaviour
{

    public GameObject AchivePanel;
    public bool isActive = false;
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text desc;
    public int showTime = 5;

    private void Update()
    {
        Queue<AchivementEvent> AchiveQueue = AchivementsManager.AchiveQueue;
        AchivementEvent action = null;

        // Dequeue an action from the event queue
        lock (AchivementsManager.AchiveQueueLock)
        {
            
            if (AchiveQueue.Count > 0)
            {
                action = AchiveQueue.Dequeue();
            }
        }

        if (action != null)
        {
            Debug.Log(action.name);
            // Execute the action
            Debug.Log(AchivePanel.activeSelf);
            AchivePanel.SetActive(true);
            Debug.Log(AchivePanel.activeSelf);

            title.text = action.name;
            desc.text = action.description;


            Thread.Sleep(showTime);

            AchivePanel.SetActive(false);
            title.text = "";
            desc.text = "";

        }
    }
}
