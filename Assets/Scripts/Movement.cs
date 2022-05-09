using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class Movement : MonoBehaviour
    {
        /// <summary>
        /// The X-speed of floating trunk, in units per second.
        /// </summary>
        public float speedX = 0.0f;
        public Direction direction;
        public float leftX;
        public float rightX;
        private Platform platform;

        public void SetDirection(Direction direction, float leftX, float rightX , Platform platform)
        {
            this.direction = direction;
            this.leftX = leftX; 
            this.rightX = rightX;
            this.platform = platform;
        }
        // Start is called before the first frame update
       protected virtual void  Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
            if (direction == Direction.LEFT && transform.position.x < leftX || direction == Direction.RIGHT && transform.position.x > rightX)
            {
                gameObject.SetActive(false);
                platform.GetObjectFromPool();
            }

        }
    }

}
