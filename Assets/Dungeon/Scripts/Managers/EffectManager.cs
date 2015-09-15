using UnityEngine;

namespace Memoria.Dungeon.Managers
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance { get { return DungeonManager.instance.effectManager; } }
        
        [SerializeField]
        private GameObject[] effectPrefabs;
        
        public GameObject InstantiateEffect(int index)
        {
            return Instantiate<GameObject>(effectPrefabs[index]);
        }
        
        public GameObject InstantiateEffect(int index, Vector3 position, float duration)
        {
            var effect = Instantiate(effectPrefabs[index], position, Quaternion.identity) as GameObject;
            Destroy(effect, duration);
            return effect;
        }
    }
}