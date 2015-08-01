using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Memoria.Dungeon.Items;
using Memoria.Dungeon.BlockComponent;
using Memoria.Battle.GameActors;

namespace Memoria.Dungeon.Managers
{
    public class ParameterManager : MonoBehaviour
    {
        public static ParameterManager instance { get { return DungeonManager.instance.parameterManager; } }

        private static Dictionary<BlockType, int> toIndex =
            new Dictionary<BlockType, int>()
            {
                { BlockType.Thunder, 0 },
                { BlockType.Wind, 1 },
                { BlockType.Water, 2 },
                { BlockType.Fire, 3 },
            };

        private ReactiveProperty<DungeonParameter> _parameter = new ReactiveProperty<DungeonParameter>();

        public DungeonParameter parameter
        {
            get { return _parameter.Value; }
            set { _parameter.Value = value; }
        }

        public IObservable<DungeonParameter> OnChangeParameterAsObservable()
        {
            return _parameter.AsObservable();
        }

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text spText;

        [SerializeField]
        private Text keyText;

        [SerializeField]
        private Text sillingText;

        [SerializeField]
        private ElementalPowerStock[] charactersPowerStocks = new ElementalPowerStock[4];

        public void Awake()
        {
            SubscribeParameterChangeEvent();
            SubscribePlayerWalkEvent();
            SubscribeBlockOpertateEvent();
            SubscribeItemEvent();
        }

        private void SubscribeParameterChangeEvent()
        {
            var parameterChanged = OnChangeParameterAsObservable();

            // HPの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.hp)
                .Subscribe(UpdateHpText);

            parameterChanged
                .DistinctUntilChanged(param => param.maxHp)
                .Subscribe(UpdateHpText);

            // SPの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.sp)
                .Subscribe(UpdateSpText);

            parameterChanged
                .DistinctUntilChanged(param => param.maxSp)
                .Subscribe(UpdateSpText);

            // Keyの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.getKeyNum)
                .Subscribe(UpdateKeyText);

            parameterChanged
                .DistinctUntilChanged(param => param.allKeyNum)
                .Subscribe(UpdateKeyText);

            // Sillingの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.silling)
                .Subscribe(UpdateSillingText);

            // ストックの変化イベントの追加
            parameterChanged
                .Where(param => param.stocks != null)
                .DistinctUntilChanged(param => param.stocks)
                .Subscribe(UpdatePowerStock);
        }

        private void SubscribePlayerWalkEvent()
        {
            // プレイヤーが歩き終わったあと
            DungeonManager.instance.player.OnWalkEndAsObservable()
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 1;
                    parameter = param;
                });
        }

        private void SubscribeBlockOpertateEvent()
        {
            var blockManager = BlockManager.instance;

            // ブロックの破壊時
            blockManager.OnCreateBlockAsObservable()
                .SelectMany(block => block.OnBreakAsObservable())
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 2;
                    parameter = param;
                });

            // ブロックのリセット時
            blockManager.OnRandomizeAsObservable()
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 2;
                    parameter = param;
                });
        }

        private void SubscribeItemEvent()
        {
            var createItem = ItemManager.instance.OnCreateItemAsObservable();

            // キーを取得した時
            createItem
                .Where(item => item.itemData.type == ItemType.Key)
                .SelectMany(item => item.OnTakeAsObservable())
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.getKeyNum++;
                    parameter = param;
                });

            // 宝石を取得した時
            createItem
                .Where(item => item.itemData.type == ItemType.Jewel)
                .SelectMany(item => item.OnTakeAsObservable().Select(_ => item.itemData.attribute))
                .Subscribe(attribute =>
                {
                    TakePowerStock(attribute);

                    var param = parameter;
                    param.silling += 1000;
                    parameter = param;
                });

            // 魂を取得したとき
            createItem
                .Where(item => item.itemData.type == ItemType.Soul)
                .SelectMany(item => item.OnTakeAsObservable().Select(_ => item.itemData.attribute))
                .Subscribe(FillPowerStock);
        }

        public void TakePowerStock(BlockType attribute)
        {
            int index = toIndex[attribute];
            int nowCount = parameter.stocks[index];
            int fillCount = 3;
            int nextCount = Mathf.Min(nowCount + 1, fillCount);
            parameter.stocks[index] = nextCount;
            charactersPowerStocks[index].stock = nextCount;
        }

        public void FillPowerStock(BlockType attribute)
        {
            int index = toIndex[attribute];
            int fillCount = 3;
            parameter.stocks[index] = fillCount;
            charactersPowerStocks[index].stock = fillCount;
        }

        private void UpdateHpText(DungeonParameter parameter)
        {
            hpText.text = string.Format("{0:000}/{1:000}", parameter.hp, parameter.maxHp);
        }

        private void UpdateSpText(DungeonParameter parameter)
        {
            spText.text = string.Format("{0:000}/{1:000}", parameter.sp, parameter.maxSp);
        }

        private void UpdateKeyText(DungeonParameter parameter)
        {
            keyText.text = string.Format("{0}/{1}", parameter.getKeyNum, parameter.allKeyNum);
        }

        private void UpdateSillingText(DungeonParameter parameter)
        {
            sillingText.text = string.Format("{0:0000}", parameter.silling);
        }

        private void UpdatePowerStock(DungeonParameter parameter)
        {
            Enumerable.Range(0, 4)
                .ToList()
                .ForEach(index =>
                {
                    charactersPowerStocks[index].stock = parameter.stocks[index];
                });
        }
    }
}