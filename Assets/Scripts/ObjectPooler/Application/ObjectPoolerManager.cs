using System.Collections.Generic;
using ObjectPooler.Domain;
using UnityEngine;

namespace ObjectPooler.Application
{
    /**
     * In this place we configure Object Pool Items (see more at ObjectPooler.Domain.ObjectPoolItem)
     * For convenience: items are splitted into groups. Later on items from groups are combined into one list (_poolDictionary))
     */
    public class ObjectPoolerManager : MonoBehaviour
    {
        //this is divided by groups for more human-friendly management in the Inspector
        public List<ObjectPoolItem> buildings;
        public List<ObjectPoolItem> people;
        
        //this is needed to run loop over grouped items
        private List<List<ObjectPoolItem>> _allObjects = new List<List<ObjectPoolItem>>();
        
        //here we store all GameObject for Pool Item with specific name.
        //For example: in this place all GameObjects for "building_Large" will stored.
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        
        //list of all unique pools
        private Dictionary<string, ObjectPoolItem> _objectsPoolItems = new Dictionary<string, ObjectPoolItem>();
        
        //fast check if Pool is resizable, for example:
        //"building_Large" - true
        //"arrows" - false
        //this prevents for looping all the pools everytime
        private Dictionary<string, bool> _autoResize = new Dictionary<string, bool>();

        public void Start()
        {
            _allObjects.Add(buildings);
            _allObjects.Add(people);

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            //create that many GameObjects as there is in ObjectPooler.Domain.ObjectPoolItem.size
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

        /**
         * Spawns GameObject from Pool.
         * If there are no free GameObjects and resize option is disabled: display warning
         */
        public GameObject SpawnFromPool(string poolItemName, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(poolItemName))
            {
                Debug.LogWarningFormat("Poll with name: {0} doesn't exist", poolItemName);
                return null;
            }

            //there are no more free GameObjects in the pool queue
            if (_poolDictionary[poolItemName].Count == 0)
            {
                _autoResize.TryGetValue(poolItemName, out var autoresize);
                _objectsPoolItems.TryGetValue(poolItemName, out ObjectPoolItem objectPoolItem);

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
            
            //get free GameObject from Pool queue of GameObjects
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

        //We don't need this GameObject anymore. Set it at 0,0,0 position.
        public void ReleaseBackToPool(string poolItemName, GameObject obj)
        {
            obj.transform.position = Vector3.zero;
            _poolDictionary[poolItemName].Enqueue(obj);
        }

        //create GameObject and ands it into Pool queue
        private void SpawnGO(GameObject prefab, Transform parent, Queue<GameObject> objectPool)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            objectPool.Enqueue(obj);
        }
    }
}