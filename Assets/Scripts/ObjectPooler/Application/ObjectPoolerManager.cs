using System.Collections.Generic;
using ObjectPooler.Domain;
using UnityEngine;

namespace ObjectPooler.Application
{
    public class ObjectPoolerManager : MonoBehaviour
    {
        //this is divided by groups for more human-friendly management in Inspector
        public List<ObjectPoolItem> buildings;
        public List<ObjectPoolItem> people;
        
        private List<List<ObjectPoolItem>> _allObjects = new List<List<ObjectPoolItem>>();
        
        private List<ObjectPoolItem> _poolItems;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        
        private Dictionary<string, ObjectPoolItem> _objectsPoolItems = new Dictionary<string, ObjectPoolItem>();
        private Dictionary<string, bool> _autoResize = new Dictionary<string, bool>();

        public void Start()
        {
            _allObjects.Add(buildings);
            _allObjects.Add(people);

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (List<ObjectPoolItem> objectPoolItems in _allObjects)
            {
                foreach (ObjectPoolItem poolItem in objectPoolItems)
                {
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int i = 0; i < poolItem.size; i++)
                    {
                        SpawnGO(poolItem.prefab, poolItem.parent, objectPool);
                    }

                    _poolDictionary.Add(poolItem.poolItemName, objectPool);
                    
                    _objectsPoolItems.Add(poolItem.poolItemName, poolItem);
                    _autoResize.Add(poolItem.poolItemName, poolItem.autoResize);
                }
            }
        }

        public GameObject SpawnFromPool(string poolItemName, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(poolItemName))
            {
                Debug.LogWarningFormat("Poll with name: {0} doesn't exist", poolItemName);
                return null;
            }

            if (_poolDictionary[poolItemName].Count == 0)
            {
                bool autoresize;
                ObjectPoolItem objectPoolItem;
                
                _autoResize.TryGetValue(poolItemName, out autoresize);
                _objectsPoolItems.TryGetValue(poolItemName, out objectPoolItem);

                if (objectPoolItem == null)
                {
                    Debug.LogWarningFormat("Poll with name: {0} not exists", poolItemName);
                    return null;
                }
                
                if (autoresize)
                {
                    SpawnGO(objectPoolItem.prefab, objectPoolItem.parent, _poolDictionary[poolItemName]);
                    return SpawnFromPool(poolItemName, position, rotation);
                }
                
                Debug.LogWarningFormat("Poll with name: {0} is empty. Autoresize disabled", poolItemName);
                return null;
            }
            
            GameObject obj = _poolDictionary[poolItemName].Dequeue();
            if (obj == null)
            {
                Debug.LogWarningFormat("Poll with name: {0} returned empty GameObject!", poolItemName);
                return null;
            }
            
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            
            return obj;
        }

        public void ReleaseBackToPool(string poolItemName, GameObject obj)
        {
            obj.transform.position = Vector3.zero;
            //obj.name = poolItemName + "_released";
            _poolDictionary[poolItemName].Enqueue(obj);
        }

        private void SpawnGO(GameObject prefab, Transform parent, Queue<GameObject> objectPool)
        {
            GameObject obj = Object.Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            objectPool.Enqueue(obj);
        }
    }
}