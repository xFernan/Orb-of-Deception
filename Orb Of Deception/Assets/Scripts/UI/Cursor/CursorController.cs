using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public class CursorController : MonoBehaviour
    {
        private static CursorController _instance;

        public static CursorController Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CursorController>();
                    if (_instance == null)
                    {
                        var newMusicManager = Instantiate((GameObject) Resources.Load("Cursor"));
                        _instance = newMusicManager.GetComponent<CursorController>();
                    }
                }
                
                return _instance;
            }
        }

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image pointerCursorSprite;
        [SerializeField] private Image sightCursorSprite;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            _instance = this;
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            RePosition();
        }

        private void RePosition()
        {
            var positionSnapped = new Vector3
            {
                x = Mathf.Round(Input.mousePosition.x / Screen.width * GameManager.WidthInPixels) - (float) GameManager.WidthInPixels / 2,
                y = Mathf.Round(Input.mousePosition.y / Screen.height * GameManager.HeightInPixels) - (float) GameManager.HeightInPixels / 2
            };

            rectTransform.anchoredPosition = positionSnapped;
        }

        public void DisplaySightCursor()
        {
            sightCursorSprite.enabled = true;
            pointerCursorSprite.enabled = false;
        }

        public void DisplayPointerCursor() 
        {
            sightCursorSprite.enabled = false;
            pointerCursorSprite.enabled = true;
        }

        public void HideCursor()
        {
            sightCursorSprite.enabled = false;
            pointerCursorSprite.enabled = false;
        }
    }
}
