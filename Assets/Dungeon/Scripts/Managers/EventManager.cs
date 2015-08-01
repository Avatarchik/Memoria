using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.BlockEvents;
using UniRx;

namespace Memoria.Dungeon.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance { get { return DungeonManager.instance.eventManager; } }

        private MapManager mapManager;
        private Player player;

        [SerializeField]
        private Animator eventAnimator;

        [SerializeField]
        private GameObject messageBox;

        private Text messageBoxText;

        public string message
        {
            get { return messageBoxText.text; }
            set { messageBoxText.text = value; }
        }

        private ItemTakeEvent itemTakeEvent;
        private BattleEvent battleEvent;
        private PowerTakeEvent powerTakeEvent;
        private SpRemainCheckEvent spRemainCheckEvent;

        private Subject<Unit> onEndBlockEvent;

        public IObservable<Unit> OnEndBlockEventAsObservable()
        {
            return onEndBlockEvent ?? (onEndBlockEvent = new Subject<Unit>());
        }

        private void OnEndBlockEvent()
        {
            if (onEndBlockEvent != null)
            {
                onEndBlockEvent.OnNext(Unit.Default);
            }
        }

        void Awake()
        {
            mapManager = MapManager.instance;
            player = DungeonManager.instance.player;

            player.OnWalkEndAsObservable()
                .Subscribe(_ => OnArrivePlayer());

            messageBoxText = messageBox.GetComponentInChildren<Text>();
            messageBox.SetActive(false);

            itemTakeEvent = new ItemTakeEvent(this, eventAnimator);
            battleEvent = new BattleEvent(this, eventAnimator);
            powerTakeEvent = new PowerTakeEvent(this, eventAnimator);
            spRemainCheckEvent = new SpRemainCheckEvent(this, eventAnimator);
        }

        // プレイヤーが歩き終わったときに呼び出される
        public void OnArrivePlayer()
        {
            // プレイヤーがいるブロックを取得			
            Block block = mapManager.GetBlock(player.location);
            if (block.blockType == BlockType.None)
            {
                return;
            }
            
            StartCoroutine(CoroutineBlockEvent(block));
        }
        
        private IEnumerator CoroutineBlockEvent(Block block)
        {
            DungeonManager.instance.EnterState(DungeonState.BlockEvent);
            
            yield return itemTakeEvent.StartTakeItemCoroutine(player.location);
            yield return battleEvent.StartBattleEventCoroutine(block, itemTakeEvent.taked);
            
            if (battleEvent.onBattleEvent)
            {
                yield break;
            }
            
            yield return powerTakeEvent.StartTakePowerCoroutine(block);
            yield return spRemainCheckEvent.StartCheckSpRemainCoroutine();
            
            OnEndBlockEvent();
            DungeonManager.instance.ExitState();
            yield break;   
        }        

        public void ReturnFromBattle()
        {
            Block block = mapManager.GetBlock(player.location);
            
            StartCoroutine(CoroutineReturnFromBattle(block));
        }
        
        private IEnumerator CoroutineReturnFromBattle(Block block)
        {
            yield return new WaitForSeconds(1);
            yield return powerTakeEvent.StartTakePowerCoroutine(block);
            yield return spRemainCheckEvent.StartCheckSpRemainCoroutine();
        }

        public void ShowMessageBox(bool visible)
        {
            messageBox.SetActive(visible);
        }
    }
}