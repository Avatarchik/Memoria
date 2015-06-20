using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset;

    private DungeonManager dungeonManager;
    private MapManager mapManager;

	public Animator animator { get { return GetComponent<Animator>(); } }

    /// <summary>
    /// プレイヤーの向き
    /// 0 : left
    /// 1 : right
    /// 2 : down
    /// 3 : up
    /// </summary>
    public int direction
    {
        get { return (int)animator.GetFloat("direction"); }
        set { animator.SetFloat("direction", value); }
    }

    /// <summary>
    /// プレイヤーの移動中フラグ
    /// </summary>
    public bool isMoving
    {
        get { return animator.GetBool("moving"); }
        set { animator.SetBool("moving", value); }
    }

    /// <summary>
    /// プレイヤーの歩く速さ
    /// </summary>
    public float speed
    {
        get { return animator.speed; }
        set { animator.speed = value; }
    }
    [SerializeField]
    private float _speed;

    #region messages
    /// <summary>
    /// プレイヤーのマップ上座標
    /// </summary>
	public Location location { get; set; }

    void Awake()
    {
        dungeonManager = DungeonManager.instance;
        mapManager = dungeonManager.mapManager;
    }

    // Use this for initialization
    void Start()
    {
        speed = _speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dungeonManager.activeState == DungeonState.None)
        {
            StartCoroutine(CoroutineTouch());
        }
    }

    void OnTouchMap(Location touchLocation)
    {
        Location distance = touchLocation - location;

        if (distance.magnitude < 1)
        {
            return;
        }

        Location moveDirection = new Location();
        int[] checkDirections = new [] { 0, 1 };
//        float ax = Mathf.Abs(distance.x);
//        float ay = Mathf.Abs(distance.y);
//        int[] checkDirections = (ax >= ay) ? (new [] { 0, 1 }) : (new [] { 1, 0 });

        Predicate<int> canMove = (dir) =>
        {
            moveDirection = distance.normalized;

            switch(dir)
            {
                case 0:
                    moveDirection.y = 0;
                    break;

                case 1:
                    moveDirection.x = 0;
                    break;
            }

            return moveDirection.magnitude > 0 && CanMove(moveDirection);
        };

        foreach (int checkDirection in checkDirections)
        {
            if (canMove(checkDirection))
            {
                OnMove(moveDirection);
                return;
            }
        }
    }

    void OnMove(Location moveDirection)
    {
        dungeonManager.EnterState(DungeonState.PlayerMoving);

        Vector3 position = mapManager.ToPosition(location + moveDirection);
        float yOffset = -0.2f;
        float time = 1 / speed;
        position.y += yOffset;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", position,
            "time", time,
            "oncomplete", "CompleteMove",
            "easetype", iTween.EaseType.linear));

        direction = ToDirection(moveDirection);
        isMoving = true;
        
        location += moveDirection;
    }
    #endregion

    private IEnumerator CoroutineTouch()
    {
        float time = 0;
        while (Input.GetMouseButton(0))
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        float limit = 0.5f;
        if (time < limit)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Location touchLocation = mapManager.ToLocation(touchPosition);

            if (mapManager.canPutBlockArea.Contains(touchPosition) && mapManager.map.ContainsKey(touchLocation))
            {
                OnTouchMap(touchLocation);
            }
        }
        
        yield break;
    }

    private void CompleteMove()
    {
        isMoving = false;
        dungeonManager.ExitState();
    }

    private bool CanMove(Location moveDirection)
    {
        Location toLocation = location + moveDirection;

        if (!mapManager.map.ContainsKey(toLocation))
        {
            return false;
        }

        Block now = mapManager.map[location];
        Block next = mapManager.map[toLocation];
        int dir = ToDirection(moveDirection);
        return now.shape.directions[dir] && next.shape.directions[dir ^ 1];
    }

    private int ToDirection(Location moveDirection)
    {
        int[,] toDirectionTable = new int[,]
        {
            {0, 0, 1},
            {2, 0, 3},
        };

        moveDirection.Normalize();

        return toDirectionTable[0, moveDirection.x + 1] + toDirectionTable[1, moveDirection.y + 1];
    }

	public void SetPosition(Location location)
	{
		this.location = location;
		transform.position = mapManager.ToPosition(location) + offset;
	}
}