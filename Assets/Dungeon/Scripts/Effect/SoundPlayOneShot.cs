using UnityEngine;
using System.Collections;
using Memoria.Managers;

namespace Memoria.Dungeon.Effect
{
    public class SoundPlayOneShot : MonoBehaviour
    {
        [SerializeField]
        private int soundIndex;

        // Use this for initialization
        void Start()
        {
            SoundManager.instance.PlaySound(soundIndex);
        }
    }
}