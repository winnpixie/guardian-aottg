using System.Collections.Generic;
using UnityEngine;

public static class ClothFactory
{
    private static Dictionary<string, List<GameObject>> ClothCache;

    static ClothFactory()
    {
        ClothCache = new Dictionary<string, List<GameObject>>(CostumeHair.FemaleHairs.Length);
    }

    public static GameObject GetHair(GameObject reference, string name, Material material, Color color)
    {
        GameObject gameObject2;
        ParentFollow parentFollow;
        if (ClothCache.TryGetValue(name, out List<GameObject> value))
        {
            for (int i = 0; i < value.Count; i++)
            {
                GameObject gameObject = value[i];
                if (gameObject == null)
                {
                    value.RemoveAt(i);
                    i = Mathf.Max(i - 1, 0);
                    continue;
                }
                ParentFollow component = gameObject.GetComponent<ParentFollow>();
                if (!component.isActiveInScene)
                {
                    component.isActiveInScene = true;
                    gameObject.renderer.material = material;
                    gameObject.renderer.material.color = color;
                    gameObject.GetComponent<Cloth>().enabled = true;
                    gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    gameObject.GetComponent<ParentFollow>().SetParent(reference.transform);
                    ReapplyClothBones(reference, gameObject);
                    return gameObject;
                }
            }
            gameObject2 = GenerateCloth(reference, name);
            gameObject2.renderer.material = material;
            gameObject2.renderer.material.color = color;
            parentFollow = gameObject2.AddComponent<ParentFollow>();
            parentFollow.SetParent(reference.transform);
            value.Add(gameObject2);
            ClothCache[name] = value;
            return gameObject2;
        }
        gameObject2 = GenerateCloth(reference, name);
        gameObject2.renderer.material = material;
        gameObject2.renderer.material.color = color;
        parentFollow = gameObject2.AddComponent<ParentFollow>();
        parentFollow.SetParent(reference.transform);
        value = new List<GameObject>();
        value.Add(gameObject2);
        ClothCache.Add(name, value);
        return gameObject2;
    }

    public static GameObject GetCape(GameObject reference, string name, Material material)
    {
        GameObject gameObject2;
        ParentFollow parentFollow;
        if (ClothCache.TryGetValue(name, out List<GameObject> value))
        {
            for (int i = 0; i < value.Count; i++)
            {
                GameObject gameObject = value[i];
                if (gameObject == null)
                {
                    value.RemoveAt(i);
                    i = Mathf.Max(i - 1, 0);
                    continue;
                }
                ParentFollow component = gameObject.GetComponent<ParentFollow>();
                if (!component.isActiveInScene)
                {
                    component.isActiveInScene = true;
                    gameObject.renderer.material = material;
                    gameObject.GetComponent<Cloth>().enabled = true;
                    gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    gameObject.GetComponent<ParentFollow>().SetParent(reference.transform);
                    ReapplyClothBones(reference, gameObject);
                    return gameObject;
                }
            }
            gameObject2 = GenerateCloth(reference, name);
            gameObject2.renderer.material = material;
            parentFollow = gameObject2.AddComponent<ParentFollow>();
            parentFollow.SetParent(reference.transform);
            value.Add(gameObject2);
            ClothCache[name] = value;
            return gameObject2;
        }
        gameObject2 = GenerateCloth(reference, name);
        gameObject2.renderer.material = material;
        parentFollow = gameObject2.AddComponent<ParentFollow>();
        parentFollow.SetParent(reference.transform);
        value = new List<GameObject>();
        value.Add(gameObject2);
        ClothCache.Add(name, value);
        return gameObject2;
    }

    public static void DisposeObject(GameObject cachedObject)
    {
        if (!(cachedObject != null))
        {
            return;
        }
        ParentFollow component = cachedObject.GetComponent<ParentFollow>();
        if (component != null)
        {
            if (component.isActiveInScene)
            {
                cachedObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                cachedObject.GetComponent<Cloth>().enabled = false;
                component.isActiveInScene = false;
                cachedObject.transform.position = new Vector3(0f, -99999f, 0f);
                cachedObject.GetComponent<ParentFollow>().RemoveParent();
            }
        }
        else
        {
            Object.Destroy(cachedObject);
        }
    }

    public static string GetDebugInfo()
    {
        int num = 0;
        foreach (KeyValuePair<string, List<GameObject>> item in ClothCache)
        {
            num += ClothCache[item.Key].Count;
        }
        int num2 = 0;
        Cloth[] array = Object.FindObjectsOfType<Cloth>();
        foreach (Cloth cloth in array)
        {
            if (cloth.enabled)
            {
                num2++;
            }
        }
        return $"{num} cached cloths, {num2} active cloths, {ClothCache.Keys.Count} types cached";
    }

    private static GameObject GenerateCloth(GameObject go, string res)
    {
        if (go.GetComponent<SkinnedMeshRenderer>() == null)
        {
            go.AddComponent<SkinnedMeshRenderer>();
        }
        SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = component.bones;
        SkinnedMeshRenderer component2 = ((GameObject)Object.Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
        component2.transform.localScale = Vector3.one;
        component2.bones = bones;
        component2.quality = SkinQuality.Bone4;
        return component2.gameObject;
    }

    private static void ReapplyClothBones(GameObject reference, GameObject clothObject)
    {
        SkinnedMeshRenderer component = reference.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer component2 = clothObject.GetComponent<SkinnedMeshRenderer>();
        component2.bones = component.bones;
        component2.transform.localScale = Vector3.one;
    }

    public static void ClearClothCache()
    {
        ClothCache.Clear();
    }
}
