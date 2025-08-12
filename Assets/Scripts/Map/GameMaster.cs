using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static playerState currentPlayerState = playerState.Defensive;

    [SerializeField] playerState playerState;

    public static int playerUnitCount, enemyUnitCount;
    [SerializeField] TextMeshProUGUI playerUnitCountText, enemyUnitCountText;

    public static int[] playerFormation = new int[50], enemyFormation = new int[50];

    private static Transform _playerFormationOrigin, _enemyFormationOrigin;
    public Transform playerFormationOrigin, enemyFormationOrigin;

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
            return (isAlly ? _playerFormationOrigin.position : _enemyFormationOrigin.position) + new Vector3(-(index / 5), -(index % 5));
    }
}
