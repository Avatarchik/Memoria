using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon.BlockEvents
{
    public class ItemTakeEvent
    {
        private static Dictionary<BlockType, float> toValue = new Dictionary<BlockType, float>()
        {
            { BlockType.Thunder, 0 },
            { BlockType.Water, 1 },
            { BlockType.Fire, 2 },
            { BlockType.Wind, 3 },
        };

        private static MapManager mapManager { get { return MapManager.instance; } }

        private MonoBehaviour coroutineAppended;
        private Animator eventAnimator;

        public ItemTakeEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }

        public IObservable<bool> CreateTakeItemAsObservable(Vector2Int location)
        {
            return Observable.FromCoroutine<bool>(observer => CoroutineTakeItem(observer, location));
        }

        public IEnumerator CoroutineTakeItem(IObserver<bool> observer, Vector2Int location)
        {
            DungeonManager.instance.EnterState(DungeonState.ItemTaking);
            bool exists = mapManager.ExistsItem(location);

            if (exists)
            {                
                Item item = mapManager.GetItem(location);
                
                if (!item.visible)
                {
                    DungeonManager.instance.ExitState();
                    observer.OnNext(false);
                    observer.OnCompleted();
                    yield break;
                }
                
                item.visible = false;

                switch (item.itemData.type)
                {
                    case ItemType.Key:
                        yield return coroutineAppended.StartCoroutine(CoroutineTakeKey());
                        break;

                    case ItemType.Jewel:
                        yield return coroutineAppended.StartCoroutine(CoroutineTakeJewel(item.itemData.attribute));
                        break;

                    case ItemType.Soul:
                        yield return coroutineAppended.StartCoroutine(CoroutineTakeSoul(item.itemData.attribute));
                        break;

                    case ItemType.MagicPlate:
                        yield return coroutineAppended.StartCoroutine(CoroutineTakeMagicPlate(item.itemData.attribute));
                        break;
                }

                item.Take();
                //  mapManager.TakeItem(item);
                yield return new WaitForSeconds(1f);
            }

            DungeonManager.instance.ExitState();
            observer.OnNext(exists);
            observer.OnCompleted();
        }

        private IEnumerator CoroutineTakeKey()
        {
            eventAnimator.SetFloat("itemType", 0);
            eventAnimator.SetTrigger("getKey");
            yield return new WaitForSeconds(1);
        }

        private IEnumerator CoroutineTakeJewel(BlockType attribute)
        {
            eventAnimator.SetFloat("itemType", 1);
            eventAnimator.SetFloat("attribute", toValue[attribute]);
            EventManager.instance.message = "宝石を入手した！！";
            eventAnimator.SetTrigger("getJewel");
            yield return new WaitForSeconds(1.4f);
        }

        private IEnumerator CoroutineTakeSoul(BlockType attribute)
        {
            yield break;
        }

        private IEnumerator CoroutineTakeMagicPlate(BlockType attribute)
        {
            yield break;
        }
    }
}