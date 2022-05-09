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
        public bool randomizeValues = false;
        public Direction direction;
        public float speed = 2.0f;
        public float interval = 6.0f;
        public float leftX = -20.0f;
        public float rightX = 20.0f;
        private float elapsedTime;
        private List<GameObject> generatedObjects = new List<GameObject>();

        public void SetPlayerTransform(Transform playerTransform , IObjectPool objectPool)
        {
            this.playerTransform = playerTransform;
            this.objectPoolLisner = objectPool; 
        }

        protected virtual void OnEnable()
        {
            if (randomizeValues)
            {
                direction = Random.value < 0.5f ? Direction.LEFT : Direction.RIGHT;
                speed = Random.Range(2.0f, 4.0f);
                interval = Random.Range(5.0f, 9.0f);
            }
        }

        protected virtual void OnDisable()
        {
            
        }

        public virtual void Init()
        {
            if (tileList.Count == 0)
                return;

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.LEFT ? rightX : leftX, 0.6f, 0);
            var objec = (GameObject)Instantiate(tileList[Random.Range(0, tileList.Count - 1)], position, Quaternion.identity);
            objec.GetComponent<Movement>().speedX = (int)direction * speed;
            objec.GetComponent<Movement>().SetDirection(direction,leftX,rightX,this);

            if (direction < 0)
                objec.transform.rotation = Quaternion.Euler(-90, 0, -90);
            else
                objec.transform.rotation = Quaternion.Euler(-90, 0, 90);

            generatedObjects.Add(objec);
        }

        public virtual void GetObjectFromPool()
        {
            if (generatedObjects.Count == 0)
                return;

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.LEFT ? rightX : leftX, 0.6f, 0);
            GameObject obj = generatedObjects[Random.Range(0, generatedObjects.Count - 1)];
            obj.transform.position  = position;
            obj.GetComponent<Movement>().speedX = (int)direction * speed;
            obj.GetComponent<Movement>().SetDirection(direction, leftX, rightX, this);

            if (direction < 0)
                obj.transform.rotation = Quaternion.Euler(-90, 0, -90);
            else
                obj.transform.rotation = Quaternion.Euler(-90, 0, 90);

            obj.SetActive(true);
        }


        private void Update()
        {
            if (playerTransform == null)
                return;

            if (transform.position.z <= playerTransform.position.z - 20f)
            {
                gameObject.SetActive(false);
                objectPoolLisner.AddtoPool(gameObject);
            }
        }
    }
}

