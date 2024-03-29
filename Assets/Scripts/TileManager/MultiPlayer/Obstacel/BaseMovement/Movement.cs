using Photon.Pun;
using UnityEngine;

namespace CrossyRoad.TileController.MultiPlayer.Platform
{
    public class Movement : MonoBehaviour
    {
        /// <summary>
        /// The X-speed of floating trunk, in units per second.
        /// </summary>
        public float speedX = 5.0f;
        public Direction direction;
        public float leftX = -16;
        public float rightX = 16;
        private Platform platform;
        public RotateOver rotationFixed;
        public Quaternion quaternion;
        public float leftDir;
        public float rightDir;
        private PhotonView photonView;

        protected virtual void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        protected virtual void Start()
        {

        }

        public void SetDirection(Direction direction, float leftX, float rightX, Platform platform)
        {
            this.direction = direction;
            this.leftX = leftX;
            this.rightX = rightX;
            this.platform = platform;
        }

        public void SetRoation() 
        {
            switch (rotationFixed)
            {
                case RotateOver.X_AXIES:

                    if (direction < 0)
                        transform.rotation = Quaternion.Euler(leftDir, quaternion.x, quaternion.z);
                    else
                        transform.rotation = Quaternion.Euler(rightDir, quaternion.x, quaternion.z);

                    break;
                case RotateOver.Y_AXIES:

                    if (direction < 0)
                        transform.rotation = Quaternion.Euler(quaternion.x, leftDir, quaternion.z);
                    else
                        transform.rotation = Quaternion.Euler(quaternion.x, rightDir, quaternion.z);

                    break;
                case RotateOver.Z_AXIES:

                    if (direction < 0)
                        transform.rotation = Quaternion.Euler(quaternion.x, quaternion.y, leftDir);
                    else
                        transform.rotation = Quaternion.Euler(quaternion.x, quaternion.y, rightDir);

                    break;
            }
            
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!photonView.IsMine)
                return;

            transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
        }

        protected virtual void OnTriggerEnter()
        {

        }
    }

}
