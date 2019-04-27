
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Vendorling/GameConfig", order = 1)]
public class GameConfig : ScriptableObject {
    public List<CoinConfig> Coins;
}