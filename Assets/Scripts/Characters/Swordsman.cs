using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Swordsman : LivingEntity
{
    public float speed;
    private LivingEntity target;
    private int id;
    [SerializeField] CircleCollider2D attackRange;

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

        if ((isAlly ? GameMaster.currentPlayerState : GameMaster.currentEnemyState) == playerState.Defensive)
        {
            _state = State.Idle;
            target = null;
        }

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

                    //check for anyone to attack in range
                    Collider2D[] targets = Physics2D.OverlapCircleAll(attackRange.transform.position + (Vector3)attackRange.offset, attackRange.radius);
                    if (targets.Any(x => x.tag == "Unit" && x.GetComponent<LivingEntity>().isAlly != isAlly))
                    {
                        target = targets.First(x => x.tag == "Unit" && x.gameObject != gameObject).GetComponent<LivingEntity>();
                        _state = State.WalkingToTarget;
                        return;
                    }
                }
                else
                {
                    _state = State.WalkingToBase;
                }
                break;

            case State.WalkingToTarget:
                walkToTarget();
                removeFromFormation();
                break;

            case State.EngagedInBattle:
                onEngagedInBattle();
                break;

            case State.WalkingToBase:
                walkToBase();
                removeFromFormation();
                break;
        }
    }

    void findTarget()
    {
        LivingEntity[] entities = FindObjectsByType<LivingEntity>(FindObjectsSortMode.None);
        float lastTargetDistance = int.MaxValue;
        target = null;
        foreach (LivingEntity entity in entities)
        {
            if (entity.isAlly != isAlly && entity.name != (isAlly ? "Enemy Castle" : "Player Castle"))
            {

                float distance = Vector3.Distance(transform.position, entity.transform.position);
                if (distance < lastTargetDistance)
                {
                    target = entity;
                    lastTargetDistance = distance;
                }
            }
        }
        if (!target) target = entities.First(entity => entity.name == (isAlly ? "Enemy Castle" : "Player Castle"));
        _state = State.WalkingToTarget;
    }


    void walkToTarget()
    {
        if (!target) _state = State.Idle;
        if (_state != State.WalkingToTarget) return;

        Vector3 TargetPos = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, TargetPos) < 1f) _state = State.EngagedInBattle;
    }

    void onEngagedInBattle()
    {
        if (!target) _state = State.Idle;
        if (_state != State.EngagedInBattle) return;
        if (Vector3.Distance(transform.position, target.transform.position) > 1.2f) _state = State.Idle;

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
    void removeFromFormation()
    {
        int index = GameMaster.findInFormation(isAlly, id);
        if (index != -1)
        {
            GameMaster.removeFromFormation(isAlly, id);
        }
    }


    protected internal override void Die()
    {
        base.Die();
        removeFromFormation();
        Destroy(gameObject);
    }
}
