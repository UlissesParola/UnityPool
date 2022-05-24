using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	[System.Serializable]
	public class BaseObject
	{
		public GameObject Prefab;
		public int Quantity;
		public string Name;
	}

    [SerializeField]
    private BaseObject[] BasePoolObjects;

	public int PoolSize {
		get { return BasePoolObjects.Length; }
	}

	private Dictionary<string, Queue<GameObject>> _poolDictionary; 

	void Start () {
		
		_poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (BaseObject pool in BasePoolObjects)
		{
			Queue<GameObject> PoolQueue = new Queue<GameObject>();

			pool.Name = pool.Prefab.name;

			for (int i = 0; i < pool.Quantity; i++)
			{
				GameObject newObject = CreatePoolObject(pool);
				PoolQueue.Enqueue(newObject);
			}
			
			_poolDictionary.Add(pool.Name, PoolQueue);
		}
	}

	public GameObject InstantiatePoolObject(string poolName)
	{
		bool poolIsEmpity = _poolDictionary[poolName].Count == 0;

		if (poolIsEmpity)
		{
			foreach (BaseObject item in BasePoolObjects)
			{
				if (item.Name.Equals(poolName))
                {
                    GameObject newObject = CreatePoolObject(item);
                    _poolDictionary[poolName].Enqueue(newObject);
                }
            }
		}

		GameObject obj = _poolDictionary[poolName].Dequeue();
		obj.SetActive(true);
		return obj;
	}

	public GameObject InstantiatePoolObject(int index)
	{
		return InstantiatePoolObject(BasePoolObjects[index].Name);
	}

	private GameObject CreatePoolObject(BaseObject baseObject)
    {
        GameObject newObject = Instantiate(baseObject.Prefab);
        newObject.SetActive(false);
        newObject.transform.SetParent(this.transform);
        return newObject;
    }

    public void RetainObject(GameObject obj, string poolName)
	{
		if (_poolDictionary.ContainsKey(poolName))
		{
			obj.SetActive(false);
			obj.transform.SetParent(this.transform);
			_poolDictionary[poolName].Enqueue(obj);
		}
	}
}
