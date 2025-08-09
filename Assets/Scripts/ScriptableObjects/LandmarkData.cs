using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "LandmarkData", menuName = "ScriptableObjects/LandmarkData", order = 1)]
public class LandmarkData : ScriptableObject
{
    public string name; 
    public string explanation; 
}
