using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
    public class Cloud : MonoBehaviour
    {        
        private enum CloudPosition
        {
            Top,
            Bottom,
        }
        
        [SerializeField]
        private CloudPosition cloudPosition;
        
        [SerializeField]
        private float space;
        
        [SerializeField]
        private float offset;

        // Use this for initialization
        void Start()
        {
            Vector3 position = transform.position;
            
            switch (cloudPosition)
            {
                case CloudPosition.Top:
                    position.y = MapManager.instance.stageArea.yMax * space + offset;
                    break;
                    
                case CloudPosition.Bottom:
                    position.y = MapManager.instance.stageArea.yMin * space - offset;
                    break;
            }
            
            transform.position = position;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}