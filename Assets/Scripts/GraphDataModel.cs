using UnityEngine;

namespace Prisms.Assignment
{
    [System.Serializable]
    public struct GraphDataModel //Eventually we could turn this into a Scriptable Asset
    {
        [SerializeField]
        public GraphDataElement[] data;

        [System.Serializable]
        public struct GraphDataElement
        {
            public int index;
            public int value;
        }
    }
}