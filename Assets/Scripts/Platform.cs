using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class Platform : MonoBehaviour 
    {
        public TileType tileType;
        public IObjectPool objectPoolLisner;
        public Transform playerTransform;

        public void SetPlayerTransform(Transform playerTransform , IObjectPool objectPool )
        {
            this.playerTransform = playerTransform;
            this.objectPoolLisner = objectPool; 
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public virtual void Init()
        {

        }

        private void Update()
        {
            if (playerTransform == null)
                return;

            if(Vector3.Distance(playerTransform.position, transform.position) < 20)
            {
                gameObject.SetActive(false);
                objectPoolLisner.AddtoPool(gameObject);
            }
        }
    }
}

