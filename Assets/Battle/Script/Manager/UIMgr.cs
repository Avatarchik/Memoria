using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public class UIMgr : MonoBehaviour {

        private ActorSpawner _spawner;
        private AttackTracker _attackTracker;
        private Dictionary<string, GameObject>[] _obj;
        private GameObject[] _button;
        private Dictionary<string, GameObject> _cursor;
        private Dictionary<string, GameObject> _nameplate;
        private GameObject _descFrame;

//        private Dictionary<string, UIElement> _elements;

        // Use this for initialization
        void Awake () {
            _spawner = FindObjectOfType<ActorSpawner>();
            _obj = new Dictionary<string, GameObject>[3];
            _cursor = new Dictionary<string, GameObject>();

//            _elements = new Dictionary<string, UIElement>();

            for (int i = 0; i < _obj.Length; i++)
            {
                _obj[i] = new Dictionary<string, GameObject>();
            }
        }

        // Update is called once per frame


        public void ShowDescBar(string resource)
        {
            if(!_descFrame)
            {
                var frame = (GameObject)Resources.Load(resource);
                _descFrame = Instantiate(frame);
                _descFrame.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
                _descFrame.transform.position = new Vector3(-0.0f, 4.5f, 1);
            }
        }

        public void RemoveDescBar()
        {
            Destroy(_descFrame);
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
                if(_cursor.ContainsKey(owner))
                {
                    Destroy(_cursor[owner]);
                    _cursor.Remove(owner);
                }
            }
        }

        //TODO: Set selection animation based on single/multiple targets
        public void SetCurorAnimation(TargetType targets, string owner)
        {
            Animator anim;
            switch (targets)
            {
                case TargetType.ALL:
                    foreach(var c in _cursor)
                    {
                        anim = c.Value.GetComponent<Animator>();
                        anim.SetBool("select", true);
                    }
                    break;
                case TargetType.SINGLE:
                    anim = _cursor[owner].GetComponent<Animator>();
                    anim.SetBool("select", true);
                    break;
            }
        }

        //************************************ Skills


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

        public void SpawnSkills(Hero player)
        {

            var profile = player.GetComponent<Profile>();
//            string[] skills = player.GetSkills();
            foreach(var skill in profile.attackList.Where(x => x.Value.stockCost < 3))
            {
                var skillObj = _spawner.Spawn<SkillIcon>(skill.Key);
                skillObj.GetComponent<SkillIcon>().SetOnClick(player.SetAttack, skill.Key);
                skillObj.GetComponent<UIElement>().SetParent();
                //

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

        //************************************ Nameplates

        public void DestroyNameplate(string battleID)
        {
            if(_nameplate[battleID])
            {
                Destroy(_nameplate[battleID]);
                _nameplate[battleID] = null;
            }
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

        public void SpawnNamebars(Dictionary<Entity, float> actors)
        {
            foreach(var obj in actors.OrderByDescending(x => x.Value))
            {
                var namebar = obj.Key.GetComponentInParent<Namebar>();
                var actorId = obj.Key.battleID;
                var spawnedBar = _spawner.Spawn<Namebar>(namebar.spriteResource);
                spawnedBar.GetComponent<Namebar>().SetParent();
                _nameplate.Add(actorId, spawnedBar);
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