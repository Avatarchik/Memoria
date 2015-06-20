using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapButton : MonoBehaviour
{
    private DungeonManager dungeonManager;

    [SerializeField]
    private GameObject returnButton;

    [SerializeField]
    private MenuButton menuButton;

    [SerializeField]
    private List<GameObject> setActiveObjects;

    private List<GameObject> activeObjects;

    void Awake()
    {
        returnButton.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        dungeonManager = DungeonManager.instance;

        dungeonManager.changedDungeonState += (sender, e) => 
        {
            if (e.nowState == DungeonState.MapViewer && e.nextState == DungeonState.OpenMenu)
            {
                Exit();
            }
        };
    }
    
    // Update is called once per frame
//    void Update()
//    {    
//    }

    public void Enter()
    {
        if (dungeonManager.activeState == DungeonState.OpenMenu)
        {
            dungeonManager.EnterState(DungeonState.MapViewer);
            activeObjects = setActiveObjects.Where(g => g.activeSelf).ToList();
            activeObjects.ForEach(g => g.SetActive(false));
            returnButton.SetActive(true);
        }
    }

    public void Exit()
    {
        returnButton.SetActive(false);
        activeObjects.ForEach(g => g.SetActive(true));
        menuButton.OnMenuExit();
    }
}
