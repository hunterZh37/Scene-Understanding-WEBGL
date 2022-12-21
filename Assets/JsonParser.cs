using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class JsonParser : MonoBehaviour
{

    [System.Serializable]
    public class SaveLoadRoot
    {
        public string objName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public List<SaveLoadParent> parentList;
    }

    [System.Serializable]
    public class SaveLoadParent
    {
        public string objName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public SaveLoadChild child;
    }
    [System.Serializable]
    public class SaveLoadChild
    {
        public string objName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public SaveLoadShape shape;
    }
    [System.Serializable]
    public class SaveLoadShape
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public GameObject rootGameObject;
    public string json;
    public Material material;

    // Start is called before the first frame update

    void GetData() => StartCoroutine(GetData_Coroutine());

    IEnumerator GetData_Coroutine()
    {
        
        string uri = "https://pages.cs.wisc.edu/~hunterz/GameObjectData.json";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                json = request.error;
            else
               json = request.downloadHandler.text;
            SaveLoadRoot root = JsonUtility.FromJson<SaveLoadRoot>(json);
            GameObject rootGameObject = new GameObject(root.objName);
            rootGameObject.transform.position = root.position;
            rootGameObject.transform.rotation = root.rotation;
            rootGameObject.transform.localScale = root.scale;
            foreach (SaveLoadParent parent in root.parentList)
            {
                GameObject gameObjectParent = new GameObject(parent.objName);
                gameObjectParent.transform.SetParent(rootGameObject.transform);
                gameObjectParent.transform.localPosition = parent.position;
                gameObjectParent.transform.localRotation = parent.rotation;
                gameObjectParent.transform.localScale = parent.scale;



                GameObject gameObjectChild = new GameObject(parent.child.objName);
                gameObjectChild.transform.SetParent(gameObjectParent.transform);
                gameObjectChild.transform.localPosition = parent.child.position;
                gameObjectChild.transform.localRotation = parent.child.rotation;
                gameObjectChild.transform.localScale = parent.child.scale;



                GameObject gameObjectShape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObjectShape.GetComponent<BoxCollider>().enabled = false;
                gameObjectShape.GetComponent<MeshRenderer>().material = material;
               

                UnityEngine.Object.Destroy(gameObjectShape.GetComponent<BoxCollider>());

                gameObjectShape.transform.SetParent(gameObjectChild.transform);
                gameObjectShape.transform.localPosition = parent.child.shape.position;
                gameObjectShape.transform.localRotation = parent.child.shape.rotation;
                gameObjectShape.transform.localScale = parent.child.shape.scale;
               






            }
        }
    }
    void Start()
    {

        GetData();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
