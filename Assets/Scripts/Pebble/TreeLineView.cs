using UnityEngine;
using UnityEngine.UI;

namespace BrainRiot.Pebble
{
    // 두 원(RectTransform) 사이를 잇는 얇은 UI 선. Image가 붙은 오브젝트에 부착.
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class TreeLineView : MonoBehaviour
    {
        [SerializeField] private RectTransform from;
        [SerializeField] private RectTransform to;
        [SerializeField] private float thickness = 2f;

        private RectTransform _rect;

        private void Awake() => _rect = GetComponent<RectTransform>();

        private void LateUpdate()
        {
            if (from == null || to == null) return;

            Vector2 a = from.anchoredPosition;
            Vector2 b = to.anchoredPosition;
            Vector2 dir = b - a;
            float distance = dir.magnitude;

            _rect.sizeDelta = new Vector2(distance, thickness);
            _rect.anchoredPosition = a + dir / 2f;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _rect.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
