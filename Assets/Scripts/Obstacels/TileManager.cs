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

    private void Awake()
    {
        tileMotor = new TileMotor(playerTransform, tileGeneration, spawnPoint, parent);
        for (int i = 0; i < 30; i++)
        {
            tileMotor.SpawnTile(this, 0f, 100f);
        }
    }

    public void Update()
    {
        tileMotor.SpawnTileFromPool(0f, 100f);
    }

    public void AddtoPool(GameObject gameObject)
    {
        tileMotor.objectPool.Add(gameObject);
    }
}
