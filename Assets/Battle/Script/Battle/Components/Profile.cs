﻿using UnityEngine;
using System.Collections.Generic;

abstract public class Profile : MonoBehaviour {

    
    public Parameter parameter;
    public AttackType attackType;
    public string ultimateAttack;
    public string nameplate { get; set; }
    public Dictionary<string, AttackType> attackList = new Dictionary<string, AttackType>();
    //public GameObject attackObj;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        
    }   
}
