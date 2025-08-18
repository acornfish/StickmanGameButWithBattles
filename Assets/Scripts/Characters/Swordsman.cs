using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swordsman : LivingEntity
{
    public float speed;
    private LivingEntity target;
    private int id;

    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        id = gameObject.GetInstanceID();
        _state = State.Idle;
    }

    new void Update()
    {
        base.Update();
        ZPos = Mathf.RoundToInt(transform.position.z);

        switch (_state)
        {
            case State.Idle:
                if ((isAlly ? GameMaster.currentPlayerState : GameMaster.currentEnemyState) == playerState.Offensive)
                {
                    findTarget();
                }
                else if ((isAlly ? GameMaster.currentPlayerState : GameMaster.currentEnemyState) == playerState.Defensive)
                {
                    getInFormation();
                }
                else
                {
                    _state = State.WalkingToBase;
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
        if (!target) _state = State.Idle;
        if (_state != State.EngagedInBattle) return;

        target.GetComponent<LivingEntity>().health -= Time.deltaTime * 10; //Just for prototyping
        // we need to implement a attack cycle
    }

    void walkToBase()
    {
        Vector3 TargetPos = BasePos.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        if (transform.position == TargetPos) _state = State.Idle;
    }

    void getInFormation()
    {
        int index = GameMaster.findInFormation(isAlly, id);
        if (index == -1)
        {
            index = GameMaster.putInFormation(isAlly, id);
        }
        Vector2 targetPos = GameMaster.mapFormationIndexToPosition(isAlly, index);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if ((Vector2)transform.position == targetPos) _state = State.Idle;
    }


    protected internal new void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
