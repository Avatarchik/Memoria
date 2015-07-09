using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public class UIMgr : MonoBehaviour
    {
        private ActorSpawner _spawner;
        private AttackTracker _attackTracker;
        private Dictionary<string, GameObject> _cursor;
        private Dictionary<string, UIElement> _elements;

        void Awake ()
        {
            _attackTracker = GetComponent<AttackTracker>();
            _spawner = FindObjectOfType<ActorSpawner>();
            _cursor = new Dictionary<string, GameObject>();
            _elements = new Dictionary<string, UIElement>();
        }

        void Update()
        {
           // Update nameplate order

            int i = 0;
            foreach(var obj in _attackTracker.attackOrder.OrderByDescending(x => x.Value))
            {
                var namebar = obj.Key.GetComponent<Namebar>().spriteResource;
                _elements[namebar].transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
                i++;
            }
        }

        //************************************ Skill frame

        public void ShowDescBar(string resource)
        {
            var frame = (_spawner.Spawn<DescriptionFrame>(resource)).GetComponent<DescriptionFrame>();
            frame.ParentToUI();
            frame.Init();
            frame.name = "Frame_" + resource;
            _elements.Add("frame_"+ resource, frame);
        }

        //************************************ Cursor

        public void SetCursor(string owner, GameObject obj)
        {
            Vector3 pos = obj.transform.position;
            pos.y += 2f;

            var cursorObj = (_spawner.Spawn<BattleCursor>("cursor")).GetComponent<BattleCursor>();
            cursorObj.ParentToUI();
            cursorObj.Init();
            cursorObj.transform.position = pos;
            _elements.Add("cursor_"+ owner, cursorObj);
        }

        public void SetCurorAnimation(TargetType targets, string owner)
        {
            switch (targets)
            {
                case TargetType.ALL:
                    foreach(var c in _elements.Where(x => x.Key.Contains("cursor")))
                    {
                        c.Value.GetComponent<BattleCursor>().SelectAnimation();
                    }
                    break;
                case TargetType.SINGLE:
                    _elements["cursor_"+ owner].GetComponent<BattleCursor>().SelectAnimation();
                    break;
            }
        }

        //************************************ Skills

        public void SpawnSkills(Hero player)
        {
            var profile = player.GetComponent<Profile>();
            var cnt = 0;
            foreach(var skill in profile.attackList.Where(x => x.Value.stockCost < 3))
            {
                var skillObj = (_spawner.Spawn<SkillIcon>(skill.Key)).GetComponent<SkillIcon>();
                skillObj.ParentToUI();
                skillObj.Init();
                skillObj.SetOnClick(new Action<string>(player.SetAttack), skill.Key);
                skillObj.transform.position = new Vector3(player.transform.position.x - 1.6f, player.transform.position.y + 1.9f + cnt, 1);
                skillObj.name = skill.Key;
                _elements.Add("skill_" + skill.Key, skillObj);
                cnt++;
            }
        }

        //************************************ Nameplates

        public void SpawnNamebars(Dictionary<Entity, float> actors)
        {
            var i = 0;
            foreach(var obj in actors.OrderByDescending(x => x.Value))
            {
                var namebar = obj.Key.GetComponent<Namebar>();
                var barObj = (_spawner.Spawn<Namebar>(namebar.spriteResource)).GetComponent<Namebar>();
                barObj.ParentToUI();
                barObj.Init();
                barObj.transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
                _elements.Add(namebar.spriteResource, barObj);
               i++;
            }
        }

        //************************************ Destroyx

        public void DestroyElement(string elementType)
        {
            foreach(var element in _elements)
            {
                if(element.Key.Contains(elementType))
                {
                    element.Value.Destroy();
                }
            }
            foreach(var element in _elements.Where(id => id.Key.Contains(elementType)).Select(id => id.Key).ToList())
            {
                _elements.Remove(element);
            }
        }
    }
}