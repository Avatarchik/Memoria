using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour {
    public bool Init { get; set; }
    
    public GameObject SpawnEnemy (string enemy, string resource)
    {
        var enemyObj = (GameObject)Resources.Load(resource);
        GameObject obj = Instantiate(enemyObj, new Vector3(0,0,0), Quaternion.identity) as GameObject;

        var enemyAI = Type.GetType(enemy);
        
        obj.AddComponent<HealthSystem>();
        obj.AddComponent<DeathSystem>();
        
        obj.GetComponent<HealthSystem>().maxHp = 150;
        obj.GetComponent<HealthSystem>().hp = obj.GetComponent<HealthSystem>().maxHp;
        
        obj.GetComponent<DeathSystem>().isAlive = true;
        obj.AddComponent(enemyAI);
        
        obj.AddComponent<Enemy>();
        obj.AddComponent<BoxCollider2D>();
        obj.GetComponent<BoxCollider2D>().enabled = false;
                    
        return obj;
    }
}

//        obj.transform.parent = transform;
