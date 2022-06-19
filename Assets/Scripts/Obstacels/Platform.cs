using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class Platform : MonoBehaviour 
    {
        private IObjectPool objectPoolLisner;
        private Transform playerTransform;
        public List<GameObject> tileList = new List<GameObject>();
        public bool randomizeValues = true;
        public Direction direction;
        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;
        private List<GameObject> generatedObjects = new List<GameObject>();

        public void SetPlayerTransform(Transform playerTransform , IObjectPool objectPool)
        {
            this.playerTransform = playerTransform;
            this.objectPoolLisner = objectPool; 
        }

        protected virtual void OnEnable()
        {
            Debug.Log("onenable");
            GetRandomValue();
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
       
        public virtual void Init()
        {
            if (tileList.Count == 0)
                return;

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.LEFT ? rightX : leftX, 0.6f, 0);
            var objec = (GameObject)Instantiate(tileList[Random.Range(0, tileList.Count - 1)], position, Quaternion.identity);
            objec.GetComponent<Movement>().speedX = (int)direction * speed;
             Movement movement = objec.GetComponent<Movement>();
            movement.speedX = (int)direction * speed;
            movement.SetDirection(direction,leftX,rightX,this);
            movement.SetRoation();
            generatedObjects.Add(objec);
        }

        public virtual void GetObjectFromPool()
        {
            if (generatedObjects.Count == 0)
                return;

            GetRandomValue();

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.LEFT ? rightX : leftX, 0.6f, 0);
            GameObject obj = generatedObjects[Random.Range(0, generatedObjects.Count - 1)];
            obj.transform.position  = position;
            Movement movement = obj.GetComponent<Movement>();
            movement.speedX = (int)direction * speed;
            movement.SetDirection(direction, leftX, rightX, this);
            movement.SetRoation();
            obj.SetActive(true);
        }


        private void Update()
        {
            if (playerTransform == null)
                return;

            if (transform.position.z <= playerTransform.position.z - 40f)
            {
                gameObject.SetActive(false);
                for (int i = 0; i < generatedObjects.Count; i++)
                {
                    generatedObjects[i].SetActive(false);
                }
                objectPoolLisner.AddtoPool(gameObject);
            }
        }
        protected virtual void OnTriggerEnter()
        {

        }
    }
}

