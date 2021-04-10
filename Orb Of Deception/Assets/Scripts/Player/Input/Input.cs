using UnityEngine;

namespace OrbOfDeception.Player
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
