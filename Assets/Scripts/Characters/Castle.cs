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

    protected internal new void Die()
    {
        //Do not call base.Die();
        spriteRenderer.color = spriteRenderer.color + new Color(0,0,0,-Time.deltaTime);

    }
}
