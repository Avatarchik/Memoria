using UnityEngine;
using System.Collections.Generic;
//  using System.Collections;

namespace Memoria.Dungeon
{
    public class StageArea : MonoBehaviour
    {
        public List<StageData> stageDatas = new List<StageData>();
        
        // Use this for initialization
        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(stageDatas[0].areaSpritePath);
        }

        // Update is called once per frame
        //  void Update()
        //  {

        //  }
    }
}