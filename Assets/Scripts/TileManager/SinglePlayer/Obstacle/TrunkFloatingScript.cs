using UnityEngine;
using System.Collections;
using CrossyRoard;
using Photon.Pun;

namespace CrossyRoad
{
    public class TrunkFloatingScript : Movement
    {

        // ==================================================================
        // TODO Make generic Sinerp() -- or use iTween or Animator instead :v
        // ==================================================================


        /// <summary>
        /// Time for sinking animation, in seconds.
        /// </summary>
        public float animationTime = 0.1f;

        /// <summary>
        /// Distance of the trunk sinking, in units.
        /// </summary>
        public float animationDistance = 0.1f;

        /// <summary>
        /// The water splash prefab to be instantiated.
        /// </summary>
        public GameObject splashPrefab;

        private float originalY;
        private bool sinking;
        private float elapsedTime;
        private Rigidbody playerBody;
        private PhotonView photonView;

        public bool IsCollide { get; private set; }

        protected override void Awake()
        {
            photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override void Start()
        {
            originalY = transform.position.y;
        }

        protected override void Update()
        {
            if (GameUtil.IsMultiplayer)
            {
                if (!photonView.IsMine)
                    return;
            }

            base.Update();  

            elapsedTime += Time.deltaTime;
            if (elapsedTime > animationTime)
            {
                sinking = false;
                transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            }

            if (sinking)
            {
                float y = Sinerp(originalY, originalY - animationDistance, (elapsedTime < animationTime) ? (elapsedTime / animationTime) : 1);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                playerBody = collision.gameObject.GetComponent<Rigidbody>();

                if (!sinking)
                {
                    var o = (GameObject)Instantiate(splashPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
                    o.transform.localScale = transform.localScale;

                    sinking = true;
                    elapsedTime = 0.0f;
                }
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("update score");
            if (!IsCollide)
                MyEventArgs.UIEvents.updateScore.Dispatch(1);

            IsCollide = true;
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                playerBody.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
            }
        }

        private float Sinerp(float min, float max, float weight)
        {
            return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
        }

    }

}

