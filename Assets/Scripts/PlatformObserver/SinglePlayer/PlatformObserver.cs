using UnityEngine;
using System.Collections.Generic;
using CrossyRoad;
using Photon.Pun;
using CrossyRoad.TileController.SinglePlayer.Platform;

namespace ObserverPattern.SinglePlayer
{
    //Wants to know when another object does something interesting 
    public abstract class Observer
    {
        public abstract void Add(int i, List<Observer> observers);
    }

    public class PlatformObserver : Observer
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

        public PlatformObserver(GameObject boxObj  , Transform cameraTransform , IObjectPool objectPoolLisner)
        {
            this.boxObj = boxObj;
            this.cameraTransform = cameraTransform;
            this.objectPoolLisner = objectPoolLisner;
        }

        public PlatformObserver(GameObject boxObj, Transform cameraTransform)
        {
            this.boxObj = boxObj;
            this.cameraTransform = cameraTransform;
        }

        //What the box will do if the event fits it (will always fit but you will probably change that on your own)
        public override void Add(int i ,List<Observer> observers)
        {
            Debug.Log("Testing 0 "+ GetDistance());
            if (boxObj.transform.position.z < cameraTransform.position.z && boxObj.activeInHierarchy)
            {
                    Debug.Log("Testing 1");
                    objectPoolLisner.AddToPool(boxObj);
                    boxObj.SetActive(false);
               
            }

        }

        private float GetDistance()
        {
            return (boxObj.transform.position.z - cameraTransform.position.z);
        }
    }

    public class ObstacelObserver : Observer
    {
        //The box gameobject which will do something
        GameObject boxObj;

        /// <summary>
        /// Object Polling
        /// </summary>
        private IObjectPool objectPoolLisner;

        //Variable of car and water object Mover and get random Direction
        public bool randomizeValues = true;

        public float leftX = -20.0f;
        public float rightX = 20.0f;

        /// <summary>
        /// Direction
        /// </summary>
        public Movement movement;

        public ObstacelObserver(GameObject boxObj, Movement movement, IObjectPool objectPoolLisner)
        {
            this.boxObj = boxObj;
            this.movement = movement;
            this.objectPoolLisner = objectPoolLisner;
        }

        //What the box will do if the event fits it (will always fit but you will probably change that on your own)
        public override void Add(int i, List<Observer> observers)
        {
            Debug.Log("Notify "+ movement.direction);
            if (movement.direction == Direction.LEFT && boxObj.transform.position.x < leftX || movement.direction == Direction.RIGHT && boxObj.transform.position.x > rightX && boxObj.activeInHierarchy)
            {
                Debug.Log("Notify");
                boxObj.SetActive(false);
                objectPoolLisner.AddToPool((boxObj));
            }
        }
    }
}
