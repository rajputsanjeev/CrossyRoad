using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;

public class TileManager : MonoBehaviour
{
    public TileGeneration tileGeneration;
    public List<Tile> tiles = new List<Tile>();
    public Transform player;
    public Transform spawnPoint;
    public TileMotor tileMotor;


    private void Awake()
    {
        tileMotor = new TileMotor(player, tileGeneration, spawnPoint);
    }

    public void Update()
    {
        tileMotor.SpawnTile();
    }
}
