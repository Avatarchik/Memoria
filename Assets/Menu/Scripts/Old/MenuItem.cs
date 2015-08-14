using UnityEngine;
using System.Collections;

namespace Menu
{
	public class MenuItem : MonoBehaviour
	{
		// このオブジェクトが Menu のどこにあるのか
		// 0: Tower 直下
		// 1: Camp 直下
		// 2: Labo 直下
		public int menuCode = 0;

		public Animator animator { get; set; }

		void Awake()
		{
			animator = GetComponent<Animator>();
			MenuTransition transition = GameObject.FindObjectOfType<MenuTransition>();
			transition.onTransitionEvent += HandleOnTransitionEvent;
		}

		// Use this for initialization
		void Start()
		{
		}

		void HandleOnTransitionEvent(object sender, ButtonEventArgs e)
		{
			if (e.nextMenu == menuCode)
			{
				animator.SetTrigger("onTransitionTrigger");
			}
		}
	
		// Update is called once per frame
		void Update()
		{	
		}
	}
}