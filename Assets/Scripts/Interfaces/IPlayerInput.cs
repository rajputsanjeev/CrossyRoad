using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;
public interface IPlayerInput 
{
    void ReadInput();

    void CalculateMousePosition();

    Vector3 initialPos { get; }

    Vector3 target { get; set; }
}
