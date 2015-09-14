using UnityEngine;
using System.Collections.Generic;
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
        private BottomParticles _bottomParticles;

        void Start ()
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
        public void SetCurorAnimation(TargetType targets, Entity target)
        {
            switch (targets)
            {
                case TargetType.SINGLE:
                    if(_elements.ContainsKey("cursor_"+ target.battleID))
                        _elements["cursor_"+ target.battleID].GetComponent<BattleCursor>().SelectAnimation();
                    break;

                default:
                    foreach(var c in _elements.Where(x => x.Key.Contains("cursor")))
                    {
                        c.Value.GetComponent<BattleCursor>().SelectAnimation();
                    }
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
                skillObj.spriteResource = skill.Value.spriteData.barSprite;
                skillObj.ParentToUI();
                skillObj.Init();
                skillObj.SetOnClick(new Action<string>(player.SetAttack), skill.Key);
                skillObj.transform.position = new Vector3(
                                                          (player.GetComponent<Profile>().skillPos.x),
                                                          (player.GetComponent<Profile>().skillPos.y) - cnt,
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
                var namebarId = obj.Key.GetComponent<Profile>().nameplateId;
                var barObj = (_spawner.Spawn<Namebar>("UI/Namebar")).GetComponent<Namebar>();
                barObj.SetSprite(namebarId);
                barObj.ParentToUI();
                barObj.SetSlotTable(_queueSlots);
                barObj.SlotPos = (int)obj.Key.orderIndex;
//                barObj.transform.position = _queueSlots[(int)obj.Key.orderIndex];

                barObj.Init();
                barObj.name = "Namebar_" + obj.Key.battleID.ToString();
                _elements.Add("Namebar_"+ obj.Key.battleID, barObj);
            }
        }

        public void UpdateNameplates(NewTurn e)
        {
            var barObj = (Namebar)_elements["Namebar_"+ e.entity.battleID];
            if(!barObj) {
                return;
            }
//            Debug.Log(e.entity.orderIndex +" : "+ e.entity);
            barObj.SlotPos = (int)e.entity.orderIndex;
            if(e.entity.charge)
                barObj.CastingEffect();

        }

        //************************************ Description frame

        public void SpawnDescription(string resource)
        {
            var frame = (_spawner.Spawn<DescriptionFrame>("UI/description_frame")).GetComponent<DescriptionFrame>();
            frame.spriteResource = resource;
            frame.ParentToUI();
            frame.Init();
            frame.name = "frame_desc";
            _elements.Add("frame_desc", frame);
        }

        //************************************ Result

        public void SpawnResult(Sprite resultSprite, bool boss)
        {
            var result = (_spawner.Spawn<Result>("UI/result")).GetComponent<Result>();
            result.ParentToUI();
            result.GetComponent<UnityEngine.UI.Image>().sprite = resultSprite;
            result.transform.position = new Vector3(0, 0, 1);
            _elements.Add("result", result);
            if(boss)
            {
                if(GameData.floorMax + 1 < 2)
                    GameData.floorMax += 1;
            }
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

        public void Clear()
        {
            _elements.Clear();
        }
    }
}