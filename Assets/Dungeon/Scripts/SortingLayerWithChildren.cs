using UnityEngine;
using System.Collections;
using System.Linq;

namespace Memoria.Dungeon
{
    public class SortingLayerWithChildren : MonoBehaviour
    {
        [SerializeField]
        private int _orderInLayer = 0;

        public int orderInLayer
        {
            get
            {
                return _orderInLayer;
            }
            set
            {
                _orderInLayer = value;
                GetComponentsInChildren<Renderer>()
                    .ToList()
                    .ForEach(renderer => renderer.sortingOrder = _orderInLayer);
            }
        }

        void Awake()
        {
            orderInLayer = _orderInLayer;
        }

        void OnValidate()
        {
            orderInLayer = _orderInLayer;
        }
    }
}