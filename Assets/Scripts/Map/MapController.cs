using System.Collections.Generic;
using UnityEngine;

namespace BrainRiot.Map
{
    // 지도 화면: 챕터 데이터 목록을 받아 카드로 뿌려줌.
    public class MapController : MonoBehaviour
    {
        [SerializeField] private List<ChapterData> chapters;
        [SerializeField] private ChapterCardView cardPrefab;
        [SerializeField] private Transform row;

        private void Start()
        {
            foreach (var chapter in chapters)
            {
                var card = Instantiate(cardPrefab, row);
                card.Setup(chapter);
            }
        }
    }
}
