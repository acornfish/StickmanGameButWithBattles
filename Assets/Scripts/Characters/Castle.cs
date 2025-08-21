using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : LivingEntity
{
    SpriteRenderer spriteRenderer;
    new void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected internal override void Die()
    {
        //Do not call base.Die();
        GameMaster.gameOver();
        Destroy(gameObject);
    }
}
