using UnityEngine;
using TMPro;

namespace BrainRiot.Pebble
{
    // 클리어 결과 모달: 별점 + 조작 횟수 + 통나무 보상 표시
    public class ResultPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text starsText;
        [SerializeField] private TMP_Text opsText;
        [SerializeField] private TMP_Text woodText;

        private const string LitStar = "<color=#F2A93B>★</color>";
        private const string DimStar = "<color=#3C463F>★</color>";

        private void OnEnable() => PebbleGameManager.Instance.OnCleared += Show;

        private void OnDisable()
        {
            if (PebbleGameManager.Instance != null)
                PebbleGameManager.Instance.OnCleared -= Show;
        }

        private void Show(int stars, int ops, int wood)
        {
            panel.SetActive(true);
            string s = "";
            for (int i = 0; i < 3; i++) s += i < stars ? LitStar : DimStar;
            starsText.text = s;
            opsText.text = ops.ToString();
            woodText.text = wood.ToString();

            // TODO: WoodWallet.Instance.Add(wood); 같은 형태로 전체 재화 누적 연결
        }

        public void Close() => panel.SetActive(false);
    }
}
