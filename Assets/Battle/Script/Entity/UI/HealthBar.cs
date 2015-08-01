using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Memoria.Battle.GameActors
{
    public class HealthBar : MonoBehaviour {
        private Dictionary<int, Sprite> _healthBarSprites;
        private Image _hpBarSprite;
        private int _precentDivided;
        private MainPlayer _mainPlayer;

        void Awake()
        {
            _hpBarSprite = GetComponent<Image>();
            _healthBarSprites = new Dictionary<int, Sprite>();

            for (int i = 0; i <= 10; i++)
            {
                _healthBarSprites[i] = (i < 10) ?
                    Resources.Load<Sprite>("HealthBar/health0" + i) :
                    Resources.Load<Sprite>("HealthBar/health" + i);
            }
        }

        void Start()
        {
            _mainPlayer = GameObject.FindObjectOfType<MainPlayer>() as MainPlayer;
        }

        void LateUpdate()
        {
            _precentDivided = GetHealthPercent10();

            if(_precentDivided < 0) {
                _precentDivided = 0;
            }

            UpdateHealthBar(_precentDivided);
        }

        public void UpdateHealthBar(int hpPercent)
        {
            _hpBarSprite.sprite = _healthBarSprites[hpPercent];
        }

        public int GetHealthPercent10()
        {
            var _onePercent = _mainPlayer.health.maxHp / 100;
            var _healthPercent = _mainPlayer.health.hp / _onePercent;
            return Mathf.CeilToInt(_healthPercent / 10);
        }
    }
}