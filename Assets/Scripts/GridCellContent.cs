using UnityEngine;
using System.Collections;

public class GridCellContent : MonoBehaviour
{
    [SerializeField]
    GameEnum.GridCellContentType type = default;

    GridCellContentFactory originFactory;

    public GridCellContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void Recycle ()
    {
        originFactory.Reclaim(this);
    }

    public GameEnum.GridCellContentType Type => type;

}
