using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LivingEntity : GameEntity
{
    public bool isAlly = true;
    protected internal Rigidbody2D rb;
    [SerializeField] protected State _state;

    public float health = 100;
    public Transform BasePos;

    protected internal void Start()
    {
        if (isAlly) GameMaster.playerUnitCount++; else GameMaster.enemyUnitCount++;

        GameObject[] candidates = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject candidate in candidates)
        {
            Debug.Log(candidate.tag);
            if (candidate.tag == (isAlly ? "PlayerBase" : "EnemyBase"))
            {
                BasePos = candidate.transform;
            }
        }
    }

    protected internal void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }


    protected internal void Die() {
        if (isAlly) GameMaster.playerUnitCount--; else GameMaster.enemyUnitCount--;
    }
}

