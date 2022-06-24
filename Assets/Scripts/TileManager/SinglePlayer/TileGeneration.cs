using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossyRoad;

namespace CrossyRoad {

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

        public List<GameObject> GetTileObstacal(List<GameObject> tileList , Transform parent)
        {
            if (tileList.Count == 0)
            {
                return null;
            }
            Debug.Log("tile list Count " + tileList.Count);
            List<GameObject> list = new List<GameObject>();
           int random = Random.Range(0, tileList.Count-1) ;
            GameObject tileObstacel = Instantiate(tileList[random]);
            tileObstacel.SetActive(false);
            tileObstacel.transform.SetParent(parent, false);
            list.Add(tileObstacel);
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
    }

}
