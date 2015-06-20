using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParameterManager : MonoBehaviour
{
	private DungeonParameter _parameter;
    public DungeonParameter parameter
	{
		get
		{
			if (_parameter == null)
			{
				_parameter = new DungeonParameter();
				
				_parameter.changedLevelValue += (sender, e) =>
				{
					int level = e.parameter.level;
					levelText.text = "Lv." + level.ToString("00");
				};
				_parameter.changedHpValue += (sender, e) => { UpdateHpText(); };
				_parameter.changedSpValue += (sender, e) => { UpdateSpText(); };
				_parameter.changedFloorValue += (sender, e) =>
				{
					int floor = e.parameter.floor;
					floorText.text = floor.ToString("000F");
				};
				_parameter.changedSkillValue += (sender, e) =>
				{
					string skill = e.parameter.skill;
					skillText.text = skill;
				};

				_parameter.changedTpValue += (sender, e) => { UpdateTpText(); };
			}

			return _parameter;
		}
	}

    [SerializeField]
    private Text
        levelText;

    [SerializeField]
    private Text
        hpText;

    [SerializeField]
    private Text
        spText;

    [SerializeField]
    private Text
        floorText;

    [SerializeField]
    private Text
        skillText;

    [SerializeField]
    private Text
        tpText;

    void Awake()
    {
    }

	public void SetParamater(DungeonParameter parameter)
	{
		this.parameter.Set(parameter);
	}

    private void UpdateHpText()
    {
        int hp = parameter.hp;
        int maxHp = parameter.maxHp;
        hpText.text = string.Format("{0:000}/{1:000}", hp, maxHp);
    }

    private void UpdateSpText()
    {
        int sp = parameter.sp;
        int maxSp = parameter.maxSp;
        spText.text = string.Format("{0:000}/{1:000}", sp, maxSp);
    }

    private void UpdateTpText()
    {
        int tp = parameter.tp;
        tpText.text = tp.ToString();
    }

    // Use this for initialization
//    void Start()
//    {    
//    }
    
    // Update is called once per frame
//    void Update()
//    {    
//    }
}