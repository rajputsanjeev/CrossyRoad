using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossyRoard;
using Photon.Pun;
using System.IO;
using ObserverPattern.Multiplayer;

namespace CrossyRoad.TileController.MultiPlayer.Platform 
{ 
    public class Platform : MonoBehaviour
    {
        public bool IsCollide { get; private set; }
        private bool randomizeValues = true;
        public PhotonView photonView;

        public Direction direction { get; private set; }
        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;
        public List<ObstacelType> obstacelTypes;
        public ObserverHandler addToObservers = new ObserverHandler();
        public TileType tileType;

        protected virtual void Awake()
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

        protected virtual void OnEnable()
        {
            if (photonView.IsMine)
            {
                GetRandomValue();
                StartCoroutine(CallObserver());
            }
        }
        
        protected virtual void OnDisable()
        {
            IsCollide = false;
        }

        public void SetObstacelType(List<ObstacelType> obstacelTypes, TileType tileType)
        {
            this.obstacelTypes = obstacelTypes;
            this.tileType = tileType;   
           StartCoroutine(GetTileObstacal(tileType, obstacelTypes));
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

        private IEnumerator CallObserver()
        {
            yield return new WaitForSeconds(2);
            addToObservers.Notify();
            StartCoroutine(CallObserver());
        }

        private IEnumerator GetTileObstacal(TileType tileType, List<ObstacelType> tileList)
        {
            if (tileList.Count == 0)
            {
                yield return null;
            }
            Debug.Log("tile list Count " + tileList.Count);


            int randomCount = UnityEngine.Random.Range(2, 3);

            int randomTile = UnityEngine.Random.Range(0, tileList.Count - 1);


            for (int i = 0; i < randomCount; i++)
            {

                // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
                var position = transform.position + new Vector3(direction == Direction.LEFT ? Random.Range(rightX / 2, rightX) : Random.Range(leftX / 2, leftX), 0.6f, 0);

                GameObject tileObstacel = PhotonNetwork.InstantiateRoomObject(Path.Combine(Path.Combine("Obstacel", tileType.ToString().ToLower()), tileList[0].obstacelName), position, Quaternion.identity);

                Movement movement = tileObstacel.GetComponent<Movement>();

                if (movement != null)
                {
                    movement.speedX = (int)direction * speed;
                    movement.SetDirection(direction, leftX, rightX, this);
                    movement.SetRoation();
                }

                ObstacelObserver obstacelObserver = new ObstacelObserver(tileObstacel, movement);
                addToObservers.AddObserver(obstacelObserver);

                tileObstacel.SetActive(true);

                yield return new WaitForSeconds(Random.Range(4, 5));
            }

            yield return new WaitForSeconds(2);
            StartCoroutine(GetTileObstacal(tileType, obstacelTypes));
        }


        protected virtual void OnTriggerEnter(Collider collider)
        {

        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Debug.Log("update score");
            if (!IsCollide)
                MyEventArgs.UIEvents.updateScore.Dispatch(1);

            IsCollide = true;
        }
    }
}

