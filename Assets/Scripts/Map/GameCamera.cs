using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float limitX0, limitX1;
    public int speed = 10;
    Vector3 targetPos;
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        Vector3 pos = Vector3.MoveTowards(transform.position, transform.position + Vector3.right * inputHorizontal * Time.deltaTime * 100, speed*Time.deltaTime);
        pos = new Vector3(Mathf.Clamp(pos.x, limitX0, limitX1),
         pos.y,
         pos.z);
        transform.position = pos;
    }
}
