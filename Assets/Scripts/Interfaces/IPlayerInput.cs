using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crossyroad;
public interface IPlayerInput 
{
    void ReadInput(Vector3 init, PlayerSetting playerSetting);

    void Calculate(Vector3 init, Vector3 final);

    Vector3 initialPos { get; }

    Vector3 target { get; set; }
}
