using UnityEngine;
using System.Collections;

public class GridCellContent : MonoBehaviour
{
    [SerializeField]
    GameEnum.GridCellContentType type = default;

    GridCellContentFactory originFactory;

    public GridCell myCell;

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
        //myCell.Content = originFactory.Get(GameEnum.GridCellContentType.Empty);
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

    public GridCell MyCell
    {
        get => myCell;
        set
        {
            //Debug.Assert() should assert that a gridcell can't have multiple contents
            myCell = value;
        }
    }

}
