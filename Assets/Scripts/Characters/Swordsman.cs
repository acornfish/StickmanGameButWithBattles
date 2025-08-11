using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swordsman : LivingEntity
{
    public float speed;
    private LivingEntity target;

    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        _state = State.Idle;
    }

    void Update()
    {
        ZPos = Mathf.RoundToInt(transform.position.z);

        switch (_state)
        {
            case State.Idle:
                if (GameMaster.currentPlayerState == playerState.Offensive)
                {
                    findTarget();
                }
                break;

            case State.WalkingToTarget:
                walkToTarget();
                break;

            case State.EngagedInBattle:
                onEngagedInBattle();
                break;

            case State.WalkingToBase:
                walkToBase();
                break;
        }
    }

    void findTarget()
    {
        LivingEntity[] entities = FindObjectsByType<LivingEntity>(FindObjectsSortMode.None);
        float lastTargetDistance = int.MaxValue;
        foreach (LivingEntity entity in entities)
        {
            if (entity.isAlly != isAlly)
            {
                float distance = Vector3.Distance(transform.position, entity.transform.position);
                if (distance < lastTargetDistance)
                {
                    target = entity;
                    lastTargetDistance = distance;
                    _state = State.WalkingToTarget;
                }
            }
        }
    }


    void walkToTarget()
    {
        Vector3 TargetPos = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, TargetPos) < 1f) _state = State.EngagedInBattle;
    }

    void onEngagedInBattle()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 1f) _state = State.Idle;

        //Bunu ömer arkadaşıma salıyorum kesinlikle üşenmem ile alakası yok -invalid
    }

    void walkToBase()
    {
        Vector3 TargetPos = BasePos.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        if (transform.position == TargetPos) _state = State.Idle;
    }

    void getInFormation()
    {
        // En keyifli yeri yine sana bıraktım ömer arkadaşım
    }
}
