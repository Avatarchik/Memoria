using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Menu
{
	public class ButtonEventArgs : EventArgs
	{
		private int _nextMenu;

		public int nextMenu { get { return _nextMenu; } }

		private int _transitionType;

		public int transitionType { get { return _transitionType; } }

		public ButtonEventArgs(int nextMenu, int transitionType)
		{
			_nextMenu = nextMenu;
			_transitionType = transitionType;
		}
	}

	public class MenuTransition : MonoBehaviour
	{
		private enum FadeType
		{
			In,
			Out
		}

		private enum UIMoveType
		{
			In,
			Out
		}

		public Camera mainCamera;

		// TODO : 隠蔽化
		public int nowMenu = 1;
		public float transitionTime = 1.0f;
		[SerializeField]
		// for Debug
	private bool
			_isTransitioning = true;

		public bool isTransitioning
		{
			get { return _isTransitioning; }
			set { _isTransitioning = value; }
		}

		public Animator animator { get; set; }

		public Image image { get; set; }

		public event EventHandler<ButtonEventArgs> onTransitionEvent;

		void Awake()
		{
			animator = GetComponent<Animator>();
			image = GetComponent<Image>();
		}

		// Use this for initialization
		IEnumerator Start()
		{
			yield return StartCoroutine(CoroutineMoveUI(1, UIMoveType.In));
			isTransitioning = false;
		}
	
		// Update is called once per frame
		void Update()
		{	
		}

		public void OnTransition(int nextMenu)
		{
			if (isTransitioning || nextMenu == nowMenu)
			{
				return;
			}

			isTransitioning = true;
			StartCoroutine(CoroutineOnTransition(nextMenu));
		}

		// Called by OnTransition
		IEnumerator CoroutineOnTransition(int nextMenu)
		{
			yield return StartCoroutine(CoroutineMoveUI(nowMenu, UIMoveType.Out));

			// カメラの移動先を取得
			float width = 19.2f;
			Vector3 targetPosition = mainCamera.transform.position;
			targetPosition.x = (nextMenu - 1) * width;
			targetPosition.y = 0;
		
			yield return StartCoroutine(CoroutineFade(targetPosition, transitionTime));
			yield return StartCoroutine(CoroutineMoveUI(nextMenu, UIMoveType.In));

			isTransitioning = false;
			nowMenu = nextMenu;
		}

		// Called by CoroutineOnTransition()
		IEnumerator CoroutineMoveUI(int moveTarget, UIMoveType moveType)
		{
			ButtonEventArgs args = new ButtonEventArgs(moveTarget, (int)moveType);
			onTransitionEvent(this, args);

			float delay = 1f;
			yield return new WaitForSeconds(delay);
		}

		// Called by CoroutineOnTransition()
		IEnumerator CoroutineFade(Vector3 targetPosition, float transitionTime)
		{
			float fadeTime = transitionTime / 3f;
			yield return StartCoroutine(CoroutineFadeInOut(fadeTime, FadeType.In));

			// 間をつくる
			yield return new WaitForSeconds(fadeTime);
			mainCamera.transform.position = targetPosition;

			yield return StartCoroutine(CoroutineFadeInOut(fadeTime, FadeType.Out));
		}

		// Called by CoroutineFade()
		IEnumerator CoroutineFadeInOut(float fadeTime, FadeType fadeType)
		{
			animator.SetTrigger("onFadeTrigger");
			animator.speed = 1 / fadeTime;
			yield return new WaitForSeconds(fadeTime);
		}
	}
}