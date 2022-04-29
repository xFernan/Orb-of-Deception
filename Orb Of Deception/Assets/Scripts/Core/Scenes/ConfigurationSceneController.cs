using UnityEngine;

namespace OrbOfDeception.Core.Scenes
{
    public class ConfigurationSceneController : MonoBehaviour
    {
        private void Start()
        {
            LevelChanger.Instance.GoToScene("Backstage");
        }
    }
}
