using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainRiot.Pebble
{
    // 통나무 조각 저장소 시각화 (HTML의 reserve-row와 동일한 역할)
    public class ReserveUI : MonoBehaviour
    {
        [SerializeField] private Image iconPrefab;
        [SerializeField] private Transform row;
        [SerializeField] private Color activeColor = new Color(0.788f, 0.541f, 0.310f); // #C98A4F
        [SerializeField] private float usedAlpha = 0.2f;

        private readonly List<Image> _icons = new List<Image>();

        private void Start()
        {
            var mgr = PebbleGameManager.Instance;
            for (int i = 0; i < mgr.ReserveTotal; i++)
            {
                var icon = Instantiate(iconPrefab, row);
                icon.color = activeColor;
                _icons.Add(icon);
            }
            mgr.OnStateChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (PebbleGameManager.Instance != null)
                PebbleGameManager.Instance.OnStateChanged -= Refresh;
        }

        private void Refresh()
        {
            int available = PebbleGameManager.Instance.Available();
            for (int i = 0; i < _icons.Count; i++)
            {
                var c = activeColor;
                c.a = i < available ? 1f : usedAlpha;
                _icons[i].color = c;
            }
        }
    }
}
