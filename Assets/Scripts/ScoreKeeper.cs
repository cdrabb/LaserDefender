using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public static int score = 0;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        Reset();
    }

    private void Update()
    {
        
    }

    public void Score(int points)
    {
        score += points;
        text.text = score + "";
    }

    public static void Reset()
    {
        score = 0;
    }
}
