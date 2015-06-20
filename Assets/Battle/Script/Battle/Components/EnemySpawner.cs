using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour {
    public static bool Init;
    private GameObject[] _enemy = new GameObject[2];
    System.Random random;
    public static List<GameObject> enemyObjs = new List<GameObject> ();

    public string[] enemies;
    
    // Use this for initialization
    void Start () {

        enemies = GetRandomEnemies();
        
        random = new System.Random ();
        for (int i = 0; i <= _enemy.Length - 1; i++) {
            _enemy[i] = (GameObject)Resources.Load("enemy" + i);
        }
        int eCnt = random.Next (1, 1); //Set number of Enemies that can spawn
        for (int i = 0; i < eCnt; i++) {
            var enemy = Type.GetType(enemies[i]);
            var pos = new Vector3((eCnt / 1.5f - eCnt + i) * 2, 1.5f, -9);
            var obj = Instantiate(_enemy[random.Next(0, _enemy.Length)], pos, Quaternion.identity) as GameObject; 
            obj.AddComponent<HealthSystem>();
            obj.AddComponent<DeathSystem>();

            obj.GetComponent<HealthSystem>().maxHp = 150 * (i + 1);
            obj.GetComponent<HealthSystem>().hp = obj.GetComponent<HealthSystem>().maxHp;

            obj.GetComponent<DeathSystem>().isAlive = true;
            obj.AddComponent(enemy);
            
            obj.AddComponent<Enemy>();
            obj.GetComponent<Enemy>().battleID = "e0" + i;

            obj.AddComponent<BoxCollider2D>();

            obj.transform.parent = this.transform;

            enemyObjs.Add(obj);
           BattleMgr.actorList.Add(obj);
        }
        Init = true;;

    }
    public string[] GetRandomEnemies()
    {
        string[] result = {"Golem"};
        return result;
    }
}
