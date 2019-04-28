
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameSpawns", menuName = "Vendorling/CoinSpawns", order = 1)]
public class CoinSpawns : ScriptableObject {
    public List<CoinSpawn> Spawns = new List<CoinSpawn>();
    public int TargetAmount;
    public bool IsSolvable;
}