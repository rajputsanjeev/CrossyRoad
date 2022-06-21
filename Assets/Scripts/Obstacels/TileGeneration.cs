using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;

namespace Crossyroad {

    [CreateAssetMenu(menuName = "Tile/TileGeneration", fileName = "data")]
    public class TileGeneration : ScriptableObject
    {
        public List<Tile> tiles = new List<Tile>();
        public int minZ = 3;
        public int lineAhead = 10;
        public int lineBehind = 5;


        public T GetTile<T>() where T : Tile
        {
            T line = null;
            line = tiles[Random.Range(0, tiles.Count)] as T;
            return line;
        }

        public List<GameObject> GetTileObstacal(TileObstacal tileObstacal)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                int random = Random.Range(0, tileObstacal.tileList.Count) ;
               GameObject tileObstacel =  SpawnTile(tileObstacal.tileList[random]);
                list.Add(tileObstacel);
            }
            return list;
        }


        public GameObject SpawnTile(GameObject tile)
        {
            GameObject gameObject = Instantiate(tile) as GameObject;
            return gameObject;
        }



    }

    [System.Serializable]
    public class Tile
    {
        public TileType tileType;
        public GameObject tileObject;
        public List<GameObject> tileList;
        public TileObstacal tileObstacal;
    }

    [System.Serializable]
    public class TileObstacal
    {
        public List<GameObject> tileList;
    }
}

