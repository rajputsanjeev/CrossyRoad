using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossyRoard;
using CrossyRoad.PhotonPlayerMovement;
using Photon.Pun;

namespace CrossyRoad
{
    public class Platform : MonoBehaviour 
    {
        public bool IsCollide { get; private set; }
        private bool randomizeValues = true;

        public Direction direction { get; private set; }
        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;
        private List<GameObject> generatedObjects = new List<GameObject>();
        private List<int> IdCollides = new List<int>();
        public PhotonView photonView;

        protected virtual void Awake()
        {
            if (GameUtil.IsMultiplayer)
            {
                photonView = GetComponent<PhotonView>();

                if (photonView.InstantiationData != null)
                {
                    Debug.Log("Active " + (bool)photonView.InstantiationData[0]);
                    gameObject.SetActive((bool)photonView.InstantiationData[0]);
                    transform.position = (Vector3)photonView.InstantiationData[1];
                    transform.localScale = (Vector3)photonView.InstantiationData[2];
                }
            }
        }

        protected virtual void OnEnable()
        {
            if (GameUtil.IsMultiplayer)
            {
                if (photonView.IsMine)
                {
                    GetRandomValue();
                    GetObjectFromPool();
                }
            }
            else
            {
                GetRandomValue();
                GetObjectFromPool();
            }
           
        }
        
        protected virtual void OnDisable()
        {
            IsCollide = false;
        }

        public void SetObstacel(List<GameObject> generatedObjects)
        {
            this.generatedObjects = generatedObjects;
        }

        private void GetRandomValue()
        {
            if (randomizeValues)
            {
                direction = Random.value < 0.5f ? Direction.LEFT : Direction.RIGHT;
                speed = Random.Range(2.0f, 4.0f);
                interval = Random.Range(5.0f, 9.0f);
            }
        }

        public virtual void GetObjectFromPool()
        {
            if (generatedObjects.Count == 0)
                return;

            GetRandomValue();

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.LEFT ? Random.Range(rightX/2, rightX) : Random.Range(leftX/2, leftX) , 0.6f, 0);
            GameObject obj = generatedObjects[Random.Range(0, generatedObjects.Count - 1)];
            obj.transform.position  = position;
            Movement movement = obj.GetComponent<Movement>();
           // Debug.Log("position " + position);

            if(movement != null)
            {
                movement.speedX = (int)direction * speed;
                movement.SetDirection(direction, leftX, rightX, this);
                movement.SetRoation();
            }
           
            obj.SetActive(true);
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {

        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (GameUtil.IsMultiplayer)
            {
                if (IdCollides.Contains(collision.gameObject.GetComponent<PlayerMovement>().GetView().ViewID))
                     return;


                  collision.gameObject.GetComponent<PlayerMovement>().AddScore();
                  IdCollides.Add(collision.gameObject.GetComponent<PlayerMovement>().GetView().ViewID);
                  
            }
            else
            {
                Debug.Log("update score");
                if (!IsCollide)
                    MyEventArgs.UIEvents.updateScore.Dispatch(1);

                IsCollide = true;
            }
        
        }
    }
}

