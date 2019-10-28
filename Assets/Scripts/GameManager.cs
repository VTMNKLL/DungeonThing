using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //[SerializeField]
    //Player playerPrefab = default;

    [SerializeField]
    Player player = default;

    [SerializeField]
    Vector2Int playerStartPosition = default;

    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    Grid board = default;

    [SerializeField]
    public GameTimer gameTimer = default;

    [SerializeField]
    GridCellContentFactory cellContentFactory;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake()
    {
        board.Initialize(boardSize,cellContentFactory);
        //player = Instantiate(playerPrefab);
        //player.transform.SetParent(transform, false);
        player.CurrentCell = board.GetCell(playerStartPosition);
    }

    // Invoked if component may have changed
    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            board.ShowPaths = !board.ShowPaths;
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
    }

    void HandleTouch()
    {
        GridCell cell = board.GetCell(TouchRay);
        if (cell != null)
        {
            //cell.Content = cellContentFactory.Get(GameEnum.GridCellContentType.Destination);
            board.ToggleWall(cell);
        }
    }


    void HandleAlternativeTouch()
    {
        GridCell cell = board.GetCell(TouchRay);
        if (cell != null)
        {
            //cell.Content = cellContentFactory.Get(GameEnum.GridCellContentType.Destination);
            board.ToggleDestination(cell);
        }
    }
}
