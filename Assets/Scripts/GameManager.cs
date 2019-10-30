using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float coinChance;

    public int remainingCoins;

    public int numberCollected = 0;
    public int numberLeft = 0;
    public int originalNumber = 0;

    public Text textBox;
    bool displayWinText = false;


    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake()
    {
        board.Initialize(boardSize, cellContentFactory, coinChance);
        originalNumber = numberLeft = cellContentFactory.typeCounter[GameEnum.GridCellContentType.Item];
        numberCollected = 0;
        //player = Instantiate(playerPrefab);
        //player.transform.SetParent(transform, false);
        player.CurrentCell = board.GetCell(playerStartPosition);
        textBox.text = numberCollected + " / " + originalNumber;
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
            //HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //HandleAlternativeTouch();
        }

        int items = cellContentFactory.typeCounter[GameEnum.GridCellContentType.Item];
        if (items == 0 && displayWinText == false)
        {
            if (items == 0)
            {
                textBox.text = "Congratulations....";
            }
        }
        else if (items != numberLeft)
        {
            numberLeft = items;
            numberCollected = originalNumber - numberLeft;
            
        }
        else
        {
            textBox.text = numberCollected + " / " + originalNumber;
        }
    }

    void HandleTouch()
    {
        //GridCell cell = board.GetCell(TouchRay);
        //if (cell != null)
        //{
        //    //cell.Content = cellContentFactory.Get(GameEnum.GridCellContentType.Destination);
        //    board.ToggleWall(cell);
        //}

        RaycastHit hit;
        Ray ray = TouchRay;
        Debug.Log("Attempting Raycast...");
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit!! " + hit.GetType());
            //Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
        }

    }


    void HandleAlternativeTouch()
    {
        //GridCell cell = board.GetCell(TouchRay);
        //if (cell != null)
        //{
        //    //cell.Content = cellContentFactory.Get(GameEnum.GridCellContentType.Destination);
        //    board.ToggleDestination(cell);
        //}
    }
}
