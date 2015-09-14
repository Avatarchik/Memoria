using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Managers;

namespace Memoria.Title
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private Image board;
        //	private AudioSource se;

        // Use this for initialization
        void Start()
        {
            board.gameObject.SetActive(false);
            //		se = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        //void Update()
        //{

        //}

        public void LoadLevel(string level)
        {
            board.gameObject.SetActive(true);
            StartCoroutine(CoroutineLoadLevel(level));
            //		se.PlayOneShot (se.clip);
        }

        private IEnumerator CoroutineLoadLevel(string level)
        {
            Color from = board.color;
            Color to = Color.white;
            float time = 1f;
            float elapsed = 0;
            //	se.PlayOneShot (se.clip);
            SoundManager.instance.PlaySound(33);

            while (elapsed <= time)
            {
                elapsed += Time.deltaTime;
                board.color = Color.Lerp(from, to, elapsed / time);

                yield return null;
            }

            Application.LoadLevel(level);
            yield break;
        }
    }
}