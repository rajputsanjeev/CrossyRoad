using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;

public class TileManager : MonoBehaviour , IObjectPool
{
    public TileGeneration tileGeneration;
    public Transform player;
    public Transform spawnPoint;
    public TileMotor tileMotor;

    private void Awake()
    {
        tileMotor = new TileMotor(player, tileGeneration, spawnPoint);
    }

    public void Update()
    {
        tileMotor.SpawnTile(this);
    }

    public void AddtoPool(GameObject gameObject)
    {
        tileMotor.objectPool.Add(gameObject);
    }
}
