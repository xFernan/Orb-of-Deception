using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrbOfDeception
{
    public class DontDestroyOnLoadObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
