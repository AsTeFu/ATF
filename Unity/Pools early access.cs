using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPool {
    public GameObject obj;
    public int count;
    [ReadOnly] public int indexPool;
    [ReadOnly] public int indexObject = -1;
    [ReadOnly] public Vector3[] posInPool;
}

public class Pools : MonoBehaviour {
    
    [SerializeField] private static GameObject[][] poolObjects = new GameObject[0][];
    private static Transform poolTransform;

    private void Awake() {
        poolTransform = transform;
    }
    

    public static void AddInPool(ref DataPool data)  {
        GameObject[][] tmp = poolObjects;
        poolObjects = new GameObject[poolObjects.Length + 1][];
        
        for (int i = 0; i < tmp.Length; i++) {
            poolObjects[i] = tmp[i];
        }

        poolObjects[poolObjects.Length - 1] = new GameObject[data.count];

        GameObject parent = new GameObject(data.obj.name);
        parent.transform.SetParent(poolTransform);
        data.posInPool = new Vector3[data.count];

        for (int i = 0; i < data.count; i++) {
            data.posInPool[i] = poolTransform.position + new Vector3(i, poolObjects.Length);
            poolObjects[poolObjects.Length - 1][i] = Instantiate(data.obj, poolTransform.position + new Vector3(i, poolObjects.Length), Quaternion.identity, poolTransform);
            poolObjects[poolObjects.Length - 1][i].transform.SetParent(parent.transform);
            poolObjects[poolObjects.Length - 1][i].name = data.obj.name + " " + i;
            
        }

        data.indexObject = 0;
        data.indexPool = poolObjects.Length - 1;

    }
    public static GameObject GetObject(ref DataPool data) {
        if (data.indexPool == -1) return null;
        
        int tmp = data.indexObject;
        
        data.indexObject++;
        if (data.indexObject == data.count)
            data.indexObject = 0;

        return poolObjects[data.indexPool][tmp];
    }
    public static void ReturnToPool(GameObject obj) {
        
    }

    public static T[] GetComponents<T>(DataPool data) {
        T[] array = new T[data.count];
        for (int i = 0; i < data.count; i++) {
            array[i] = GetObject(ref data).GetComponent<T>();
        }

        return array;
    }
    
    //public static void RemoveFromPool(DataPool data) {
    //    GameObject[][] tmp = poolObjects;
    //    poolObjects = new GameObject[poolObjects.Length - 1][];

    //    int indexDelete = data.indexPool;
    //    for (int i = 0; i < poolObjects.Length;) {
    //        if (indexDelete != i) {

    //        }
    //    }
    //}
}
