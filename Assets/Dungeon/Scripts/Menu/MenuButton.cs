using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    private DungeonManager dungeonManager;

    [SerializeField]
    private GameObject mapButton;

    [SerializeField]
    private GameObject leaveButton;

    [SerializeField]
    private GameObject returnButton;

    void Awake()
    {
        dungeonManager = DungeonManager.instance;
    }

    // Use this for initialization
    void Start()
    {
        mapButton.SetActive(false);
        leaveButton.SetActive(false);
        returnButton.SetActive(false);
    }
    
    // Update is called once per frame
//    void Update()
//    {
//    }

    public void OnMenuEnter()
    {
        if (dungeonManager.activeState != DungeonState.None)
        {
            return;
        }

        dungeonManager.EnterState(DungeonState.OpenMenu);
        mapButton.SetActive(true);
        leaveButton.SetActive(true);
        returnButton.SetActive(true);
    }

    public void OnMenuExit()
    {
        if (dungeonManager.activeState != DungeonState.OpenMenu)
        {
            return;
        }

        mapButton.SetActive(false);
        leaveButton.SetActive(false);
        returnButton.SetActive(false);
        dungeonManager.ExitState();
    }
}
