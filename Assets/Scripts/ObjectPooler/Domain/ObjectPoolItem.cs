using UnityEngine;

namespace ObjectPooler.Domain
{
    /**
     * Here are stored information about pool item.
     * poolItemName need to unique (like: building_house_5, animal_domestic_2)
     * prefab shows which GameObject should be displayed when ObjectPooler.Application.ObjectPoolerManager.SpawnFromPool is called
     * parent is a reference to transform inside whom GameObject will be spawn
     * size: how many GameObjects we want to spawn on game starts
     * autoResize: if there is more need that available free GameObjects: spawn more and add to Pool queue
     */
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