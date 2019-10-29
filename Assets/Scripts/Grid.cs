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

    GridCellContentFactory contentFactory;

    public bool showPaths;

    public int remainingCoins = 0;

    public void Initialize(Vector2Int size, GridCellContentFactory contentFactory, float coinChance)
    {
        // Resize the main ground
        this.size = size;
        this.contentFactory = contentFactory;
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
                if (x == 0 || x == size.x-1 || y == 0 || y == size.y-1)
                    cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Wall);
                else
                {
                    if (Random.value <= coinChance)
                    {
                        cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Item);
                    }
                    else
                    {
                        cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Empty);
                    }
                }
            }
        }

        ToggleDestination(cells[cells.Length / 2]);

    }

    public bool FindPaths()
    {
        foreach (GridCell cell in cells)
        {
            if (cell.Content.Type == GameEnum.GridCellContentType.Destination)
            {
                cell.BecomeDestination();
                searchFrontier.Enqueue(cell);
            }
            else
            {
                cell.ClearPath();
            }
        }
        if (searchFrontier.Count == 0)
        {
            return false;
        }
        //cells[cells.Length / 2].BecomeDestination();
        //searchFrontier.Enqueue(cells[cells.Length/2]);

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

        // Make sure there's never an enclosed space
        foreach (GridCell cell in cells)
        {
            if (!cell.HasPath)
            {
                return false;
            }
        }

        if (showPaths)
        {
            foreach (GridCell cell in cells)
            {
                cell.ShowPath();
            }
        }
        return true;
    }


    public GridCell GetCell(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);
            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                return cells[x + y * size.x];
            }
        }
        return null;
    }

    public GridCell GetCell(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < size.x && pos.y >= 0 && pos.y < size.y)
        {
            return cells[pos.x + pos.y * size.x];
        }
        return null;
    }


    public void ToggleDestination (GridCell cell)
    {
        if (cell.Content.Type == GameEnum.GridCellContentType.Destination)
        {
            cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Empty);
            if (!FindPaths()) // failure to find a path
            {
                cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Destination);
                FindPaths();
            }
        }
        else if (cell.Content.Type == GameEnum.GridCellContentType.Empty)
        {
            cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleWall (GridCell cell)
    {
        if (cell.Content.Type == GameEnum.GridCellContentType.Wall)
        {
            cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Empty);
            FindPaths();
        }
        else if (cell.Content.Type == GameEnum.GridCellContentType.Empty)
        {
            cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Wall);
            if (!FindPaths())
            {
                cell.Content = contentFactory.Get(GameEnum.GridCellContentType.Empty);
                FindPaths();
            }
        }
    }

    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (showPaths)
            {
                foreach (GridCell cell in cells)
                {
                    cell.ShowPath();
                }
            }
            else
            {
                foreach (GridCell cell in cells)
                {
                    cell.HidePath();
                }
            }
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
