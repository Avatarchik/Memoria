using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using Memoria.Dungeon.Items;

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
        private Text hpText;

        [SerializeField]
        private Text spText;

        [SerializeField]
        private Text keyText;

        [SerializeField]
        private Text sillingText;

        public void Awake()
        {
            var dungeonManager = DungeonManager.instance;
            var blockManager = BlockManager.instance;
            var mapManager = MapManager.instance;
            var itemManager = ItemManager.instance;
            var parameterChanged = _parameter.AsObservable();

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

            // プレイヤーが歩き終わったあと
            dungeonManager.player.OnWalkEndAsObservable()
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 1;
                    parameter = param;
                });

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

            var createItem = itemManager.OnCreateItemAsObservable();

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
                .SelectMany(item => item.OnTakeAsObservable())
                .Subscribe(_ => 
                {
                    var param = parameter;
                    param.silling += 1000;
                    parameter = param; 
                });
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
    }
}