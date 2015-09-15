using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.Menu
{
    public class MapViewer : MonoBehaviour
    {
        public float speed = 1;

        [SerializeField]
        private MenuButton menuButton;

        [SerializeField]
        private Button mapViewButton;

        [SerializeField]
        private Button returnButton;

        [SerializeField]
        private List<GameObject> setActiveObjects;

        private DungeonManager dungeonManager;
        private Vector3 basePosition;
        private List<GameObject> activeObjects = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            dungeonManager = DungeonManager.instance;
            basePosition = transform.localPosition;

            var dragObservable = Observable.Create<Unit>(observer =>
                this.UpdateAsObservable()
                    .Where(_ => Input.GetMouseButton(0))
                    .Subscribe(observer.OnNext))
                .Select(_ => Input.mousePosition);

            IDisposable drag = null;

            mapViewButton.OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
                .Do(_ => EnterMapViewer())
                .Do(_ =>
                {
                    drag = dragObservable
                        .Zip(dragObservable.Skip(1), (p1, p2) => p1 - p2)
                        .TakeUntil(this.UpdateAsObservable()
                            .Where(__ => Input.GetMouseButtonUp(0)))
                        .Repeat()
                        .Subscribe(input => 
						{
							var pos = transform.position + speed * input;
							var canMoveArea = MapManager.instance.stageArea;
							
							pos.x = Mathf.Clamp(pos.x, canMoveArea.xMin, canMoveArea.xMax);
							pos.y = Mathf.Clamp(pos.y, canMoveArea.yMin, canMoveArea.yMax);
							transform.position = pos;
						});
                })
                .SelectMany(_ => returnButton.OnClickAsObservable().First())
                .Do(_ => ExitMapViewer())
                .Subscribe(_ => drag.Dispose());

            returnButton.gameObject.SetActive(false);
        }

        private void EnterMapViewer()
        {
            DungeonManager.instance.EnterState(DungeonState.MapViewer);
            activeObjects = setActiveObjects.Where(g => g.activeSelf).ToList();
            activeObjects.ForEach(g => g.SetActive(false));
            returnButton.gameObject.SetActive(true);
        }

        private void ExitMapViewer()
        {
            transform.localPosition = basePosition;
            activeObjects.ForEach(g => g.SetActive(true));
            returnButton.gameObject.SetActive(false);
            menuButton.SetUIActive(false);
            dungeonManager.ExitState();
        }

        private Vector2 GetInput()
        {
            return new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        }
    }
}