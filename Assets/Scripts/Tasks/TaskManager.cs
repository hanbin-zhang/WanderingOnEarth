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
        currentTask = new Task0();
        currentTask.Handle(this, null);
        ListenEvents();
        
    }

    private void ListenEvents()
    {
        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener(msg =>
        {            
            currentTask.Handle(this, msg);            
        });

        Manager.EventController.Get<OnPlantEvent>()?.AddListener(msg =>
        {
            currentTask.Handle(this, msg);
        });
    }

    public void ShowOnScreen(string text)
    {
        shownText.text = text;
    }
    
   
}

public abstract class Task
{   
    public abstract void Handle(TaskManager taskManger, BaseMessage msg);
}

public class Task0: Task
{
    public override void Handle(TaskManager taskManager, BaseMessage msg)
    {
        Manager.Invoke(() => taskManager.ShowOnScreen("Hi, Protect yourself and help make the world a better place!"), 0f, taskManager);
        Manager.Invoke(() => taskManager.ShowOnScreen("You have the option to place some items which can be found in the inventory."), 2f, taskManager);       
        Manager.Invoke(() => taskManager.ShowOnScreen("this land is polluted, cannot plant, press O"), 5f, taskManager);
        taskManager.currentTask = new Task1();
    }
}

public class Task1 : Task
{   
    public override void Handle(TaskManager taskManager, BaseMessage msg)
    {
        if (msg is OnStateChangeMessage)
        {
            OnStateChangeMessage onStateChangeMsg = msg.Of<OnStateChangeMessage>();
            if (onStateChangeMsg.stateBefore == StateLabel.POLLUTED && onStateChangeMsg.stateAfter == StateLabel.NORMAL)
            {
                Manager.Invoke(() => taskManager.ShowOnScreen("Congratulation! now you can plant!"), 0f, taskManager);             
                Manager.Invoke(() => taskManager.ShowOnScreen("plant 10 trees can be a safe space"), 5f, taskManager);
                taskManager.currentTask = new Task2();
            }
        }
    }
}

public class Task2 : Task
{    
    public override void Handle(TaskManager taskManager, BaseMessage msg)
    {
        if (msg is OnStateChangeMessage)
        {
            OnStateChangeMessage onStateChangeMsg = msg.Of<OnStateChangeMessage>();
            if (onStateChangeMsg.stateBefore == StateLabel.NORMAL && onStateChangeMsg.stateAfter == StateLabel.SAFE)
            {                
                Manager.Invoke(() => taskManager.ShowOnScreen("Congratulation! you are safe!"), 0f, taskManager);               
                Manager.Invoke(() => taskManager.ShowOnScreen("you can find some surprise!"), 5f, taskManager);
                taskManager.currentTask = new Task3();
            }
        }

        if (msg is OnPlantMessage)
        {
            OnPlantMessage onPlantMsg = msg.Of<OnPlantMessage>();
            if (onPlantMsg.name == "TreeMain")
            {
                int treeNum = Manager.StateController.GetStateProperty(onPlantMsg.pos).treeNumber;
                Manager.Invoke(() => taskManager.ShowOnScreen($"you are planting {treeNum} trees"), 0f, taskManager);
            }
        }
    }
}

public class Task3 : Task
{
    /*start = "you can find some surprise!";
        end = "Congratulation! you collect it!";*/
    public override void Handle(TaskManager taskManager, BaseMessage msg)
    {
                
    }
}
