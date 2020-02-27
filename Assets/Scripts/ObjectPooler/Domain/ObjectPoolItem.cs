using UnityEngine;

namespace ObjectPooler.Domain
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public string poolItemName;
        public GameObject prefab;
        public Transform parent;
        public int size;
        public bool autoResize;
    }
}