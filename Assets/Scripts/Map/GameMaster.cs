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
        // TODO: make this enumarator based
        if (playerSummonQueue.Count > 0 && playerSummonQueue.Peek().Item2 == Time.time)
        {
            var currentSummon = playerSummonQueue.Dequeue();
            currentSummon.Item3.Invoke();
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


    public static void listNewSpawn(TroopType type, float summonTime, Action onSummon)
    {
        playerSummonQueue.Enqueue(new Tuple<TroopType, float, Action>(type, summonTime, onSummon));        
    }

}
