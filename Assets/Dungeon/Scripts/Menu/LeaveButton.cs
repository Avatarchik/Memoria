using UnityEngine;
using System.Collections;

public class LeaveButton : MonoBehaviour
{
    private DungeonManager dungeonManager;
    [SerializeField]
    private GameObject
        message;
    [SerializeField]
    private GameObject
        yesButton;
    [SerializeField]
    private GameObject
        noButton;

    void Awake()
    {
        dungeonManager = DungeonManager.instance;
    }

    // Use this for initialization
    void Start()
    {    
        message.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }
    
    // Update is called once per frame
//    void Update()
//    {    
//    }

    public void OnClick()
    {
        if (dungeonManager.activeState == DungeonState.OpenMenu)
        {
            Enter();
        }
    }

    public void Enter()
    {
        dungeonManager.EnterState(DungeonState.LeaveSelect);
        message.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
    }

    public void Exit()
    {
        message.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        dungeonManager.ExitState();
    }

	public void LeaveDungeon()
	{
		dungeonManager.Leave();
	}
}
