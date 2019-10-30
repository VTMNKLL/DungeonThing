using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    GridCell currentCell;
    [SerializeField]
    GameManager _gameManager;
    [SerializeField]
    GridCell targetCell = null;
    GridCell lastCell = null;
    public float reach = 1;

   enum Action
    {
        None, Step, Turn, Click
    }

    Action lastAction = Action.None;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    public float progressThruMoveAnimation = 0;
    public float progressThruTurnAnimation = 0;
    bool animatingMove = false;
    bool animatingTurn = false;
    int moveDirection;
    int turnDirection;
    Vector3 forwardDirection;
    Vector3 lastDirection;
    Vector3 targetDirection;
    Quaternion targetRotation;
    Quaternion lastRotation;
    public float angle = 0;
    public Vector3 finalDirection;
    public Transform camTransform;
    public float stepMultiplier;

    public float debugValue;
    Ray lastClickRay;
    RaycastHit lastHit;

    // Start is called before the first frame update
    void Start()
    {
        forwardDirection = new Vector3(0,0,1);
    }

    void Initialize(GridCell cell)
    {
        this.CurrentCell = cell;
    }

    // Update is called once per frame
    void Update()
    {
        forwardDirection = this.transform.forward;

        if (currentCell != null)
        {


            if (Input.GetKeyDown(KeyCode.W))
            {
                //if (currentCell.north != null)
                //{
                    //Debug.Log("Setting target cell...");
                    //transform.position = currentCell.north.transform.position;
                    //targetCell = currentCell.north;
                    moveDirection += 1;
                    turnDirection = 0;
                    lastAction = Action.Step;
                //}
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //if (currentCell.south != null)
                //{
                    //Debug.Log("Setting target cell...");
                    //transform.position = currentCell.south.transform.position;
                    //targetCell = currentCell.south;
                    moveDirection -= 1;
                    turnDirection = 0;
                    lastAction = Action.Step;
                //}
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //if (currentCell.west != null)
                //{
                    //Debug.Log("Setting target cell...");
                    //transform.position = currentCell.north.transform.position;
                    //targetCell = currentCell.west;
                    turnDirection -= 1;
                    moveDirection = 0;
                    lastAction = Action.Turn;
                //}
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //if (currentCell.east != null)
                //{
                    //Debug.Log("Setting target cell...");
                    //transform.position = currentCell.south.transform.position;
                    //targetCell = currentCell.east;
                    turnDirection += 1;
                    moveDirection = 0;
                    lastAction = Action.Turn;
                //}
            }

            if (Input.GetMouseButton(0))
            {
                //lastAction = Action.Click;
                lastClickRay = TouchRay;
                Debug.Log("Attempting Raycast...");
                //Ray ray = lastClickRay;
                lastAction = (Physics.Raycast(lastClickRay, out lastHit, reach)) ? Action.Click : Action.None; // make sure raycast is valid
            }

            if (_gameManager.gameTimer.playerActFirstFrame)// && targetCell != null)
            {
                if (lastAction == Action.Step)
                {
                    finalDirection = moveDirection * forwardDirection;
                    angle = Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg;
                
                    if (finalDirection.magnitude > 0 && angle >= -45 && angle < 45 && currentCell.north != null && !(currentCell.north.Content.Type == GameEnum.GridCellContentType.Wall || currentCell.north.Content.Type == GameEnum.GridCellContentType.Item))
                        targetCell = currentCell.north;
                    if (finalDirection.magnitude > 0 && (angle > 135 || angle < -135) && currentCell.south != null && !(currentCell.south.Content.Type == GameEnum.GridCellContentType.Wall || currentCell.south.Content.Type == GameEnum.GridCellContentType.Item))
                        targetCell = currentCell.south;
                    if (finalDirection.magnitude > 0 && (angle > 45 && angle < 135) && currentCell.east != null && !(currentCell.east.Content.Type == GameEnum.GridCellContentType.Wall || currentCell.east.Content.Type == GameEnum.GridCellContentType.Item))
                        targetCell = currentCell.east;
                    if (finalDirection.magnitude > 0 && (angle > -135 && angle < -45) && currentCell.west != null && !(currentCell.west.Content.Type == GameEnum.GridCellContentType.Wall || currentCell.west.Content.Type == GameEnum.GridCellContentType.Item))
                        targetCell = currentCell.west;

                    if (targetCell != null)
                    {
                        Debug.Log("!!!!Setting Current Cell!!!!");
                        CurrentCell = targetCell;
                        targetCell = null;
                        progressThruMoveAnimation = 0;
                        animatingMove = true;
                    }
                }
                else if (lastAction == Action.Turn)
                {

                    if (Mathf.Abs(turnDirection) > 0)
                    {
                        if(turnDirection > 1)
                            turnDirection = 1;
                        if(turnDirection < -1)
                            turnDirection = -1;
                        Debug.Log("!!!!SETTING TURN!!!!");
                        targetDirection = Quaternion.AngleAxis(turnDirection * 90, Vector3.up) * forwardDirection;
                    
                        lastRotation = this.transform.rotation;
                        targetRotation = Quaternion.AngleAxis(turnDirection * 90, Vector3.up) * this.transform.rotation;

                        // SHOULD ACTUALLY ALIGN TO A CARDINAL DIRECTION (declare four quaternions at top)

                        progressThruTurnAnimation = 0;
                        animatingTurn = true;
                    }
                }
                else if (lastAction == Action.Click)
                {
                   // assumes valid raycast
                    Debug.Log("Hit!! " + lastHit.GetType());
                    GameObject thing =  lastHit.collider.gameObject;
                    if(thing.transform.parent != null)
                    {
                        GridCellContent content = thing.GetComponentInParent<GridCellContent>();
                        if (content != null )
                        {
                            if (content.Type == GameEnum.GridCellContentType.Item)
                            {
                                content.MyCell.Content = content.OriginFactory.Get(GameEnum.GridCellContentType.Empty);
                                //Debug.Log("Number left: " + content.Recycle());
                            }
                        }
                    }

                        //Transform objectHit = hit.transform;

                    // Do something with the object that was hit by the raycast.

                }

                moveDirection = 0;
                turnDirection = 0;
                //lastHit = null;
                //lastClickRay = null;
                lastAction = Action.None;
            }
        }

        if (animatingMove)
        {
            //this.transform.position += new Vector3(0,Mathf.Sin(progressThruMoveAnimation / _gameManager.gameTimer.executionTime));

            progressThruMoveAnimation += Time.deltaTime;
            if (progressThruMoveAnimation / _gameManager.gameTimer.executionTime > 1)
            {
                progressThruMoveAnimation = _gameManager.gameTimer.executionTime;
                animatingMove = false;
            }
            Vector3 vel = new Vector3();
            this.transform.position = Vector3.Lerp(lastCell.transform.position, currentCell.transform.position, progressThruMoveAnimation / _gameManager.gameTimer.executionTime);
            this.transform.position += new Vector3(0, .05f * Mathf.Cos(stepMultiplier * Mathf.PI * progressThruMoveAnimation/_gameManager.gameTimer.executionTime) * Mathf.Sin(2 * Mathf.PI * progressThruMoveAnimation / _gameManager.gameTimer.executionTime), 0);
            debugValue = progressThruMoveAnimation / _gameManager.gameTimer.executionTime;
        }

        if (animatingTurn)
        {
            progressThruTurnAnimation += Time.deltaTime;
            if (progressThruTurnAnimation > 1)
            {
                progressThruTurnAnimation = 1;
                animatingTurn = false;
            }
            this.transform.rotation = Quaternion.Slerp(lastRotation, targetRotation, progressThruTurnAnimation / _gameManager.gameTimer.executionTime);
            
        }
    }

    private void LateUpdate()
    {
        Ray ray = TouchRay;
        RaycastHit rayhit;
        bool hit = Physics.Raycast(ray, out rayhit, reach); // make sure raycast is valid
        if (hit)
        {
            Selectable gameObj = rayhit.transform.gameObject.GetComponent<Selectable>();
            if (gameObj != null)
            {
                gameObj.mouseOver = true;
            }
        }
    }

    public GridCell CurrentCell
    {
        get => currentCell;
        set
        {
            //transform.position = value.transform.position;
            if(lastCell == null)
            {
                lastCell = value;
                transform.position = value.transform.position;
            }
            else
                lastCell = currentCell;
            currentCell = value;
        }
    }

}
