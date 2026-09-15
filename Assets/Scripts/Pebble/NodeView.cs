using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrainRiot.Pebble
{
    // 트리의 원 하나(프리팹)에 붙는 컴포넌트. 클릭 처리 + 상태에 따른 시각 갱신.
    [RequireComponent(typeof(Button))]
    public class NodeView : MonoBehaviour
    {
        [SerializeField] private string nodeId;
        [SerializeField] private TMP_Text valueLabel;
        [SerializeField] private Image background;
        [SerializeField] private Color filledColor = new Color(0.247f, 0.749f, 0.624f); // #3FBF9F
        [SerializeField] private Color emptyColor  = new Color(0.122f, 0.227f, 0.196f); // #1F3A32

        private Button _button;
        private RectTransform _rect;

        public string NodeId => nodeId;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _rect = GetComponent<RectTransform>();
            _button.onClick.AddListener(() => PebbleGameManager.Instance.HandleNodeClick(nodeId));
        }

        private void OnEnable()
        {
            var mgr = PebbleGameManager.Instance;
            mgr.OnStateChanged += Refresh;
            mgr.OnActionRejected += HandleRejected;
            Refresh();
        }

        private void OnDisable()
        {
            var mgr = PebbleGameManager.Instance;
            if (mgr == null) return;
            mgr.OnStateChanged -= Refresh;
            mgr.OnActionRejected -= HandleRejected;
        }

        private void Refresh()
        {
            var mgr = PebbleGameManager.Instance;
            var data = mgr.Nodes[nodeId];
            valueLabel.text = data.value.ToString();
            background.color = mgr.IsFilled(nodeId) ? filledColor : emptyColor;
        }

        private void HandleRejected(string id, string message)
        {
            if (id != nodeId) return;
            // ToastController가 있다면 여기서 message를 띄워주면 됨:
            // ToastController.Instance?.Show(message);
            StopAllCoroutines();
            StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            Vector2 origin = _rect.anchoredPosition;
            float duration = 0.35f, elapsed = 0f;
            while (elapsed < duration)
            {
                float offset = Mathf.Sin(elapsed * 60f) * 6f;
                _rect.anchoredPosition = origin + new Vector2(offset, 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _rect.anchoredPosition = origin;
        }
    }
}
