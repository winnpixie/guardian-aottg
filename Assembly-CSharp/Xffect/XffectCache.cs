using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XffectCache : MonoBehaviour
{
    private Dictionary<string, ArrayList> ObjectDic = new Dictionary<string, ArrayList>();

    private void Awake()
    {
        foreach (Transform item in base.transform)
        {
            ObjectDic[item.name] = new ArrayList();
            ObjectDic[item.name].Add(item);
            Xffect component = item.GetComponent<Xffect>();
            if (component != null)
            {
                component.Initialize();
            }
            item.gameObject.SetActive(false);
        }
    }

    protected Transform AddObject(string name)
    {
        Transform transform = base.transform.Find(name);
        if (transform == null)
        {
            Debug.Log("object:" + name + "doesn't exist!");
            return null;
        }
        Transform transform2 = Object.Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
        ObjectDic[name].Add(transform2);
        transform2.gameObject.SetActive(false);
        Xffect component = transform2.GetComponent<Xffect>();
        if (component != null)
        {
            component.Initialize();
        }
        return transform2;
    }

    public ArrayList GetObjectCache(string name)
    {
        ArrayList arrayList = ObjectDic[name];
        if (arrayList == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        return arrayList;
    }

    public Transform GetObject(string name)
    {
        ArrayList arrayList = ObjectDic[name];
        if (arrayList == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        foreach (Transform item in arrayList)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return AddObject(name);
    }
}
