using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectPoolManager : MonoBehaviour {

#region Public Fields
    public static ObjectPoolManager Instance;
#endregion


#region Private Serializable Fields
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int numberOfPooledObjects = 100;
    [SerializeField] private bool increasePoolIfEmpty = true;
    [SerializeField] private int numberOfExtraObjects = 20;
#endregion


#region Private Fields
    private List<GameObject> listOfObjects = new();
#endregion


#region MonoBehaviour CallBacks

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
    }

    void Start(){
        FillThePool();
    }
#endregion


#region Private Methods
    private void FillThePool(){
        for (int i = 0; i < numberOfPooledObjects; i++)
        {
            var obj = Instantiate(objectToPool, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            listOfObjects.Add(obj);
        }
    }
#endregion


#region Public Methods
    public GameObject GetPooledObject(){
        for (int i = 0; i < listOfObjects.Count; i++)
        {
            if(!listOfObjects[i].activeInHierarchy){
                return listOfObjects[i];
            }
        }

        //if we get to this point, the pool is empty!
        if(increasePoolIfEmpty){
            for (int i = 0; i < numberOfExtraObjects; i++)
            {
                var obj = Instantiate(objectToPool, Vector3.zero, Quaternion.identity, transform);
                obj.SetActive(false);
                listOfObjects.Add(obj);
            }
            return GetPooledObject();
        }

        //We would only get to this point if you chose not to spawn more objects if the pool is empty        
        return null;
    }

    public void ReturnObject(GameObject obj){
        obj.SetActive(false);
    }
#endregion
}