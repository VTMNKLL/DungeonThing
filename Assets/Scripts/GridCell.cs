using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    [SerializeField]
    Transform arrow = default;

    [SerializeField]
    GridCellContent content;

    // Hold path edges to neighboring cells
    public GridCell north, east, south, west, nextOnPath;

    int distance;


    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public bool IsAlternative { get; set; }
    public void ShowPath()
    {
        if (distance == 0)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == south ? southRotation :
            westRotation;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }

    public static void MakeEastWestNeighbors(GridCell east, GridCell west)
    {
        Debug.Assert(
            west.east == null && east.west == null, "Redefined neighbors!"
        );
        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(GridCell north, GridCell south)
    {
        Debug.Assert(
            south.north == null && north.south == null, "Redefined neighbors!"
        );
        south.north = north;
        north.south = south;
    }

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }
public void BecomeDestination () {
		distance = 0;
		nextOnPath = null;
	}
    public bool HasPath
    {
        get
        {
            return distance != int.MaxValue;
        }
    }

    GridCell GrowPathTo(GridCell neighbor)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        if (neighbor.content.Type != GameEnum.GridCellContentType.Wall)
        {
            return neighbor;
        }
        else
        {
            return null;
        }
    }

    public GridCell GrowPathNorth() => GrowPathTo(north);

    public GridCell GrowPathEast() => GrowPathTo(east);

    public GridCell GrowPathSouth() => GrowPathTo(south);

    public GridCell GrowPathWest() => GrowPathTo(west);


    // Field to change the content of this tile
    public GridCellContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");
            if (content != null)
            {
                content.Recycle();
            }
            content = value;
            value.MyCell = this; // order we do things here will matter if we're checking to see if there's only one item reference to this cell
            content.transform.localPosition = transform.localPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
