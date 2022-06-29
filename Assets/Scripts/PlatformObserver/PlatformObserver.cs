using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CrossyRoad;
using Photon.Pun;

namespace ObserverPattern
{
    //Wants to know when another object does something interesting 
    public abstract class Observer
    {
        public abstract void OnAddToPool();
    }

    public class StaticPlatform : Observer
    {
        //The box gameobject which will do something
        GameObject boxObj;

        /// <summary>
        /// Object Polling
        /// </summary>
        private IObjectPool objectPoolLisner;

        /// <summary>
        /// Camera position to track the distance
        /// </summary>
        private Transform cameraTransform;

        /// <summary>
        /// Box Own position
        /// </summary>
        private Transform objectTransform;

        /// <summary>
        /// All tiles of that platform
        /// </summary>
        public List<GameObject> tileList = new List<GameObject>();

        //Variable of car and water object Mover and get random Direction
        public bool randomizeValues = true;

        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;

        /// <summary>
        /// Direction
        /// </summary>
        public Direction direction;

        public StaticPlatform(GameObject boxObj  , Transform cameraTransform ,Transform objectTransform , IObjectPool objectPoolLisner)
        {
            this.boxObj = boxObj;
            this.cameraTransform = cameraTransform;
            this.objectPoolLisner = objectPoolLisner;
            this.objectTransform = objectTransform;
        }

        public StaticPlatform(GameObject boxObj, Transform cameraTransform, Transform objectTransform)
        {
            this.boxObj = boxObj;
            this.cameraTransform = cameraTransform;
            this.objectTransform = objectTransform;
        }

        //Get Random value
        private void GetRandomValue()
        {
            if (randomizeValues)
            {
                direction = Random.value < 0.5f ? Direction.LEFT : Direction.RIGHT;
                speed = Random.Range(2.0f, 4.0f);
                interval = Random.Range(5.0f, 9.0f);
            }
        }

        //What the box will do if the event fits it (will always fit but you will probably change that on your own)
        public override void OnAddToPool()
        {
            //Debug.Log("Notification");
            CheckPosition();
        }

        //The box will always jump in this case
        void CheckPosition()
        {
            if (Vector3.Distance(boxObj.transform.position, cameraTransform.position) > 20 && boxObj.transform.position.z < cameraTransform.position.z)
            {
                if (!GameUtil.IsMultiplayer)
                {
                          Debug.Log("Addd");
                        objectPoolLisner.AddToPool(boxObj);
                        boxObj.SetActive(false);
                }
                else
                {
                        Debug.Log("Destroy");
                        PhotonNetwork.Destroy(boxObj);
                }
            }
            
          
        }
    }
}
