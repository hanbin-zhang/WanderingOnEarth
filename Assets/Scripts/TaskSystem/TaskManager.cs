using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OnPlantEvent;
using static OnStateChangeEvent;

public class TaskManager : MonoBehaviour
{
    public Task currentTask;
    public TMPro.TMP_Text shownText;

    private void Start()
    {
        SetTask(new Task1());
        ListenEvents();
    }

    private void ListenEvents()
    {
        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener(msg =>
        {            
            currentTask.OnEvent(this, msg);            
        });

        Manager.EventController.Get<OnPlantEvent>()?.AddListener(msg =>
        {
            currentTask.OnEvent(this, msg);
        });
    }

    public void ShowOnScreen(string text)
    {
        shownText.text = text;
        // Debug.LogError($"{text}");
    }
    
    public void SetTask(Task nextTask, float time = 0)
    {
        if (currentTask != nextTask)
        {
            if (currentTask != null) currentTask.OnTaskComplete(this);
            currentTask = nextTask; 
            Manager.Invoke(() => {
                nextTask.OnTaskActivate(this);
            }, time, this);
        }
    }   
}

public abstract class Task
{   
    public abstract void OnEvent(TaskManager taskManger, BaseMessage msg);
    public abstract void OnTaskActivate(TaskManager taskManager);
    public abstract void OnTaskComplete(TaskManager taskManager);
}

public class Task1 : Task
{   
    public override void OnEvent(TaskManager taskManager, BaseMessage msg)
    {
        if (msg is OnStateChangeMessage)
        {
            OnStateChangeMessage onStateChangeMsg = msg.Of<OnStateChangeMessage>();
            if (onStateChangeMsg.stateBefore == StateLabel.POLLUTED && onStateChangeMsg.stateAfter == StateLabel.NORMAL)
            {                                                           
                taskManager.SetTask(new Task2(), 2f);                
            }
        }
    }

    public override void OnTaskActivate(TaskManager taskManager)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("Hi, Protect yourself and help make the world a better place!"), 0f, taskManager);
        Manager.Invoke(() => taskManager.ShowOnScreen("You have the option to place some items which can be found in the inventory."), 2f, taskManager);
        Manager.Invoke(() => taskManager.ShowOnScreen("this land is polluted, cannot plant, press O"), 4f, taskManager);
    }

    public override void OnTaskComplete(TaskManager taskManager)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("Congratulation! now you can plant!"), 0f, taskManager);
    }
}

public class Task2 : Task
{    
    public override void OnTaskActivate(TaskManager taskManager)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("plant 10 grasses and 10 bushes can be a safe space"), 0f, taskManager);
    }

    public override void OnEvent(TaskManager taskManager, BaseMessage msg) {
        if (msg is OnStateChangeMessage) {
            OnStateChangeMessage onStateChangeMsg = msg.Of<OnStateChangeMessage>();
            if (onStateChangeMsg.stateBefore == StateLabel.NORMAL && onStateChangeMsg.stateAfter == StateLabel.SAFE) {
                taskManager.SetTask(new Task3(), 2f);               
            }
        }

        if (msg is OnPlantMessage) {
            OnPlantMessage onPlantMsg = msg as OnPlantMessage;
            if (onPlantMsg.name == LiveObject.Name<Grass>() || onPlantMsg.name == LiveObject.Name<Bush>()) {
                int grassNum = Manager.GameObjectManager.GetRegionalGameObjects<Grass>(onPlantMsg.pos).Count;       
                int bushNum = Manager.GameObjectManager.GetRegionalGameObjects<Bush>(onPlantMsg.pos).Count;          
                Manager.Invoke(() => taskManager.ShowOnScreen($"you are planting {grassNum} grasses, {bushNum} bushes"), 0f, taskManager);
            }
        }
    }

   

    public override void OnTaskComplete(TaskManager taskManager) {
        Manager.Invoke(() => taskManager.ShowOnScreen("Congratulation! you are safe!"), 0f, taskManager);
    }
}

public class Task3 : Task
{
    /*start = "you can find some surprise!";
        end = "Congratulation! you collect it!";*/
    public override void OnEvent(TaskManager taskManager, BaseMessage msg)
    {
                
    }

    public override void OnTaskActivate(TaskManager taskManager)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("you can find some surprise!"), 0f, taskManager);
    }

    public override void OnTaskComplete(TaskManager taskManager)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("Congratulation! you collect it!"), 0f, taskManager);
    }
}
