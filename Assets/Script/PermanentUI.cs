using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    //playerstat
    public int cherries = 0;
    public int Health = 5;
    public TextMeshProUGUI cherries_text;
    public Text health_amount;
    public static PermanentUI perma;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!perma)
        {
            perma = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Reset()
    {
        cherries = 0;
        cherries_text.text = cherries.ToString();
    }
}
