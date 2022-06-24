
using UnityEngine;

namespace CrossyRoad.PlayerInputSystem
{
    public interface IPlayerInput
    {
        void ReadInput();

        void CalculateMousePosition();

        Vector3 initialPos { get; }

        Vector3 target { get; set; }
    }

}
