using System.Collections.Generic;
using UnityEngine;

namespace OrbOfDeception
{
    public class EssenceOfPunishmentLoader : MonoBehaviour
    {
        [System.Serializable]
        private class EssenceOfPunishment
        {
            public GameObject essencePrefab;
            public float essenceAmount;
        }

        [SerializeField] private EssenceOfPunishment[] _essencesOfPunishments;
    }
}
