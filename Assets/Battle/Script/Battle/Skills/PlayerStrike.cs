using UnityEngine;
using System;

public class PlayerStrike : AttackType, ITriggerable
{

    void Start ()
    {
        phaseCost = 1;
        animationDur = 120;    
        effectObj = (GameObject)Resources.Load("explode");
    }
    
    void Update () {
        
    }
  
    public override void Execute(IDamageable target)
    {
        target.TakeDamage(20);
    }
    public override void Execute(Damage damage, IDamageable target)
    {
//        target.TakeDamage(damage);
    }

    public override void PlayEffect (Entity target)
    {
        particleEffect = Instantiate (effectObj) as GameObject;
        particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y -0.3f, -3);
        particleEffect.GetComponent<ParticleSystem>().Play();	
    }
    public override int AttackTime {
        get {
            return base.AttackTime;
        }
    }
}


