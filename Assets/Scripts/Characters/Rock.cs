using UnityEngine;

public class Rock : GameEntity
{
    public Transform miningPos1, miningPos2;
    public bool isInUse = false;

    void Start()
    {
        ZPos = Mathf.RoundToInt(transform.position.z);
        
    }

    void Update()
    {
        
    }
}
