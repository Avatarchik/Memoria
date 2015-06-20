using UnityEngine;
using System.Collections;

public abstract class AttackType : MonoBehaviour {

    public int phaseCost;
    
    protected int animationDur;
    public bool attacked { get; set; }
    public GameObject particleEffect; 
    public GameObject effectObj;
    public GameObject normalEffect;
    
    public virtual int AttackTime 
    {
        get { return animationDur; }
    } 
    public abstract void Execute(Damage dmg, IDamageable target);
    public abstract void Execute(IDamageable target);
    public abstract void PlayEffect(Entity target);
} 
