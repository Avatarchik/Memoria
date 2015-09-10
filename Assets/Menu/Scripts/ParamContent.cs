using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Memoria.Battle.GameActors;

namespace Memoria.Menu
{
    public class ParamContent : MonoBehaviour
    {
        readonly Dictionary<string, GameObject> _params = new Dictionary<string, GameObject>();

        [SerializeField]
        private GameObject _statusPrefab;

        [SerializeField]
        private GameObject _numberLabel;


        public GameObject numberLabel
        {
            get
            {
                return _numberLabel;
            }
        }

        Profile profile;

        void Start()
        {
            _params.Add("hp", Instantiate(_statusPrefab));
            _params.Add("attack", Instantiate(_statusPrefab));
            _params.Add("defense", Instantiate(_statusPrefab));
            _params.Add("speed", Instantiate(_statusPrefab));

            foreach(var obj in _params)
            {
                obj.Value.transform.SetParent(this.transform, false);
                obj.Value.name = obj.Key;
            }
        }

        void Update()
        {
            if(GetComponent<Profile>())
            {
                _params["hp"].GetComponent<ParamLabel>().GenerateLabel(profile.parameter.hp, new Vector3(180, -137, 0));
                _params["attack"].GetComponent<ParamLabel>().GenerateLabel(profile.parameter.attack, new Vector3(180, -199, 0));
                _params["defense"].GetComponent<ParamLabel>().GenerateLabel(profile.parameter.defense, new Vector3(180, -258, 0));
                _params["speed"].GetComponent<ParamLabel>().GenerateLabel(profile.parameter.speed, new Vector3(180, -318, 0));

                UnloadProfile();
            }
        }

        public void SetProfile(Type profilType)
        {
            this.gameObject.AddComponent(profilType);
            profile = GetComponent<Profile>();
        }

        private void UnloadProfile()
        {        
        
            var skillComponents = GetComponents(typeof(AttackType));
            foreach(var component in skillComponents)
            {
                Destroy(component);
            }
            Destroy(profile);
            profile = null;
        }
    }
}