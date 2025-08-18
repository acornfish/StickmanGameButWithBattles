using System;
using TMPro;
using UnityEngine;

public class GoldSystem : MonoBehaviour
{
    public static int EnemyGold = 200;
    private static TextMeshProUGUI goldIndicator;
    public TextMeshProUGUI goldIndicatorText;

    public static int PlayerGold
    {
        get
        {
            return int.Parse(goldIndicator.text);
        }
        set
        {
            goldIndicator.text = value.ToString();
        }
    }


    void Start()
    {
        goldIndicator = goldIndicatorText;
        PlayerGold = 150;
    }


}
