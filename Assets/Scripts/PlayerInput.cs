using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad 
{
    public class PlayerInput : IPlayerInput
    {
        public Vector3 initialPos { get; private set; }
        public Vector3 target { get; set; }

        private float xJumpDistance;

        public PlayerInput(float xJumpDistance)
        {
            this.xJumpDistance = xJumpDistance; 
        }

        public void ReadInput(Vector3 init, PlayerSetting playerSetting)
        {
            // Handle mouse click
            if (Input.GetMouseButtonDown(0))
            {
                initialPos = Input.mousePosition;
                return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                target = new Vector3(0, 0, xJumpDistance);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                target = new Vector3(0, 0, -xJumpDistance);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (Mathf.RoundToInt(init.x) > playerSetting.minX)
                    target = new Vector3(-xJumpDistance, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Mathf.RoundToInt(init.x) < playerSetting.maxX)
                    target = new Vector3(xJumpDistance, 0, 0);
            }

        }

        public void Calculate(Vector3 initPos, Vector3 finalPos)
        {
            if (Input.GetMouseButtonUp(0))
            {
                float disX = Mathf.Abs(initialPos.x - finalPos.x);
                float disY = Mathf.Abs(initialPos.y - finalPos.y);
                float x = 0;
                float z = 0;

                if (disX > 0 || disY > 0)
                {
                    if (disX > disY)
                    {
                        if (initialPos.x > finalPos.x)
                        {
                            Debug.Log("Left");
                            target = new Vector3(-xJumpDistance, 0, 0);
                        }
                        else
                        {
                            Debug.Log("Right");
                            target = new Vector3(xJumpDistance, 0, 0);
                        }
                    }
                    else
                    {
                        if (initialPos.y > finalPos.y)
                        {
                            Debug.Log("Down");
                            target = new Vector3(0, 0, -xJumpDistance);
                        }
                        else
                        {
                            Debug.Log("Up");
                            target = new Vector3(0, 0, xJumpDistance);
                        }
                    }
                }
                else
                {
                    target = new Vector3(0, 0, xJumpDistance);
                }
            }
        }
    }

}

