using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{



    [SerializeField]
    Transform ground = default;

    [SerializeField]
    GridCell cellPrefab = default;

    Vector2Int size;

    GridCell[] cells;

    Queue<GridCell> searchFrontier = new Queue<GridCell>();

    public void Initialize(Vector2Int size)
    {
        // Resize the main ground
        this.size = size;
        ground.localScale = new Vector3(size.x, size.y, 1f);

        // Lay all grid cells
        cells = new GridCell[size.x * size.y];
        Vector2 offset = new Vector2(
            (size.x - 1) * 0.5f, (size.y - 1) * 0.5f
        );
        for (int i = 0, y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x, ++i)
            {
                GridCell cell = cells[i] = Instantiate(cellPrefab);
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = new Vector3(
                    x - offset.x, 0f, y - offset.y
                );

                if (x > 0)
                {
                    GridCell.MakeEastWestNeighbors(cell, cells[i - 1]);
                }
                if (y > 0)
                {
                    GridCell.MakeNorthSouthNeighbors(cell, cells[i - size.x]);
                }

                // Give alternating behavior to BFS
                cell.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    cell.IsAlternative = !cell.IsAlternative;
                }
            }
        }

        FindPaths();

    }

    public void FindPaths()
    {
        foreach (GridCell cell in cells)
        {
            cell.ClearPath();
        }
        cells[cells.Length / 2].BecomeDestination();
        searchFrontier.Enqueue(cells[cells.Length/2]);

        while (searchFrontier.Count > 0)
        {
            GridCell cell = searchFrontier.Dequeue();
            if (cell != null)
            {
                if (cell.IsAlternative)
                {
                    searchFrontier.Enqueue(cell.GrowPathNorth());
                    searchFrontier.Enqueue(cell.GrowPathSouth());
                    searchFrontier.Enqueue(cell.GrowPathEast());
                    searchFrontier.Enqueue(cell.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(cell.GrowPathWest());
                    searchFrontier.Enqueue(cell.GrowPathEast());
                    searchFrontier.Enqueue(cell.GrowPathSouth());
                    searchFrontier.Enqueue(cell.GrowPathNorth());
                }
            }
        }

        foreach (GridCell cell in cells)
        {
            cell.ShowPath();
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
