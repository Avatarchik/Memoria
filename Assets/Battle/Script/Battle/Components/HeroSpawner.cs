using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HeroSpawner : MonoBehaviour {
    public static bool Init;
    private GameObject[] heroes = new GameObject[4];
    
    List<GameObject> herosObj = new List<GameObject> (); //Might change to obtaining information from another compontent
    string[] _profiles = { "Amelia", "Claude", "Tracy", "Aria" }; //Temporary hero list

        // Use this for initialization
        void Start () {
        for (int i = 0; i < heroes.Length; i++) {
            //heroes[i] = (GameObject)Resources.Load("hero" + i);
            heroes[i] = (GameObject)Resources.Load("GOJSBA100"+ (i + 3));
            GameObject obj = Instantiate(heroes[i],new Vector3((3.8f / 1.5f - 4f + i) * 3.5f, -3, 1), Quaternion.identity) as GameObject;

            var hero = Type.GetType(_profiles[i]);
            obj.AddComponent<Hero>();
            obj.AddComponent<TargetSelector>();
            obj.AddComponent(hero);
            obj.GetComponent<Hero>().battleID = "h0" + i;
            
            
            obj.transform.parent = this.transform;
            herosObj.Add(obj);
           
            BattleMgr.actorList.Add(obj);
        }
        Init = true;
    }
}
