using OrbOfDeception.Switch;
using OrbOfDeception.UI.InGame_UI;
using UnityEngine;

namespace OrbOfDeception.UI.Tutorials
{
    public class SwitchTutorial : ButtonTutorial
    {
        [SerializeField] private SwitchController switchController;
        
        protected override void Update()
        {
            base.Update();

            var canBeShownBefore = canBeShown;
            canBeShown = !switchController.IsActivated();
            
            if (canBeShownBefore && !canBeShown) buttonShower.Hide();
        }
    }
}