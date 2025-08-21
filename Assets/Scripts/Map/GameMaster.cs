using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum TroopType
    {
        Miner = 5,
        SwordsMan = 3,
        Archer = 4
    }

    public static playerState currentPlayerState = playerState.Defensive, currentEnemyState = playerState.Defensive;

    [SerializeField] playerState playerState;

    public static int playerUnitCount = -1, enemyUnitCount = -1; // To account for castles being LivingEntities
    [SerializeField] TextMeshProUGUI playerUnitCountText, enemyUnitCountText;

    public static int[] playerFormation = new int[50], enemyFormation = new int[50];

    private static Transform _playerFormationOrigin, _enemyFormationOrigin;
    public Transform playerFormationOrigin, enemyFormationOrigin;

    static Queue<Tuple<TroopType, float, Action>> playerSummonQueue = new Queue<Tuple<TroopType, float, Action>>();
    private bool isCurrentlyTraining = false;

    void Awake()
    {
        _playerFormationOrigin = playerFormationOrigin;
        _enemyFormationOrigin = enemyFormationOrigin;


        for (int i = 0; i < 50; i++)
        {   
            playerFormation[i] = -1;
            enemyFormation[i] = -1;
        }

        playerState = currentPlayerState;
    }

    void Update()
    {
        currentPlayerState = playerState;
        playerUnitCountText.text = playerUnitCount.ToString() + "/50";
        enemyUnitCountText.text = enemyUnitCount.ToString() + "/50";

        for (int i = 0; i < 49; i++)
        {
            if (playerFormation[i] == -1)
            {
                playerFormation[i] = playerFormation[i + 1];
                playerFormation[i + 1] = -1;
            }
            if (enemyFormation[i] == -1)
            {
                enemyFormation[i] = enemyFormation[i + 1];
                enemyFormation[i + 1] = -1;
            }
        }
        //Update troop summons
        if (playerSummonQueue.Count > 0 && !isCurrentlyTraining)
        {
            isCurrentlyTraining = true;
            StartCoroutine(spawnNewSoldier());
        }
    }

    public static int putInFormation(bool isAlly, int id)
    {
        if (isAlly)
        {
            for (int i = 0; i < 50; i++)
            {
                if (playerFormation[i] == -1)
                {
                    playerFormation[i] = id;
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < 50; i++)
            {
                if (enemyFormation[i] == -1)
                {
                    enemyFormation[i] = id;
                    return i;
                }
            }
        }
        return -1;
    }
    
    public static int removeFromFormation(bool isAlly, int id)
    {
        if (isAlly)
        {
            for (int i = 0; i < 50; i++)
            {
                if (playerFormation[i] == id)
                {
                    playerFormation[i] = -1;
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < 50; i++)
            {
                if (enemyFormation[i] == id)
                {
                    enemyFormation[i] = -1;
                    return i;
                }
            }
        }
        return -1;
    }

    public static int findInFormation(bool isAlly, int id)
    {
        if (isAlly)
        {
            for (int i = 0; i < 50; i++)
            {
                if (playerFormation[i] == id)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < 50; i++)
            {
                if (enemyFormation[i] == id)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static Vector2 mapFormationIndexToPosition(bool isAlly, int index)
    {
        int coefficent = isAlly ? 1 : -1;
        return (isAlly ? _playerFormationOrigin.position : _enemyFormationOrigin.position) + new Vector3(coefficent * -(index / 5), -((float)index % 5));
    }

    public static void gameOver()
    {
        //TODO: ENES buraya oyun bitiş ekranı koy
    }

    IEnumerator spawnNewSoldier()
    {
        var current = playerSummonQueue.Dequeue();
        yield return new WaitForSeconds(current.Item2);
        current.Item3.Invoke();
        isCurrentlyTraining = false;
    }

    public static void listNewSpawn(TroopType type, float summonTime, Action onSummon)
    {
        playerSummonQueue.Enqueue(new Tuple<TroopType, float, Action>(type, summonTime, onSummon));
    }


    public void setPlayerStateGarrison() { playerState = playerState.Garrison; }
    public void setPlayerStateDefense() { playerState = playerState.Defensive; }
    public void setPlayerStateOffense() { playerState = playerState.Offensive; }

}
