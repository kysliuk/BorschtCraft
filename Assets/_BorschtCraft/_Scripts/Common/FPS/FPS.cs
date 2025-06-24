using UnityEngine;

namespace BorschtCraft
{
    public class FPSDisplay : MonoBehaviour
    {
        private float _deltaTime = 0.0f;
        private GUIStyle _style;
        private Rect _rect;

        private void Start()
        {
            int w = Screen.width, h = Screen.height;

            _rect = new Rect(50, 50, w, h * 2 / 100);
            _style = new GUIStyle();
            _style.alignment = TextAnchor.UpperLeft;
            _style.fontSize = h * 2 / 50;
            _style.normal.textColor = Color.white;
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            float fps = 1.0f / _deltaTime;
            string text = string.Format("{0:0.} FPS", fps);
            GUI.Label(_rect, text, _style);
        }
    }
}
