/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuide : MonoBehaviour
{
    public TMPro.TMP_Text shownText;

    private Task[] taskList;
    private int index = 0;
    private StateLabel state;

    private void Start()
    {
        TaskList();
        ShowOnScreen(taskList[0].start);
    }

    private void Update()
    {
        CheckTaskState();
    }
    private void ShowOnScreen(string text)
    {
        shownText.text = text;
    }

    private void CheckTaskState()
    {
        state = Manager.StateController.GetStateProperty(transform.position).label;

        if (state == StateLabel.POLLUTED)
        {
            index = 1;
            ShowOnScreen(taskList[index].start);
        }

        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener(msg =>
        {
            if (msg.stateBeforeChange == StateLabel.POLLUTED && msg.stateAfterChange == StateLabel.NORMAL)
            {
                // ÇÐµ½ÈÎÎñ2
                index = 2;
                ShowOnScreen(taskList[index].start);
            }
        });

    }

    private void TaskList()
    {
        Task task0 = new Task()
        {
            start = "Hi, Protect yourself and help make the world a better place!",
            end = "Congratulation! you done your task0"
        };
        Task task1 = new Task()
        {
            start = "this land is polluted, cannot plant",
            end = "Congratulation! now you can plant!"
        };
        Task task2 = new Task()
        {
            start = "normal state, plantable",
            end = "Congratulation! you done your task2"
        };

        taskList = new Task[] {task0,task1,task2};
    }
}

private class Task
{
    public string start;
    public string end;
}
*/