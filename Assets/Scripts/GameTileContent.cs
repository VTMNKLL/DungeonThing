using UnityEngine;
using System.Collections;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameEnum.GameTileContentType type = default;

    public GameEnum.GameTileContentType Type => type;

}
