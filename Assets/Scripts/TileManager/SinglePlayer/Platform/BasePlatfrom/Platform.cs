using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossyRoard;
using CrossyRoad.PhotonPlayerMovement;
using Photon.Pun;
using ObserverPattern.SinglePlayer;

namespace CrossyRoad.TileController.SinglePlayer.Platform
{
    public class Platform : MonoBehaviour , IObjectPool
    {
        public bool IsCollide { get; private set; }
        private bool randomizeValues = true;

        public Direction direction { get; private set; }
        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;
        private List<GameObject> generatedObjects = new List<GameObject>();
        public List<ObstacelType> obstacelTypes;
        public ObserverHandler addToObservers = new ObserverHandler();

        protected virtual void Awake()
        {
            generatedObjects = GetTileObstacal(obstacelTypes);
        }

        protected virtual void OnEnable()
        {
            GetRandomValue();
            StartCoroutine(GetObjectFromPool());
            StartCoroutine(CallObserver());
        }
        
        protected virtual void OnDisable()
        {
            IsCollide = false;
        }

        public void SetObstacelType(List<ObstacelType> obstacelTypes)
        {
            this.obstacelTypes = obstacelTypes;
            generatedObjects = GetTileObstacal(obstacelTypes);
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
        #region Instantiate Obstacal
        public List<GameObject> GetTileObstacal(List<ObstacelType> tileList)
        {
            if (tileList.Count == 0)
            {
                return null;
            }
            Debug.Log("tile list Count " + tileList.Count);

            List<GameObject> obstacelList = new List<GameObject>();

            int randomCount = Random.Range(3, 4);
            int random = Random.Range(0, tileList.Count - 1);

            for (int i = 0; i < randomCount; i++)
            {
                GameObject tileObstacel = Instantiate(tileList[random].obstacel);

                ObstacelObserver obstacelObserver = new ObstacelObserver(tileObstacel, tileObstacel.GetComponent<Movement>(), this);
                addToObservers.AddObserver(obstacelObserver);

                tileObstacel.SetActive(false);
                obstacelList.Add(tileObstacel);
            }

            return obstacelList;
        }

        private IEnumerator CallObserver()
        {
            yield return new WaitForSeconds(2);
            addToObservers.Notify();
            StartCoroutine(CallObserver());
        }
        #endregion

        public IEnumerator GetObjectFromPool()
        {
            if (generatedObjects == null)
            {
                yield return generatedObjects;
            }
            else if (generatedObjects.Count == 0)
            {
                yield return generatedObjects;
            }
            else
            {

                // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
                var position = transform.position + new Vector3(direction == Direction.LEFT ? Random.Range(rightX / 2, rightX) : Random.Range(leftX / 2, leftX), 0.6f, 0);

                int randomcount = Random.Range(2, generatedObjects.Count - 1);

                for (int i = 0; i < randomcount; i++)
                {
                    GameObject obj = generatedObjects[i];
                    obj.transform.position = position;
                    Movement movement = obj.GetComponent<Movement>();

                    if (movement != null)
                    {
                        movement.speedX = (int)direction * speed;
                        movement.SetDirection(direction, leftX, rightX, this);
                        movement.SetRoation();
                    }

                    obj.SetActive(true);
                    yield return new WaitForSeconds(Random.Range(4,5));
                }
            }
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

        public void AddToPool(GameObject gameObject)
        {
            generatedObjects.Add(gameObject);
            StartCoroutine(GetObjectFromPool());
        }
    }
}

