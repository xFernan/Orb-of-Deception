using System.Collections.Generic;
using OrbOfDeception.Items;
using OrbOfDeception.Orb;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Rooms
{
    public static class SaveSystem
    {
        #region Enemies
        
        private static readonly Dictionary<int, List<int>> EnemiesKilled = new Dictionary<int, List<int>>();
        
        public static void AddEnemyDead(int enemyId)
        {
            var roomId = RoomManager.CurrentRoom.GetRoomID();
            
            if (!EnemiesKilled.ContainsKey(roomId))
                EnemiesKilled.Add(roomId, new List<int>());
            EnemiesKilled[roomId].Add(enemyId);
        }

        public static bool IsEnemyDead(int enemyID)
        {
            var roomID = RoomManager.CurrentRoom.GetRoomID();
            if (!EnemiesKilled.ContainsKey(roomID))
                return false;

            return EnemiesKilled[roomID].Contains(enemyID);
        }
        
        public static void ResetEnemies()
        {
            EnemiesKilled.Clear();
        }

        #endregion
        
        #region Breakable Decoration
        
        private static readonly Dictionary<int, List<int>> DecorationsBroken = new Dictionary<int, List<int>>();
        
        public static void AddDecorationBroken(int decorationID)
        {
            var roomId = RoomManager.CurrentRoom.GetRoomID();
            
            if (!DecorationsBroken.ContainsKey(roomId))
                DecorationsBroken.Add(roomId, new List<int>());
            DecorationsBroken[roomId].Add(decorationID);
        }

        public static bool IsDecorationBroken(int decorationID)
        {
            var roomID = RoomManager.CurrentRoom.GetRoomID();
            if (!DecorationsBroken.ContainsKey(roomID))
                return false;

            return DecorationsBroken[roomID].Contains(decorationID);
        }
        
        public static void ResetDecorations()
        {
            DecorationsBroken.Clear();
        }

        #endregion
        
        #region Irreparable Breakable Decoration
        
        private static readonly Dictionary<int, List<int>> IrreparableDecorationsBroken = new Dictionary<int, List<int>>();
        
        public static void AddIrreparableDecorationBroken(int decorationID)
        {
            var roomId = RoomManager.CurrentRoom.GetRoomID();
            
            if (!IrreparableDecorationsBroken.ContainsKey(roomId))
                IrreparableDecorationsBroken.Add(roomId, new List<int>());
            IrreparableDecorationsBroken[roomId].Add(decorationID);
        }

        public static bool IsIrreparableDecorationBroken(int decorationID)
        {
            var roomID = RoomManager.CurrentRoom.GetRoomID();
            if (!IrreparableDecorationsBroken.ContainsKey(roomID))
                return false;

            return IrreparableDecorationsBroken[roomID].Contains(decorationID);
        }

        #endregion
        
        #region Collectibles
        
        private static readonly HashSet<int> CollectiblesAcquired = new HashSet<int>();
        
        public static void AddCollectibleAcquired(int collectibleID)
        {
            CollectiblesAcquired.Add(collectibleID);
        }

        public static bool IsCollectibleAcquired(int collectibleID)
        {
            return CollectiblesAcquired.Contains(collectibleID);
        }

        public static int GetCollectiblesAcquiredAmount()
        {
            return CollectiblesAcquired.Count;
        }
        
        #endregion
        
        #region Masks
        
        public static readonly HashSet<PlayerMaskController.MaskType> MasksUnlocked = new HashSet<PlayerMaskController.MaskType>();
        public static PlayerMaskController.MaskType currentMaskType = PlayerMaskController.MaskType.ShinyMask;
        
        public static void UnlockMask(PlayerMaskController.MaskType maskToUnlock)
        {
            MasksUnlocked.Add(maskToUnlock);
        }

        public static bool IsMaskUnlocked(MaskItem maskToCheck)
        {
            return MasksUnlocked.Contains(maskToCheck.maskType);
        }
        
        #endregion
        
        #region Orbs
        
        private static readonly HashSet<OrbController.OrbType> OrbsObtained = new HashSet<OrbController.OrbType>();
        public static OrbController.OrbType currentOrbType = OrbController.OrbType.None;
        
        public static void AddOrbObtained(OrbController.OrbType orbToObtain)
        {
            OrbsObtained.Add(orbToObtain);
        }

        public static bool IsOrbObtained(OrbItem orbToCheck)
        {
            return OrbsObtained.Contains(orbToCheck.orbType);
        }
        
        #endregion
        
        #region Secret Walls
        
        private static readonly HashSet<int> SecretWallsDiscovered = new HashSet<int>();
        
        public static void AddSecretWallDiscovered(int secretWallID)
        {
            SecretWallsDiscovered.Add(secretWallID);
        }

        public static bool IsSecretWallDiscovered(int secretWallID)
        {
            return SecretWallsDiscovered.Contains(secretWallID);
        }
        
        #endregion
        
        #region Switches
        
        private static readonly HashSet<int> SwitchesActivated = new HashSet<int>();
        
        public static void AddSwitchActivated(int switchID)
        {
            SwitchesActivated.Add(switchID);
        }

        public static bool IsSwitchActivated(int switchID)
        {
            return SwitchesActivated.Contains(switchID);
        }
        
        #endregion
        
        #region Doors
        
        private static readonly HashSet<int> DoorsOpened = new HashSet<int>();
        
        public static void AddDoorOpened(int doorID)
        {
            DoorsOpened.Add(doorID);
        }

        public static bool IsDoorOpened(int doorID)
        {
            return DoorsOpened.Contains(doorID);
        }
        
        #endregion
        
        #region Wave Rooms
        
        private static readonly HashSet<int> WaveRoomsCompleted = new HashSet<int>();
        
        public static void AddWaveRoomCompleted(int waveRoomID)
        {
            WaveRoomsCompleted.Add(waveRoomID);
        }

        public static bool IsWaveRoomCompleted(int waveRoomID)
        {
            return WaveRoomsCompleted.Contains(waveRoomID);
        }
        
        #endregion
        
        #region Time Counter

        private static float _timeStart;
        public static float TimePlayed => Time.time - _timeStart;

        public static void InitTimeCounter()
        {
            _timeStart = Time.time;
        }

        #endregion
        
        #region Deaths Counter

        private static int _numberOfDeaths = 0;

        public static void AddDeathToCounter()
        {
            _numberOfDeaths++;
        }

        public static int GetNumberOfDeaths()
        {
            return _numberOfDeaths;
        }
        
        #endregion
        
        #region Hits Counter

        private static int _numberOfHits = 0;

        public static void AddHitToCounter()
        {
            _numberOfHits++;
        }

        public static int GetNumberOfHits()
        {
            return _numberOfHits;
        }
        
        #endregion
        
        #region Player Spawn Position

        private static string _sceneName;
        private static Vector3 _spawnPosition;

        public static void SetNewSpawn(string sceneName, Vector3 spawnPosition)
        {
            _sceneName = sceneName;
            _spawnPosition = spawnPosition;
        }

        public static string GetSpawnSceneName()
        {
            return _sceneName;
        }

        public static Vector3 GetSpawnPosition()
        {
            return _spawnPosition;
        }
        
        #endregion

        #region Orb Tutorials
        
        private static readonly HashSet<int> OrbTutorialsDisplayed = new HashSet<int>();
        
        public static void AddTutorialDisplayed(int tutorialID)
        {
            OrbTutorialsDisplayed.Add(tutorialID);
        }

        public static bool HasTutorialBeenDisplayed(int tutorialID)
        {
            return OrbTutorialsDisplayed.Contains(tutorialID);
        }
        
        #endregion
    }
}