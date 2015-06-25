using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
	public class MapViewer : MonoBehaviour
	{
		private DungeonManager dungeonManager;

		public float speed = 1;

		private Vector3 basePosition;
		private Vector3 previewMousePosition;

		// Use this for initialization
		void Start()
		{	
			dungeonManager = DungeonManager.instance;
			basePosition = transform.localPosition;
		}
	
		// Update is called once per frame
		void Update()
		{	
			if (dungeonManager.activeState != DungeonState.MapViewer)
			{
				transform.localPosition = basePosition;            
				return;
			}

			if (Input.GetMouseButtonDown(0))
			{
				previewMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			else if (Input.GetMouseButton(0))
			{
				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 mousePositionDelta = -(mousePosition - previewMousePosition);
				mousePositionDelta.z = 0;

				transform.Translate(mousePositionDelta * speed);
			}
		}
	}
}