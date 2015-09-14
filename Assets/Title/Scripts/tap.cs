using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tap : MonoBehaviour
{
	Vector3 mp;
	Vector3 mp2;
	int c = 0;
	Vector2 Position;


	void Start()
	{
		Position = transform.position;
	}

	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			if (c % 2 == 0) {
				mp = Input.mousePosition;
				if (mp.y - mp2.y < 0) {
					if (Position.y < 13) {
						Position.y += 0.05f;
					}
				}
				if (mp.y - mp2.y > 0) {
					if (Position.y > -13) {
						Position.y -= 0.05f;
					}
				}
			}
			if (c % 2 == 1) {
				mp2 = Input.mousePosition;
				if (mp2.y - mp.y < 0) {
					if (Position.y < 13) {
						Position.y += 0.05f;
					}
				}
				if (mp2.y - mp.y > 0) {
					if (Position.y > -13) {
						Position.y -= 0.05f;
					}
				}
			}
		}
//		Vector2 Position = transform.position;
//		if (Input.GetMouseButton (0)) {
//
//			if (Position.y < 13) {
//				Position.y += 0.02f;
//			}
//		}
//		if (Input.GetMouseButton (1)) {
//			if(Position.y > -13)
//			{
//				Position.y -= 0.02f;
//			}
//		}
//		transform.position = Position;
		c = c + 1;
		transform.position = Position;
	}
}
