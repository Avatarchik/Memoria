using UnityEngine;
using System.Collections.Generic;
using System;

public class ActorSpawner : MonoBehaviour {

    public bool Init { get; set;}

    public Transform parentObject;
    
    public GameObject Spawn<T> (string profile, string resource)
    {
        var spawnObj = (GameObject)Resources.Load(resource);
        var obj = Instantiate(spawnObj);
        var _profile = Type.GetType(profile);
        
        obj.AddComponent(typeof(T));
        obj.AddComponent(_profile);
        obj.AddComponent<BoxCollider2D>();
        obj.GetComponent<BoxCollider2D>().enabled = false;
                        
        return obj;
    }

    public void InitObj(GameObject obj, List<string> components, Transform parent)
    {
        for(int i = 0; i < components.Count; i++)
        {
            var c = Type.GetType(components[i]);
            obj.AddComponent(c);
        }
        obj.transform.SetParent(parent, false);
    }

    public string[] GetRandomEnemies()
    {
        string[] result = {"Golem"};
        return result;
    }

    
    public Dictionary<string, string> GetProfiles(string[] partyMembers)
    {
        var result = new Dictionary<string, string>();
        for(int i = 0; i < partyMembers.Length; i++)
        {
            result.Add(partyMembers[i], "GOJSBA100" + (i + 3));
        }
        return result;
    }
}