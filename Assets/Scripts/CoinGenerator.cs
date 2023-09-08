using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private int amountOfCoins;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int minCoins;
    [SerializeField] int maxCoins;
    [SerializeField] SpriteRenderer[] coinImage;
    void Start()
    {
        for (int i = 0; i < coinImage.Length; i++)
        {
            coinImage[i].sprite = null;
        } 
        amountOfCoins = Random.Range(minCoins, maxCoins);
        int additionalOffset = amountOfCoins / 2;
        for (int i = 0; i < amountOfCoins; i++)
        {
            Vector3 offset = new Vector2(i - additionalOffset, 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
        }
    }
}
