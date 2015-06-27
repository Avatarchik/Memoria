using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public class UIMgr : MonoBehaviour {

        private AttackTracker _attackTracker;
        private Dictionary<string, GameObject>[] _obj;
        private GameObject[] _button;
        private Dictionary<string, GameObject> _cursor;
        private Dictionary<string, GameObject> _nameplate;
        private Dictionary<int, Sprite> _healthBarSprites;
        private int _precentDivided;
        private MainPlayer _mainPlayer;
        private GameObject _hpBar;

        // Use this for initialization
        void Awake () {

            _mainPlayer = GameObject.FindObjectOfType<MainPlayer>() as MainPlayer;
            _obj = new Dictionary<string, GameObject>[3];
            _cursor = new Dictionary<string, GameObject>();
            _healthBarSprites = new Dictionary<int, Sprite>();

            for (int i = 0; i < _obj.Length; i++)
            {
                _obj[i] = new Dictionary<string, GameObject>();
            }
            for (int i = 4000; i <= 4010; i++)
            {
                _healthBarSprites[i - 4000] = Resources.Load<Sprite>("GOJCA" + i);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _precentDivided = GetHealthPercent();
            if(_precentDivided < 0)
                _precentDivided = 0;
            UpdateHealthBar(_precentDivided);
        }
        public void UpdateHealthBar(int hpPercent)
        {
            _hpBar.GetComponent<Image>().sprite = _healthBarSprites[hpPercent];
        }

        public void SetCursor(string owner, GameObject obj, bool enable)
        {
            if(enable)
            {
                Vector3 pos = obj.transform.position;
                pos.y += 2f;
                _cursor.Add(owner, Instantiate ((GameObject)Resources.Load("cursor")) as GameObject);
                _cursor[owner].transform.SetParent(GameObject.FindObjectOfType<Canvas> ().gameObject.transform, false);
                _cursor[owner].transform.position = pos;
            }
            else
            {
                Debug.Log(_cursor.ContainsKey(owner));
                Debug.Log(_cursor.Count);
                if(_cursor.ContainsKey(owner))
                {
                    Debug.Log(_cursor[owner]);
                    Destroy(_cursor[owner]);
                    _cursor.Remove(owner);
                }
            }
        }
        //TODO: Set selection animation based on single/multiple targets
        public void SetCurorAnimation(TargetType t, string s)
        {

        }

        public int GetHealthPercent()
        {
            var _onePercent = _mainPlayer.health.maxHp / 100.0f;
            var _healthPercent = _mainPlayer.health.hp / _onePercent;
            return Mathf.CeilToInt(_healthPercent / 10);
        }

        public void ShowSkill(Hero player)
        {
            var profile = player.GetComponent<Profile>();
            string[] skills = player.GetSkills();
            _obj[2] = GenerateObjDictionary(skills, skills);
            int cnt = 0;
            _button = _button ?? new GameObject[profile.attackList.Count];
            foreach(var skill in profile.attackList.Where(x => x.Value.stockCost < 3)) {
                if (!_button[cnt])
                {
                    string skillName = skill.Key;
                    _button[cnt] = Instantiate (_obj[2][skillName]) as GameObject;
                    _button[cnt].transform.SetParent (GameObject.FindObjectOfType<Canvas> ().gameObject.transform, false);
                    _button[cnt].transform.position = new Vector3(player.transform.position.x - 1.6f, player.transform.position.y + 1.9f + cnt, 1);
                    Button b = _button[cnt].GetComponent<Button>();
                    //UnityEngine.Events.UnityAction SetAttack = () => { player.SetAttack(skillName); };
                    //b.onClick.AddListener(SetAttack);
                    b.onClick.AddListener(() => player.SetAttack(skillName));
                    if(cnt < _button.Length -1)
                        cnt++;
                }
            }
        }

        public void DestroyButton()
        {
            for (int i = 0; i < _button.Length; i++)
            {
                if (_button[i])
                {
                    Destroy (_button[i]);
                    _button[i] = null;
                }
            }
        }

        public void DestroyNameplate(string battleID)
        {
            if(_nameplate[battleID])
            {
                Destroy(_nameplate[battleID]);
                _nameplate[battleID] = null;
            }
        }

        public void CreateHpBar()
        {
            _hpBar = Instantiate ((GameObject)Resources.Load("hpBar")) as GameObject;
            _hpBar.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().gameObject.transform, false);
            _hpBar.transform.position = new Vector3(-7.6f,0,2);
        }

        public void SpawnAttackOrder ()
        {
            AttackTracker at = GetComponent<AttackTracker>();

            List<string> ids = new List<string>(); //TODO: set to profile names
            List<string> nameplates = new List<string>();

            foreach(var obj in at.attackOrder.OrderByDescending(x => x.Value))
            {
                ids.Add(obj.Key.battleID);
                nameplates.Add(obj.Key.GetComponent<Profile>().nameplate);
            }
            _nameplate = new Dictionary<string, GameObject>();
            _obj[1] = GenerateObjDictionary(ids.ToArray(), nameplates.ToArray());
            for(int i = 0; i <= at.attackOrder.Count - 1; i++)
            {
                _nameplate[ids[i]] = Instantiate(_obj[1][ids[i]]) as GameObject;
                _nameplate[ids[i]].transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform,false);
                _nameplate[ids[i]].transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
            }
        }

        public void SetAttackOrder()
        {
            int i = 0;
            AttackTracker at = GetComponent<AttackTracker>();
            foreach(var obj in at.attackOrder.OrderByDescending(x => x.Value))
            {
                _nameplate[obj.Key.battleID].transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
                i++;
            }
        }

        private Dictionary<string, GameObject> GenerateObjDictionary(string[] keys, string[] resource)
        {
            var dictionary = new Dictionary<string, GameObject>();
            for (int i = 0; i < keys.Length; i++)
            {
                if(dictionary.ContainsKey(keys[i])) {

                }
                dictionary.Add(keys[i], (GameObject)Resources.Load(resource[i]));
            }
            return dictionary;
        }
    }
}