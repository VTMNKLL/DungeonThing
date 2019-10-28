using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10.0f)]
    public float executionTime;

    [SerializeField]
    [Range(0.1f, 10.0f)]
    public float _passiveTime;

    [SerializeField]
    private float _timerPosition;

    private float _totalTime;

    public bool playerAct;
     
    public bool enemyAct;

    public bool playerActFirstFrame;
    public bool enemyActFirstFrame;

    private bool playerActFirstFrameLock;
    private bool enemyActFirstFrameLock;

    // Start is called before the first frame update
    void Start()
    {
        _totalTime = executionTime + _passiveTime;
        _timerPosition = 0;
    }

    // Late Update is called once per frame after every Update
    void LateUpdate()
    {
        _totalTime = executionTime + _passiveTime;

        _timerPosition += Time.deltaTime;
        if (_timerPosition >= _totalTime)
        {
            _timerPosition = _timerPosition - _totalTime;
        }

        playerAct = (_timerPosition < executionTime);
        playerActFirstFrame = (playerAct && !playerActFirstFrame && !playerActFirstFrameLock);
        if (playerActFirstFrame)
        {
            Debug.Log("Player FirstFrame!");
            playerActFirstFrameLock = true;
            enemyActFirstFrameLock = false;
        }
        enemyAct = ((_timerPosition + _totalTime/2.0)%_totalTime < executionTime);
        enemyActFirstFrame = (enemyAct && !enemyActFirstFrame && !enemyActFirstFrameLock);
        if (enemyActFirstFrame)
        {
            Debug.Log("Enemy FirstFrame!");
            playerActFirstFrameLock = false;
            enemyActFirstFrameLock = true;
        }

    }
}
