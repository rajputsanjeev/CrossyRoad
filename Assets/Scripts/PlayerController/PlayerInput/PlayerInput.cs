using UnityEngine;

namespace CrossyRoad.PlayerInputSystem
{
    public class PlayerInput : IPlayerInput
    {
        public Vector3 initialPos { get; private set; }
        public Vector3 target { get; set; }
       
        private float xJumpDistance;

        public PlayerSetting playerSetting;

        public Transform playerPos;

        public PlayerInput(PlayerSetting playerSetting , Transform playerPos)
        {
            this.xJumpDistance = playerSetting.xJumpDistance; 
            this.playerSetting = playerSetting; 
            this.playerPos = playerPos;
        }

        public void ReadInput()
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
                if (Mathf.RoundToInt(playerPos.position.x) > playerSetting.minX)
                    target = new Vector3(-xJumpDistance, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Mathf.RoundToInt(playerPos.position.x) < playerSetting.maxX)
                    target = new Vector3(xJumpDistance, 0, 0);
            }
        }

        public void CalculateMousePosition()
        {
            if (Input.GetMouseButtonUp(0))
            {
                float disX = Mathf.Abs(initialPos.x - Input.mousePosition.x);
                float disY = Mathf.Abs(initialPos.y - Input.mousePosition.y);
               
                if (disX > 0 || disY > 0)
                {
                    if (disX > disY)
                    {
                        if (initialPos.x > Input.mousePosition.x)
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
                        if (initialPos.y > Input.mousePosition.y)
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

