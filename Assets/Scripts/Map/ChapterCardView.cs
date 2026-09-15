using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BrainRiot.Map
{
    // 지도 화면의 챕터 카드 하나. MapController가 데이터를 주입.
    public class ChapterCardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text algorithmText;
        [SerializeField] private GameObject lockIcon;
        [SerializeField] private Button button;

        private ChapterData _data;

        public void Setup(ChapterData data)
        {
            _data = data;
            titleText.text = data.title;
            algorithmText.text = data.algorithmTag;
            lockIcon.SetActive(data.locked);
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (_data.locked)
            {
                // ToastController.Instance?.Show(_data.title + " 챕터는 다음 버전에서 만나요!");
                return;
            }
            SceneManager.LoadScene(_data.sceneName);
        }
    }
}
