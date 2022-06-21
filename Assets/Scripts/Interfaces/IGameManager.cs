using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public interface IGameManager
    {
        void OnCountdownCompleted();
        void UpdateScore();
    }
}

