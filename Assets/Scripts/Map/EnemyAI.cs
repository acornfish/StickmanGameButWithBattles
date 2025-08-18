using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected Transform summonPoint;
    [SerializeField] GameObject minerPrefab, swordsmanPrefab, archerPrefab;
    [SerializeField] int minerPrice, swordsmanPrice, archerPrice;

    public int targetMinerCount = 1, targetSwordsmanCount = 2, targetArcherCount = 0;
    private int minerCount, swordsmanCount, archerCount;

    void Start()
    {
        foreach (GameObject g in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (g.tag == "EnemyBase")
            {
                summonPoint = g.transform;
            }
        }
    }

    void FixedUpdate()
    {

        if (minerCount < targetMinerCount && GoldSystem.EnemyGold >= minerPrice)
        {
            GoldSystem.EnemyGold -= minerPrice;
            GameObject currentSummon = Instantiate(minerPrefab, summonPoint);
            Miner currentSummonedMiner = currentSummon.GetComponent<Miner>();

            currentSummonedMiner.onDeath = new Action(() =>
            {
                minerCount--;
            });

            currentSummonedMiner.isAlly = false;

            minerCount++;
            return; // whatever you are gonna do, its going to wait until next cycle
        }

        if (swordsmanCount < targetSwordsmanCount && GoldSystem.EnemyGold >= swordsmanPrice)
        {
            GoldSystem.EnemyGold -= swordsmanPrice;
            GameObject currentSummon = Instantiate(swordsmanPrefab, summonPoint);
            Swordsman currentSummonedSwordsman = currentSummon.GetComponent<Swordsman>();

            currentSummonedSwordsman.onDeath = new Action(() =>
            {
                swordsmanCount--;
            });

            currentSummonedSwordsman.isAlly = false;

            swordsmanCount++;
            return; // whatever you are gonna do, its going to wait until next cycle
        }
    }
}
