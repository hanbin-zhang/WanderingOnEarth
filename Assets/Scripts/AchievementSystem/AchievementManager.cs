using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static OnGreenValueReach100Event;
using static OnPlantEvent;
using static OnStateChangeEvent;

public class AchievementManager : MonoBehaviour
{
    private List<Achievement> achievements = new List<Achievement>()
    {
        new Achievement1(),
        new Achievement2(),
    };    
   
    public GameObject notificationPanel;
    public TMP_Text notificationText;
    private Queue<string> notificationQueue;


    void Start()
    {
        notificationQueue = Manager.EventController.NotificationQueue;
        ListenEvents();       
    }

    private void ListenEvents()
    {
        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener(msg =>
        {
            for (int i = 0; i < achievements.Count; i++)
            {
                achievements[i].OnEvent(this, msg);
            }
        });

        Manager.EventController.Get<OnPlantEvent>()?.AddListener(msg =>
        {
            for (int i = 0; i < achievements.Count; i++)
            {
                achievements[i].OnEvent(this, msg);
            }
        });

        Manager.EventController.Get<OnGreenValueReach100Event>()?.AddListener(msg =>
        {
            for (int i = 0; i < achievements.Count; i++)
            {
                achievements[i].OnEvent(this, msg);
            }
        });
    }

    

    public void Complete(Achievement achievement)
    {
        achievements.Remove(achievement);
    }

    public void ShowNotification(string text, float time = 3f)
    {
        notificationQueue.Enqueue(text);
        if (notificationQueue.Count == 1)
        {
            ProcessNotificationQueue(time);
        }
    }
    private void ProcessNotificationQueue(float time)
    {
        if (notificationQueue.Count > 0)
        {
            notificationText.text = notificationQueue.Peek();
            notificationPanel.SetActive(true);
            Manager.Invoke(() => {
                notificationQueue.Dequeue();
                ProcessNotificationQueue(time);
            }, time, this);
        }
        else
        {
            notificationPanel.SetActive(false);
            notificationText.text = "";
        }
    }
}

public abstract class Achievement
{
    public abstract void OnEvent(AchievementManager achievementManager, BaseMessage msg);

}

public class Achievement1 : Achievement
{
    // about regional state
    public override void OnEvent(AchievementManager achievementManager, BaseMessage msg)
    {
        if (msg is OnStateChangeMessage)
        {
            OnStateChangeMessage onStateChangeMsg = msg.Of<OnStateChangeMessage>();
            if (onStateChangeMsg.stateBefore == StateLabel.POLLUTED && onStateChangeMsg.stateAfter == StateLabel.NORMAL)
            {
                achievementManager.ShowNotification("[STATE CHANGE!] POLLUTED --> NORMAL");
            }
            if (onStateChangeMsg.stateBefore == StateLabel.NORMAL && onStateChangeMsg.stateAfter == StateLabel.SAFE)
            {
                achievementManager.ShowNotification("[STATE CHANGE!] NORMAL --> SAFE");
            }
            
        }
    }
}

public class Achievement2 : Achievement
{
    // about green value
    public override void OnEvent(AchievementManager achievementManager, BaseMessage msg)
    {
        if (msg is OnGreenValueReach100Message)
        {
            achievementManager.ShowNotification("[GREEN VALUE!]  --> 100");
            achievementManager.Complete(this);
        } 
        

    }
}
