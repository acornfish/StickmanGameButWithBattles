using System.Linq;
using UnityEngine;

public class SummonCard : MonoBehaviour
{
    public int price;
    public GameObject prefab;
    protected RectTransform rectTransform;
    protected Transform summonPoint;


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
        if (GoldSystem.PlayerGold >= price)
        {
            GoldSystem.PlayerGold -= price;
            Instantiate(prefab, summonPoint);
        }
    }

}
