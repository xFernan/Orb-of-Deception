using OrbOfDeception.Core;

namespace OrbOfDeception.UI
{
    public class NormalMenuController : MenuController
    {
        private HideableElement[] _elements;

        protected override void Awake()
        {
            base.Awake();
            
            _elements = GetComponentsInChildren<HideableElement>();
        }

        public override void Open()
        {
            base.Open();
            
            foreach (var element in _elements)
            {
                element.Show();
            }
        }

        public override void Close()
        {
            base.Close();
            
            foreach (var element in _elements)
            {
                element.Hide();
            }
        }
    }
}
