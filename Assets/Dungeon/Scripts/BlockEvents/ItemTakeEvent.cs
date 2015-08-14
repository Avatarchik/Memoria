using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            { BlockType.Recovery, 4 }
        };

        private static DungeonManager dungeonManager { get { return DungeonManager.instance; } }
        private static MapManager mapManager { get { return MapManager.instance; } }

        private MonoBehaviour coroutineAppended;
        private Animator eventAnimator;

        public bool taked { get; private set; }

        public ItemTakeEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }

        public Coroutine StartTakeItemCoroutine(Vector2Int location)
        {
            return coroutineAppended.StartCoroutine(CoroutineTakeItem(location));
        }

        private IEnumerator CoroutineTakeItem(Vector2Int location)
        {
            dungeonManager.EnterState(DungeonState.ItemTaking);
            taked = false;

            bool exists = mapManager.ExistsItem(location);

            if (exists)
            {
                Item item = mapManager.GetItem(location);

                if (!item.visible)
                {
                    dungeonManager.ExitState();
                    yield break;
                }

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
                
                taked = true;
                item.Take();
                yield return new WaitForSeconds(1);
            }

            dungeonManager.ExitState();
            yield break;
        }

        private IEnumerator CoroutineTakeKey()
        {
            eventAnimator.SetFloat("itemType", 0);
            eventAnimator.SetFloat("eventType", 0);
            EventManager.instance.message = "鍵を入手した！！";
            eventAnimator.SetTrigger("getKey");
            yield return new WaitForSeconds(1.4f);
        }

        private IEnumerator CoroutineTakeJewel(BlockType attribute)
        {
            eventAnimator.SetFloat("itemType", 1);
            eventAnimator.SetFloat("eventType", 0);
            eventAnimator.SetFloat("attribute", toValue[attribute]);
            EventManager.instance.message = "宝石を入手した！！";
            eventAnimator.SetTrigger("getJewel");
            yield return new WaitForSeconds(1.4f);
        }

        private IEnumerator CoroutineTakeSoul(BlockType attribute)
        {
            eventAnimator.SetFloat("itemType", 2);
            eventAnimator.SetFloat("eventType", 0);
            eventAnimator.SetFloat("attribute", toValue[attribute]);
            EventManager.instance.message = "魂を入手した！！";
            eventAnimator.SetTrigger("getSoul");
            yield return new WaitForSeconds(1.4f);
        }

        private IEnumerator CoroutineTakeMagicPlate(BlockType attribute)
        {
            eventAnimator.SetFloat("itemType", 3);
            eventAnimator.SetFloat("eventType", 0);
            eventAnimator.SetFloat("attribute", toValue[attribute]);
            EventManager.instance.message = "魔石版を入手した！！";
            eventAnimator.SetTrigger("getMagicPlate");
            yield return new WaitForSeconds(1.4f);
        }
    }
}