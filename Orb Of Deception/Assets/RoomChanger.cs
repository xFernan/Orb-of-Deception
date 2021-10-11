using System;
using System.Collections;
using System.Collections.Generic;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception
{
    public class RoomChanger : MonoBehaviour
    {
        [SerializeField] private Transform playerPositionReference;
        [SerializeField] private string sceneName;
        [SerializeField] private int nextRoomPlayerPositionID;

        public Vector3 GetPlayerPlacePosition()
        {
            return playerPositionReference.position;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                LevelChanger.Instance.FadeToScene(sceneName);
                RoomManager.targetRoomChangerID = nextRoomPlayerPositionID;
            }
        }
    }
}
