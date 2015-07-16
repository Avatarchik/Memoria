using UnityEngine;
using UnityEngine.UI;

//  using System.Collections;
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

            mapViewButton.OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
                .First()
                .Do(_ => EnterMapViewer())
                .SelectMany(_ => this.UpdateAsObservable())
                .TakeUntil(returnButton.OnClickAsObservable()
                    .Do(_ => ExitMapViewer()))
                .Repeat()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => transform.Translate(speed * GetInput()));
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