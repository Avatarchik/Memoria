using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Memoria.Battle.GameActors;

public class ParamContent : MonoBehaviour
{
    readonly Dictionary<string, GameObject> _params = new Dictionary<string, GameObject>();
    Profile profile;

    void Start()
    {
        _params.Add("hp", new GameObject());
        _params.Add("attack", new GameObject());
        _params.Add("defense", new GameObject());
        _params.Add("speed", new GameObject());

        foreach(var obj in _params)
        {
            obj.Value.AddComponent<Text>();
            obj.Value.transform.SetParent(this.transform);
            obj.Value.name = obj.Key;
        }
    }

    void Update()
    {
        if(GetComponent<Profile>())
        {
            _params["hp"].GetComponent<Text>().text = profile.parameter.hp.ToString();
            _params["attack"].GetComponent<Text>().text = profile.parameter.attack.ToString();
            _params["defense"].GetComponent<Text>().text = profile.parameter.defense.ToString();
            _params["speed"].GetComponent<Text>().text = profile.parameter.speed.ToString();
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
