using System.Collections;
using UnityEngine;

public class Miner : LivingEntity
{

    private bool onMine;
    private int goldAmount;
    private const int goldAmountPerSecond = 10;
    private int goldAmountLimit = 100;


    public float speed;
    private Rock TargetRock;


    new void Start()
    {
        base.Start();
        Debug.Log(ZPos + entityType);
        rb = GetComponent<Rigidbody2D>();
    }

    new void Update()
    {
        base.Update();
        ZPos = Mathf.RoundToInt(transform.position.z);

        switch (_state)
        {
            case State.Idle:
                Rock[] AllRocks = FindObjectsByType<Rock>(FindObjectsSortMode.None);
                OnIdle(AllRocks);
                break;

            case State.WalkingToTarget:
                if (TargetRock is not null)
                    OnWalkingToTarget();
                break;

            case State.Mining:
                OnMining();
                break;

            case State.WalkingToBase:
                OnWalkingToBase();
                break;
        }
    }

    private void OnIdle(Rock[] Rocks)
    {
        if (Rocks is null) return;

        foreach (var rock in Rocks)
        {
            if (rock.isInUse) continue;

            if (TargetRock is null)
                TargetRock = rock;

            else
            {
                var distanceRock = Vector2.Distance(transform.position, rock.miningPos.transform.position);
                var distanceTarget = Vector2.Distance(transform.position, TargetRock.miningPos.transform.position);

                if (distanceRock < distanceTarget) TargetRock = rock;
            }
        }

        if (!(TargetRock is null)) TargetRock.isInUse = true;
        _state = State.WalkingToTarget;
    }


    private void OnWalkingToTarget()
    {
        Vector3 TargetPos = TargetRock.miningPos.position;
        transform.position = Vector3.MoveTowards(transform.position,TargetPos, speed * Time.deltaTime);
        if (transform.position == TargetPos) _state = State.Mining;
    }


    private void OnMining()
    {
        if (onMine is false)
            StartCoroutine(MineTimer());
    }

    IEnumerator MineTimer()
    {
        onMine = true;
        while (goldAmount < goldAmountLimit)
        {
            TargetRock.isInUse = true;
            yield return new WaitForSeconds(1);
            goldAmount += goldAmountPerSecond; 
        }
        _state = State.WalkingToBase;
        TargetRock.isInUse = false;
        onMine = false;
    }

    private void OnWalkingToBase()
    {
        transform.position = Vector3.MoveTowards(transform.position, BasePos.transform.position, speed * Time.deltaTime);
        if (transform.position == BasePos.transform.position)
        {
            _state = State.Idle;
            if (isAlly)
            {
                GoldSystem.PlayerGold += goldAmount;
            }
            else {
                GoldSystem.EnemyGold += goldAmount;
            }
            goldAmount = 0;
        } 
    }

    protected internal new void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
