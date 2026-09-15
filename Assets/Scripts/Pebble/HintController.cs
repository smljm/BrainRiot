using UnityEngine;

namespace BrainRiot.Pebble
{
    // 힌트 1~3단계 패널 토글. 힌트를 쓰면 GameManager에 기록해서 별점에 반영.
    public class HintController : MonoBehaviour
    {
        [SerializeField] private GameObject[] hintPanels; // 순서대로 힌트1, 힌트2, 힌트3
        [SerializeField] private NodeView firstMoveHintTarget; // 힌트3에서 반짝일 노드 (예: A1)

        public void ToggleHint(int index)
        {
            if (index < 0 || index >= hintPanels.Length) return;
            bool nowOn = !hintPanels[index].activeSelf;
            hintPanels[index].SetActive(nowOn);
            PebbleGameManager.Instance.MarkHintUsed();

            if (index == 2 && nowOn && firstMoveHintTarget != null)
            {
                // TODO: firstMoveHintTarget에 pulse 애니메이션(DOTween 등) 연결
            }
        }
    }
}
