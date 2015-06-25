using UnityEngine;
using System.Collections.Generic;
using System;

namespace Memoria.Battle.GameActors
{
    public class ActorSpawner : MonoBehaviour {

        public bool Init { get; set;}

        public Transform parentObject;
    
        public GameObject Spawn<T> (Type profile, string resource)
        {
            var spawnObj = (GameObject)Resources.Load(resource);
            var obj = Instantiate(spawnObj);

            obj.AddComponent(typeof(T));
            obj.AddComponent(profile);
            obj.AddComponent<BoxCollider2D>();
            obj.GetComponent<BoxCollider2D>().enabled = false;
                        
            return obj;
        }

        public void InitObj(GameObject obj, IList<Type> components, Transform parent)
        {
            for(int i = 0; i < components.Count; i++)
            {
                obj.AddComponent(components[i]);
            }
            obj.transform.SetParent(parent, false);
        }

        public Type[] GetRandomEnemies()
        {
            Type[] result = { typeof(Golem) };
            return result;
        }

    
        public Dictionary<Type, string> GetProfiles(Type[] partyMembers)
        {
            var result = new Dictionary<Type, string>();
            for(int i = 0; i < partyMembers.Length; i++)
            {
                result.Add(partyMembers[i], "GOJSBA100" + (i + 3));
            }
            return result;
        }
    }
}