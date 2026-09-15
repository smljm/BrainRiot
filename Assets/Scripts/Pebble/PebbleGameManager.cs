using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BrainRiot.Pebble
{
    // 조약돌 쌓기 게임의 상태를 관리하는 매니저.
    // HTML 프로토타입의 filled/history/ops 로직을 그대로 C#으로 옮긴 버전.
    //
    // DefaultExecutionOrder(-100): 이 스크립트의 Awake()가 씬의 다른 모든 스크립트보다
    // 먼저 실행되도록 강제. NodeView 등이 OnEnable에서 Instance를 참조하는데,
    // Hierarchy 순서에 따라 GameManager의 Awake가 늦게 실행되면 NullReferenceException이 남.
    [DefaultExecutionOrder(-100)]
    public class PebbleGameManager : MonoBehaviour
    {
        public static PebbleGameManager Instance { get; private set; }

        [Header("Tree Definition (비워두면 기본 트리 사용)")]
        [SerializeField] private List<PebbleNodeData> nodes = new List<PebbleNodeData>();

        [Header("Balance")]
        [SerializeField] private int reserveTotal = 4;
        [SerializeField] private int optimalOps = 9;

        private Dictionary<string, PebbleNodeData> _nodeMap;
        private readonly HashSet<string> _filled = new HashSet<string>();
        private readonly List<(string type, string id)> _history = new List<(string, string)>();
        private int _ops;
        private bool _hintUsed;

        public event Action OnStateChanged;
        public event Action<string, string> OnActionRejected; // (nodeId, message)
        public event Action<int, int, int> OnCleared;          // (stars, ops, woodReward)

        public int ReserveTotal => reserveTotal;
        public IReadOnlyDictionary<string, PebbleNodeData> Nodes => _nodeMap;
        public bool HasUndo => _history.Count > 0;

        private void Awake()
        {
            Instance = this;
            if (nodes.Count == 0) BuildDefaultTree();
            _nodeMap = nodes.ToDictionary(n => n.id, n => n);
        }

        // HTML 프로토타입과 동일한 6개 노드 트리 (root-A/B-A1/A2/B1)
        private void BuildDefaultTree()
        {
            nodes = new List<PebbleNodeData>
            {
                new PebbleNodeData{ id="root", value=1, children=new[]{"A","B"},        anchoredPosition=new Vector2(300, 284) },
                new PebbleNodeData{ id="A",    value=2, children=new[]{"A1","A2"},      anchoredPosition=new Vector2(170, 168) },
                new PebbleNodeData{ id="B",    value=1, children=new[]{"B1"},           anchoredPosition=new Vector2(430, 168) },
                new PebbleNodeData{ id="A1",   value=1, children=Array.Empty<string>(), anchoredPosition=new Vector2(90, 50) },
                new PebbleNodeData{ id="A2",   value=1, children=Array.Empty<string>(), anchoredPosition=new Vector2(250, 50) },
                new PebbleNodeData{ id="B1",   value=1, children=Array.Empty<string>(), anchoredPosition=new Vector2(430, 50) },
            };
        }

        public bool IsFilled(string id) => _filled.Contains(id);

        public int Available()
        {
            int used = _filled.Sum(id => _nodeMap[id].value);
            return reserveTotal - used;
        }

        private bool ChildrenFilled(string id) => _nodeMap[id].children.All(c => _filled.Contains(c));

        public void HandleNodeClick(string id)
        {
            if (_filled.Contains(id))
            {
                _filled.Remove(id);
                _history.Add(("unfill", id));
                _ops++;
                OnStateChanged?.Invoke();
                return;
            }

            if (!ChildrenFilled(id))
            {
                OnActionRejected?.Invoke(id, "아직 준비 안 됐어요! 아래 원부터 채워보세요.");
                return;
            }

            if (Available() < _nodeMap[id].value)
            {
                OnActionRejected?.Invoke(id, "통나무가 부족해요! 다른 원을 회수해보세요.");
                return;
            }

            _filled.Add(id);
            _history.Add(("fill", id));
            _ops++;
            OnStateChanged?.Invoke();

            if (id == "root") EvaluateClear();
        }

        public void Undo()
        {
            if (_history.Count == 0) return;
            var last = _history[^1];
            _history.RemoveAt(_history.Count - 1);
            if (last.type == "fill") _filled.Remove(last.id);
            else _filled.Add(last.id);
            _ops = Mathf.Max(0, _ops - 1);
            OnStateChanged?.Invoke();
        }

        public void Restart()
        {
            _filled.Clear();
            _history.Clear();
            _ops = 0;
            _hintUsed = false;
            OnStateChanged?.Invoke();
        }

        public void MarkHintUsed() => _hintUsed = true;

        private void EvaluateClear()
        {
            int stars = _ops <= optimalOps ? 3 : _ops <= optimalOps + 4 ? 2 : 1;
            if (_hintUsed) stars = Mathf.Min(stars, 2);
            int wood = stars * 10;
            OnCleared?.Invoke(stars, _ops, wood);
        }
    }
}