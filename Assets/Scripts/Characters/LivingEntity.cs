using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LivingEntity : GameEntity
{
    public bool isAlly = true;
    protected internal Rigidbody2D rb;
    [SerializeField] protected State _state;

    public int health = 100;
    public Transform BasePos;

    protected internal void Start()
    {
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
}

