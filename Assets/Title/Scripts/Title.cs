using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour
{
	[SerializeField]
	private Image board;

	// Use this for initialization
	void Start()
	{
		board.gameObject.SetActive(false);
	}

	// Update is called once per frame
	//void Update()
	//{

	//}

	public void LoadLevel(string level)
	{
		board.gameObject.SetActive(true);
		StartCoroutine(CoroutineLoadLevel(level));
	}

	private IEnumerator CoroutineLoadLevel(string level)
	{
		Color from = board.color;
		Color to = Color.black;
		float time = 1f;
		float elapsed = 0;
		
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
