using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;

public class TileManager : MonoBehaviour , IObjectPool
{
    public TileGeneration tileGeneration;
    public Transform playerTransform;
    public Transform spawnPoint;
    public Transform parent;
    public TileMotor tileMotor;

    public void Init()
    {
        tileMotor = new TileMotor(playerTransform, tileGeneration, spawnPoint, parent);
        for (int i = 0; i < 30; i++)
        {
            tileMotor.SpawnTileAtZero(this, 0f, 70f);
        }
    }

    public void Update()
    {
        if (playerTransform == null)
            return;

        tileMotor.SpawnTileFromPool(0f, 70f);
    }

    public void AddtoPool(GameObject gameObject)
    {
        Debug.Log("Before add " + tileMotor.objectPool.Count);
        tileMotor.objectPool.Add(gameObject);
        Debug.Log("after add " + tileMotor.objectPool.Count);
    }
}
