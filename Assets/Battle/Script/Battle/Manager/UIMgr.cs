using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class UIMgr : MonoBehaviour {
    
    private AttackTracker _attackTracker;
    private Dictionary<string, GameObject>[] _obj;
    private GameObject[] _button;
    public GameObject[] _hpBar;
    private Dictionary<string, GameObject> _nameplate;
    // Use this for initialization
    void Start () {
        
        _obj = new Dictionary<string, GameObject>[3];
        for (int i = 0; i < _obj.Length; i++) {
            _obj[i] = new Dictionary<string, GameObject>();
        }        
    }
     
    // Update is called once per frame
    void Update () {
    }

    public void ShowSkill(Hero player)
    {
        var profile = player.GetComponent<Profile>();
        string[] skills = player.GetSkills();
        _obj[2] = GenerateObjDictionary(skills, skills);
        int cnt = 0;
        _button = _button ?? new GameObject[profile.attackList.Count];        
        foreach(var skill in profile.attackList) {
            if (!_button[cnt]) {
                string skillName = skill.Key;
                _button[cnt] = Instantiate (_obj[2][skillName]) as GameObject;
                _button[cnt].transform.SetParent (GameObject.FindObjectOfType<Canvas> ().gameObject.transform, false);
                _button[cnt].transform.position = new Vector3(player.transform.position.x - 1.1f, player.transform.position.y + 1.9f + cnt, -1);
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
        for (int i = 0; i < _button.Length; i++) {
            if (_button[i]) {
                Destroy (_button[i]);
                _button[i] = null;
            }
        }            
    }

    public void CreateHpBar()
    {       
        string[] parts = { "hpFrame", "hpBg", "hpFace" };
        string[] resources = { "HPWaku", "HP00", "HP01" };
        _obj[0] = GenerateObjDictionary(parts, resources);
        _hpBar = new GameObject[3];
        for(int i = 0;i < _hpBar.Length; i++) {
            _hpBar[i] = Instantiate (_obj[0][parts[i]]) as GameObject;
            _hpBar[i].transform.SetParent (GameObject.FindObjectOfType<Canvas> ().gameObject.transform, false); 
            _hpBar[i].transform.position = new Vector3(-7.4f,-4.0f,2);
        }
    }

    public void SpawnAttackList ()
    {  
        AttackTracker at = GetComponent<AttackTracker>();       
        
        List<string> ids = new List<string>(); //TODO: set to profile names
        List<string> nameplates = new List<string>();
        
        foreach(var obj in at.attackOrder.OrderByDescending(x => x.Value)) {
            Debug.Log(obj.Key.GetComponent<Profile>().nameplate);
            ids.Add(obj.Key.battleID);
            nameplates.Add(obj.Key.profile.nameplate);
        }
        _nameplate = new Dictionary<string, GameObject>();
        _obj[1] = GenerateObjDictionary(ids.ToArray(), nameplates.ToArray());
        for(int i = 0; i <= at.attackOrder.Count - 1; i++) {
            _nameplate[ids[i]] = Instantiate(_obj[1][ids[i]]) as GameObject;
            _nameplate[ids[i]].transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform,false);
            _nameplate[ids[i]].transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
        }
    }

    public void SetAttackList()
    {
        int i = 0;
        AttackTracker at = GetComponent<AttackTracker>();
        foreach(var obj in at.attackOrder.OrderByDescending(x => x.Value)) {
            _nameplate[obj.Key.battleID].transform.position = new Vector3(7.2f, -0.3f - ((i - 4) * 1.0f), 1);
            i++;
        }
    }    

    private Dictionary<string, GameObject> GenerateObjDictionary(string[] keys, string[] resource)
    {
        var dictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < keys.Length; i++) {
            if(dictionary.ContainsKey(keys[i])) {
 
            }
            dictionary.Add(keys[i], (GameObject)Resources.Load(resource[i]));
        }
        return dictionary;
    }

}
