using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using Memoria.Battle.GameActors;
using Memoria.Battle.Events;

namespace Memoria.Battle.Managers
{
    public class UIMgr : MonoBehaviour
    {
        private ActorSpawner _spawner;
        private AttackTracker _attackTracker;
        private Dictionary<string, UIElement> _elements;
        private Vector3[] _queueSlots;

        void Awake ()
        {
            _attackTracker = GetComponent<AttackTracker>();
            _spawner = FindObjectOfType<ActorSpawner>();
            _elements = new Dictionary<string, UIElement>();
            EventMgr.Instance.AddListener<NewTurn>(UpdateNameplates);
        }

        //************************************ Cursor

        public void SpawnCursor(string owner, GameObject obj)
        {
            Vector3 pos = obj.transform.position;
            pos.y += 2f;

            var cursorObj = (_spawner.Spawn<BattleCursor>("UI/cursor")).GetComponent<BattleCursor>();
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
                var skillObj = (_spawner.Spawn<SkillIcon>("Skills/"+ skill.Key)).GetComponent<SkillIcon>();
                skillObj.ParentToUI();
                skillObj.Init();
                skillObj.SetOnClick(new Action<string>(player.SetAttack), skill.Key);
                skillObj.transform.position = new Vector3(
                                                          (player.GetComponent<Profile>().skillPos.x),
                                                          (player.GetComponent<Profile>().skillPos.y - 1.0f) + cnt,
                                                          1);
                skillObj.name = skill.Key;
                _elements.Add("skill_" + skill.Key, skillObj);
                cnt++;
            }
        }

        //************************************ Nameplates

        public void SpawnNamebars(Dictionary<Entity, float> actors)
        {
            _queueSlots = _attackTracker.GetSlots();

            foreach(var obj in actors.OrderByDescending(x => x.Value))
            {
                var namebar = obj.Key.GetComponent<Namebar>();
                var barObj = (_spawner.Spawn<Namebar>("UI/"+ namebar.spriteResource)).GetComponent<Namebar>();
                barObj.ParentToUI();
                barObj.Init();
                barObj.transform.position = _queueSlots[(int)obj.Key.orderIndex];
                _elements.Add("Namebar_"+ obj.Key.battleID, barObj);
            }
        }

        public void UpdateNameplates(NewTurn e)
        {
            Namebar barObj = (Namebar)_elements["Namebar_"+ e.entity.battleID];
            if(e.curve && barObj)
            {
                barObj.transform.SetAsLastSibling();
                barObj.CurvedMove(_queueSlots[(int)e.entity.orderIndex]);
            }
            else if(e.moved && barObj)
            {
                barObj.FallDown(_queueSlots[(int)e.entity.orderIndex]);
            }
        }

        //************************************ Description frame

        public void SpawnDescription(string resource)
        {
            var frame = (_spawner.Spawn<DescriptionFrame>("UI/"+ resource)).GetComponent<DescriptionFrame>();
            frame.ParentToUI();
            frame.Init();
            frame.name = "Frame_" + resource;
            _elements.Add("frame_"+ resource, frame);
        }

        //************************************ Result

        public void SpawnResult(Sprite resultSprite)
        {
            var result = (_spawner.Spawn<Result>("UI/result")).GetComponent<Result>();
            result.ParentToUI();
            result.GetComponent<UnityEngine.UI.Image>().sprite = resultSprite;
            result.transform.position = new Vector3(0, 0, 1);
            _elements.Add("result", result);
        }

        //************************************ Destroy

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