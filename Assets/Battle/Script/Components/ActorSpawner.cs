using UnityEngine;
using System.Collections.Generic;
using System;

namespace Memoria.Battle.GameActors
{
    public class ActorSpawner : MonoBehaviour {

        public bool Init { get; set;}

        public Transform parentObject;

        public List<Type> _defaultComponents;

        public List<Component> defaultComponent;

        void Awake ()
        {
            _defaultComponents = new List<Type>();
            defaultComponent = new List<Component>();
            parentObject = this.transform;
        }

        public GameObject Spawn<T> (string resource, params Type[] extraParams)
        {
            var spawnObj = (GameObject)Resources.Load(resource);

            var obj = Instantiate(spawnObj);

            obj.AddComponent(typeof(T));

            foreach(var param in extraParams)
            {
            obj.AddComponent(param);
            }

            foreach(Type t in _defaultComponents)
            {
                obj.AddComponent(t);
            }

            obj.transform.SetParent(parentObject, false);

            return obj;
        }
    }
}