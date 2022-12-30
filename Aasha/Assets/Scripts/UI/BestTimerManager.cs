using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestTimerManager : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    private string[] bestTime;

    private void Start()
    {
        bestTimeText = GameObject.Find("Best Time Text").GetComponent<TextMeshProUGUI>();

        string bestMinutes = "empty";
        string bestSeconds = "empty";
        string bestMilliseconds = "empty";

        if(UniversalScript.instance.bestTime != 0)
        {
            bestTime = UniversalScript.instance.getBestTime().Split(',');

            bestMinutes = bestTime[0];
            bestSeconds = bestTime[1];
            bestMilliseconds = bestTime[2];
        }

        if(bestMinutes != "empty")
        {
            bestTimeText.text = "Best Time: " + bestMinutes + ":" + bestSeconds + ":" + bestMilliseconds;
        }
        else
        {
            bestTimeText.text = "";
        }
    }
}
