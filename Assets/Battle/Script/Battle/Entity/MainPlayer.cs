using UnityEngine;
using System.Collections;

public class MainPlayer : Entity, IDamageable {
    private const int HEALTH_BAR_FULL = 40;
    private float _onePercent, _healthPercent;
    // Use this for initialization
    void Start () {
        health = GetComponent<HealthSystem> ();
        health.maxHp = 250;
        health.hp = 250;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        CheckGUIHealth ();
    }

    public void CheckGUIHealth()
    {

        _onePercent = health.maxHp / 100.0f;
        _healthPercent = health.hp / _onePercent;
        
        float healthScale = (HEALTH_BAR_FULL * (_healthPercent / 100.0f));
        UIMgr uiMgr = GameObject.FindGameObjectWithTag("BattleMgr").GetComponent<UIMgr> ();
        uiMgr._hpBar[1].transform.localScale = new Vector3 (HEALTH_BAR_FULL, healthScale, 40);
    }

    public void TakeDamage(int i)
    {
        this.health.hp -= i;
    }
}
