using UnityEngine;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon
{
    public class Cursor : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            var animator = GetComponent<Animator>();

            BlockManager.instance.OnCreateBlockAsObservable()
                .Subscribe(block =>
                {
                    var begin = block.OnMoveBeginAsObservable()
                        .Do(_ => SetPositionAtTapLocation())
                        .Subscribe(_ => animator.SetBool("visible", true));

                    var end = block.OnMoveEndAsObservable()
                        .Subscribe(_ => animator.SetBool("visible", false));

                    var moving = block.OnMoveAsObservable()
                        .Do(_ => animator.SetBool("canPut", block.CanPut()))
                        .Subscribe(_ => SetPositionAtTapLocation());

                    block.OnPutAsObservable()
                        .Subscribe(_ =>
                        {
                            begin.Dispose();
                            end.Dispose();
                            moving.Dispose();
                        });
                });
        }

        private void SetPositionAtTapLocation()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = MapManager.instance.ConvertPosition(touchPosition);
            transform.position = position;
        }
    }
}