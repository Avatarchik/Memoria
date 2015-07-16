using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;

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

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text spText;

        [SerializeField]
        private Text keyText;

        public void Awake()
        {
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

            // プレイヤーが歩き終わったあと
            DungeonManager.instance.player.OnWalkEndAsObservable()
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 1;
                    parameter = param;
                });

            // ブロックの破壊時
            BlockManager.instance.OnCreateBlockAsObservable()
                .SelectMany(block => block.OnBreakAsObservable())
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 2;
                    parameter = param;
                });

            // ブロックのリセット時
            BlockManager.instance.OnRandomizeAsObservable()
                .Subscribe(_ =>
                {
                    var param = parameter;
                    param.sp -= 2;
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
    }
}