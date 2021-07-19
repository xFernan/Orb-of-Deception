using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrbOfDeception
{
    public class EssenceOfPunishmentSpawner : MonoBehaviour
    {
        #region Variables
        private class EssenceOfPunishment
        {
            public readonly GameObject essenceObject;
            public readonly int essenceValue;

            public EssenceOfPunishment(GameObject essenceObject, int essenceValue)
            {
                this.essenceObject = essenceObject;
                this.essenceValue = essenceValue;
            }
        }

        [SerializeField] private GameObject[] essenceOfPunishmentPrefabs;
        
        private List<EssenceOfPunishment> _essencesOfPunishment = new List<EssenceOfPunishment>();
        private ParticleSystem _spawnParticles;
        #endregion
        
        #region Methods

        #region MonoBehaviour Methods

        private void Awake()
        {
            _spawnParticles = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            foreach (var essencePrefab in essenceOfPunishmentPrefabs)
            {
                var newEssence = new EssenceOfPunishment(essencePrefab,
                    essencePrefab.GetComponent<EssenceOfPunishmentController>().Value);
                _essencesOfPunishment.Add(newEssence);
            }
            _essencesOfPunishment = _essencesOfPunishment.OrderByDescending(x => x.essenceValue).ToList();
        }

        #endregion
        public void SpawnEssences(int amount)
        {
            var essencesToSpawn = new List<GameObject>();
            var essenceIndex = 0;
            while (amount > 0)
            {
                if (_essencesOfPunishment[essenceIndex].essenceValue <= amount)
                {
                    var essenceToAdd = _essencesOfPunishment[essenceIndex];
                    essencesToSpawn.Add(essenceToAdd.essenceObject);
                    amount -= essenceToAdd.essenceValue;
                }
                else
                {
                    essenceIndex++;
                }
            }

            foreach (var essence in essencesToSpawn)
            {
                Instantiate(essence, transform.position, Quaternion.identity);
            }
            
            _spawnParticles.Play();
        }
        #endregion
    }
}
