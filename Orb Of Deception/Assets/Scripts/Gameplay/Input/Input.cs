using UnityEngine;

namespace OrbOfDeception.Gameplay.Input
{
    public class Input : MonoBehaviour
    {
        //private IInputController
    }

    internal interface IInputController
    {
        public float GetHorizontal();
        public bool GetJumpButtonDown();
    }
}
