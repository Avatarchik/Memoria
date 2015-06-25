using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Memoria.Battle.Managers;

namespace Memoria.Battle.Utility
{
    public class DeathSystem : MonoBehaviour
    {
        public bool isAlive;
        public static List<GameObject> deadEnemy = new List<GameObject>();

        //Initialize
        void Start()
        {

        }
        //Graphic Updates
        void Update()
        {
            foreach (GameObject enemy in BattleMgr.Instance.enemyList) {
                if(!enemy.GetComponent<DeathSystem>().isAlive) {
                    BattleMgr.actorList.Remove(enemy);
                    deadEnemy.Add(enemy);
                }  
            }
            foreach (GameObject obj in deadEnemy) {
                BattleMgr.Instance.enemyList.Remove (obj);
                Destroy (obj);
            }
        }

        public void Die()
        {
            isAlive = false;
        }
    
        public IEnumerator DeadEffect()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer> ();
            float interval = 1;
            float time = 0;

            yield return new WaitForSeconds (0.5f);

            while (time < interval) {
                Color color = sr.color;
                color.a = Mathf.Lerp(1, 0, time / interval);
                sr.color = color;
                time += Time.deltaTime;
                yield return null;
            }        
            Die();
        }
    }
}