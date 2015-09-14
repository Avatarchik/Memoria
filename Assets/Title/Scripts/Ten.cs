using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Memoria.Title
{
    public class Ten : MonoBehaviour
    {
        Image ima;
        Animator animator;
        // Use this for initialization
        void Start()
        {
            ima = GetComponent<Image>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Te()
        {
            //  Color ca;
            //  ca = ima.color;
            //  ca.a = 0;
            //  ima.color = ca;
            //  StartCoroutine 	(tenn ());
            animator.SetTrigger("tf");

        }

        private IEnumerator tenn()
        {
            //Color ca;
            /*		Debug.Log ("iii");
                    yield return new WaitForSeconds (0.1f);
                    Debug.Log ("iii");
                    yield return new WaitForSeconds (0.1f);
                    Debug.Log ("iii");
                    yield return new WaitForSeconds (0.1f);
                    Debug.Log ("iii");
                    yield return new WaitForSeconds (0.1f);
                    Debug.Log ("iii");
                    */
            /*
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            ca = ima.color;
            ca.a = 0;
            ima.color = ca;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            ca = ima.color;
            ca.a = 255;
            ima.color = ca;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            ca = ima.color;
            ca.a = 0;
            ima.color = ca;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            ca = ima.color;
            ca.a = 255;
            ima.color = ca;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            ca = ima.color;
            ca.a = 0;
            ima.color = ca;
        */
            yield break;

        }
    }
}