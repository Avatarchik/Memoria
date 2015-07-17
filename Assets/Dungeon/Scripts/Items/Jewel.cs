using UnityEngine;
using System.Collections;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Items
{
    public struct JewelData
    {
        public BlockType blockType { get; set; }
        public Vector2Int location { get; set; }
    }

    public class Jewel : MonoBehaviour
    {
        public JewelData jewelData { get; set; }
        
        // Use this for initialization
        //  void Start()
        //  {
            
        //  }

        // Update is called once per frame
        //  void Update()
        //  {

        //  }
    }
}