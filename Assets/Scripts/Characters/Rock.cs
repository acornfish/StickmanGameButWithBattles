using UnityEngine;

public class Rock : GameEntity
{
    public Transform miningPos;

    void Start()
    {
        ZPos = Mathf.RoundToInt(transform.position.z);
    }

    void Update()
    {
        
    }
}
