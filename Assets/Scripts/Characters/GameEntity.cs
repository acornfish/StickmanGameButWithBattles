using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GameEntity : MonoBehaviour
{
    private int _ZPos;
    public int ZPos
    {
        get
        {
            return _ZPos;
        }
        set
        {
            _ZPos = value;
        }
    }

    public string entityType;
}
