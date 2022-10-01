using OrbOfDeception.Core;
using OrbOfDeception.Rooms;
using OrbOfDeception.UI.InGame_UI.Counter;
using UnityEngine;

namespace OrbOfDeception.Items
{
    public class CollectibleTriangleController : CollectibleController
    {
        [Space]
        
        [SerializeField] private int collectibleID;

        protected override void Start()
        {
            base.Start();
            
            if (SaveSystem.IsCollectibleAcquired(collectibleID))
                gameObject.SetActive(false);
        }

        protected override void OnCollect()
        {
            SaveSystem.AddCollectibleAcquired(collectibleID);
            CollectibleDisplayer.Instance.ShowCounterBriefly();
        }
    }
}