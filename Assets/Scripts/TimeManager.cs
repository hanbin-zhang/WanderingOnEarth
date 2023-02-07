using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private TMP_Text Timer;
    public float second = 30;
 
    #region MonoBehaviour CallBacks
    private void Start()
    {
        Timer = this.GetComponent<TMP_Text>();
    }
 
    private void Update()
    {
 
        if (second > 0)
        {
            second = second - Time.deltaTime;
            if (second / 60 < 1)
            {
                if (second < 4)
                {
                    Timer.color = Color.red;
                }
                Timer.text = string.Format("00:{0:d2}", (int)second % 60);
            }
            else
            {
                Timer.text = string.Format("{0:d2}:{1:d2}", (int)second / 60, (int)second % 60);
            }
        }
        else
        {
            Timer.text = "00:00";
            Timer.color = Color.red;
        }
        
    }
    #endregion
}