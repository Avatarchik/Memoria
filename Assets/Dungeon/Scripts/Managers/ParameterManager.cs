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
        private NumberSprite hpValue;
        [SerializeField]
        private NumberSprite maxHpValue;
        
        [SerializeField]
        private NumberSprite spValue;
        [SerializeField]
        private NumberSprite maxSpValue;
        
        [SerializeField]
        private NumberSprite getKeyValue;
        [SerializeField]
        private NumberSprite allKeyValue;

        [SerializeField]
        private NumberSprite sillingValue;

        [SerializeField]
        private Text hpText;
        [SerializeField]
        private Text maxHpText;

        [SerializeField]
        private Text spText;
        [SerializeField]
        private Text maxSpText;

        [SerializeField]
        private Text getKeyText;
        [SerializeField]
        private Text allKeyText;

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
                .Subscribe(UpdateHpValue);

            parameterChanged
                .DistinctUntilChanged(param => param.maxHp)
                .Subscribe(UpdateHpValue);

            // SPの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.sp)
                .Subscribe(UpdateSpValue);

            parameterChanged
                .DistinctUntilChanged(param => param.maxSp)
                .Subscribe(UpdateSpValue);

            // Keyの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.getKeyNum)
                .Subscribe(UpdateKeyValue);

            parameterChanged
                .DistinctUntilChanged(param => param.allKeyNum)
                .Subscribe(UpdateKeyValue);

            // Sillingの変化イベントの追加
            parameterChanged
                .DistinctUntilChanged(param => param.silling)
                .Subscribe(UpdateSillingValue);

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
            SetPowerStock(attribute, index => parameter.stocks[index] + 1);
        }

        public void FillPowerStock(BlockType attribute)
        {
            int fillCount = 3;
            SetPowerStock(attribute, _ => fillCount);
        }

        private void SetPowerStock(BlockType attribute, System.Func<int, int> getValue)
        {
            StockType stockType = ConvertBlockTypeToStockType(attribute);
            int fillCount = 3;

            charactersPowerStocks
                .Select((e, index) => new { e.elementType, index })
                .Where(param => param.elementType == stockType)
                .Select(param => new
                {
                    index = param.index,
                    nextCount = Mathf.Clamp(getValue(param.index), 0, fillCount)
                })
                .ToList()
                .ForEach(param =>
                {
                    parameter.stocks[param.index] = param.nextCount;
                    charactersPowerStocks[param.index].stock = param.nextCount;
                });
        }

        private StockType ConvertBlockTypeToStockType(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Fire:
                    return StockType.FIRE;

                case BlockType.Wind:
                    return StockType.WIND;

                case BlockType.Water:
                    return StockType.WATER;

                case BlockType.Thunder:
                    return StockType.THUNDER;

                default:
                    throw new UnityException("The BlockType `" + blockType + "` could not convert to StockType.");
            }
        }

        private void UpdateHpValue(DungeonParameter parameter)
        {
            //  hpText.text = string.Format("{0:000}/{1:000}", parameter.hp, parameter.maxHp);
            //  hpText.text = string.Format("{0:000}", parameter.hp);
            //  maxHpText.text = string.Format("{0:000}", parameter.maxHp);
            hpValue.value = parameter.hp;
            maxHpValue.value = parameter.maxHp;
        }

        private void UpdateSpValue(DungeonParameter parameter)
        {
            //  spText.text = string.Format("{0:000}/{1:000}", parameter.sp, parameter.maxSp);
            //  spText.text = string.Format("{0:000}", parameter.sp);
            //  maxSpText.text = string.Format("{0:000}", parameter.maxSp);
            spValue.value = parameter.sp;
            maxSpValue.value = parameter.maxSp;
        }

        private void UpdateKeyValue(DungeonParameter parameter)
        {
            //  getKeyText.text = string.Format("{0}/{1}", parameter.getKeyNum, parameter.allKeyNum);
            getKeyValue.value = parameter.getKeyNum;
            allKeyValue.value = parameter.allKeyNum;
        }

        private void UpdateSillingValue(DungeonParameter parameter)
        {
            //  sillingText.text = string.Format("{0:0000}", parameter.silling);
            sillingValue.value = parameter.silling;
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