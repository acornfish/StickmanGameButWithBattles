using System;
using System.Linq;
using UnityEngine;

public class SummonCard : MonoBehaviour
{
    public int price;
    public GameObject prefab;
    protected RectTransform rectTransform;
    protected Transform summonPoint;
    [SerializeField] GameMaster.TroopType troopType;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        foreach(GameObject g in FindObjectsByType<GameObject>(FindObjectsSortMode.None)) {
            if (g.tag == "PlayerBase")
            {
                summonPoint = g.transform;
            }
        }
    }

    public void onPointerEnter()
    {
        transform.position += Vector3.up * 35;
    }

    public void onPointerExit()
    {
        transform.position += Vector3.down * 35;
    }

    public void onClick()
    {
        if (GoldSystem.PlayerGold >= price && (GameMaster.playerUnitCount < 50))
        {
            GoldSystem.PlayerGold -= price;

            GameMaster.listNewSpawn(troopType, (float)troopType, new Action(() =>
            {
                Instantiate(prefab, summonPoint);
            }));
        }
    }

}
