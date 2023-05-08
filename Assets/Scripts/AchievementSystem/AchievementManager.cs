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
        new Achievement3(),
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
        // Is it necessary to iterate through all the achivements in the list?
        // As the number of the achivements grows, the process time when each event is triggered grows
        // which could be a cause for some latency.

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
                achievementManager.ShowNotification("Try to plant something!");
            }
            if (onStateChangeMsg.stateBefore == StateLabel.NORMAL && onStateChangeMsg.stateAfter == StateLabel.SAFE)
            {
                achievementManager.ShowNotification("We are safe now!!!");
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
            achievementManager.ShowNotification("GREEN VALUE reached 100!");
            achievementManager.Complete(this);
        } 
        

    }
}

public class Achievement3 : Achievement
{
    // about plant a tree with a height over 100
    public override void OnEvent(AchievementManager achievementManager, BaseMessage msg)
    {
        if (msg is OnPlantMessage)
        {   
            OnPlantMessage onPlantMessage = msg.Of<OnPlantMessage>();
            if (onPlantMessage.pos.y >= 70)
            {
                achievementManager.ShowNotification("Reaching the highest tree in the world!");
                achievementManager.Complete(this);
            }           
        }


    }
}

