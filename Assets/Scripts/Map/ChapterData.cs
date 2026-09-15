using UnityEngine;

namespace BrainRiot.Map
{
    // 챕터(미니게임) 하나의 메타데이터. 에디터에서 Assets > Create > BrainRiot > Chapter Data로 생성.
    [CreateAssetMenu(fileName = "ChapterData", menuName = "BrainRiot/Chapter Data")]
    public class ChapterData : ScriptableObject
    {
        public string chapterId;
        public string title;          // 예: "조약돌 쌓기"
        public string algorithmTag;   // 예: "재귀 · 스택"
        public bool locked;
        public string sceneName;      // 클릭 시 로드할 씬 이름
    }
}
