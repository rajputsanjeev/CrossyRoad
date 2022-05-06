using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class PlayerMoter
    {
        public string MoveDirection
        {
            get
            {
                if (moving)
                {
                    float dx = m_target.x - m_current.x;
                    float dz = m_target.z - m_current.z;
                    if (dz > 0)
                        return "north";
                    else if (dz < 0)
                        return "south";
                    else if (dx > 0)
                        return "west";
                    else
                        return "east";
                }
                else
                    return null;
            }
        }

        private readonly IPlayerInput m_player;
        private readonly Transform m_transform;
        private readonly PlayerSetting m_playerSetting;
        private Rigidbody m_rigidbody;
        private GameObject m_mesh;
        private Vector3 m_current;
        public float startY;
        private Vector3 m_target;
        private bool moving;

        public PlayerMoter(IPlayerInput player, Transform transform, PlayerSetting playerSetting, Rigidbody rigidbody, GameObject mesh)
        {
            this.m_player = player;
            this.m_transform = transform;
            this.m_playerSetting = playerSetting;
            this.m_rigidbody = rigidbody;
            this.m_mesh = mesh;
            startY = transform.position.y;
        }

        public void SetCurrentPosition()
        {
            if (moving)
                return;

            m_current = new Vector3(Mathf.Round(m_transform.position.x), Mathf.Round(m_transform.position.y), Mathf.Round(m_transform.position.z));
        }

        public bool IsMoving
        {
            get { return moving; }
        }

        public void Move()
        {
            if (moving)
                return;

            if (m_player.target == Vector3.zero)
                return;

            var newPosition = m_current + m_player.target;

            // Don't move if blocked by obstacle.
            if (Physics.CheckSphere(newPosition + new Vector3(0.0f, 0.5f, 0.0f), 0.1f))
                return;

            m_target = newPosition;

            moving = true;
            m_playerSetting.elapsedTime = 0;
            m_rigidbody.isKinematic = true;


            if (m_mesh != null)
            {
                switch (MoveDirection)
                {
                    case "north":
                        m_mesh.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case "south":
                        m_mesh.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case "east":
                        m_mesh.transform.rotation = Quaternion.Euler(0, 270, 0);
                        break;
                    case "west":
                        m_mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    default:
                        break;
                }

            }

        }

        public void MovePlayer()
        {
            if (!moving)
                return;

            m_playerSetting.elapsedTime += Time.deltaTime;

            float weight = (m_playerSetting.elapsedTime < m_playerSetting.timeForMove) ? (m_playerSetting.elapsedTime / m_playerSetting.timeForMove) : 1;
            float x = Lerp(m_current.x, m_target.x, weight);
            float z = Lerp(m_current.z, m_target.z, weight);
            float y = Sinerp(m_current.y, startY + m_playerSetting.jumpHeight, weight);

            Vector3 result = new Vector3(x, y, z);
            m_transform.position = result; // note to self: why using transform produce better movement?

            if (result == m_target)
            {
                moving = false;
                m_current = m_target;
                m_rigidbody.isKinematic = false;
                m_player.target = Vector3.zero;
                m_rigidbody.AddForce(0, -10, 0, ForceMode.VelocityChange);
            }
        }

        private float Lerp(float min, float max, float weight)
        {
            return min + (max - min) * weight;
        }

        private float Sinerp(float min, float max, float weight)
        {
            return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
        }
    }

}
