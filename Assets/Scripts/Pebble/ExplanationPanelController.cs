using UnityEngine;
using UnityEngine.UI;

namespace BrainRiot.Pebble
{
    // 해설 카드: 기본 설명 ↔ 고1 심화 설명(콜 스택 · 공간 복잡도) 토글
    public class ExplanationPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject normalText;
        [SerializeField] private GameObject detailText; // 고1 심화 설명
        [SerializeField] private Toggle detailToggle;

        private void Awake()
        {
            detailToggle.onValueChanged.AddListener(OnToggle);
            OnToggle(detailToggle.isOn);
        }

        private void OnToggle(bool isOn)
        {
            detailText.SetActive(isOn);
            normalText.SetActive(!isOn);
        }
    }
}
