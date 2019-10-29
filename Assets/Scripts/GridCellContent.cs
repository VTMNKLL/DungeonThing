using UnityEngine;
using System.Collections;

public class GridCellContent : MonoBehaviour
{
    [SerializeField]
    GameEnum.GridCellContentType type = default;

    GridCellContentFactory originFactory;

    float animationTimer = 0;
    float animationSpeed = 0;
    bool animate = false;

    public GridCellContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public int Recycle ()
    {
        return originFactory.Reclaim(this);
    }

    public void RecycleWithAnimation()
    {

    }

    //public void GameObject()
    //{
    //    originFactory.
    //}

    public GameEnum.GridCellContentType Type => type;

}
