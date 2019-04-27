
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Vendorling/GameConfig", order = 1)]
public class GameConfig : ScriptableObject {
    public List<CoinConfig> Coins;
    public List<CoinSpawns> CoinSpawns;

    public CoinConfig GetCoinConfig(int coinID) {
        foreach(var coin in Coins)
        {
            if (coin.CoinID == coinID)
                return coin;
        }

        return null;
    }
}