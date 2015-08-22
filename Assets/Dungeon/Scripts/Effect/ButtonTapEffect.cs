using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.Effect
{
    public class ButtonTapEffect : MonoBehaviour
    {
        [SerializeField]
        private Button button;
                 
        private int effectIndex;

        // Use this for initialization
        void Start()
        {
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var effectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    effectPosition.z = 0;
                    EffectManager.instance.InstantiateEffect(effectIndex, effectPosition, 2f);
                });
        }
    }
}