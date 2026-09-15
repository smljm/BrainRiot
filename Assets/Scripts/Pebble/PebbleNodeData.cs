using System;
using UnityEngine;

namespace BrainRiot.Pebble
{
    // 트리의 원(circle) 하나에 대한 데이터. HTML 프로토타입의 nodeDefs와 1:1 대응.
    [Serializable]
    public class PebbleNodeData
    {
        public string id;
        public int value;
        public string[] children;
        public Vector2 anchoredPosition; // 트리 영역 안에서의 좌표 (px 기준)
    }
}
