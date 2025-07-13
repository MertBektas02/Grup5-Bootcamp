using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Entry Cost Data")]
public class LevelEntryCostData : ScriptableObject
{
    public string sceneName;
    public List<ResourceCost> costList;
}