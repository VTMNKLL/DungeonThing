using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GridCellContentFactory : MonoBehaviour
{
    Scene contentScene;

    [SerializeField]
    GridCellContent destinationPrefab = default;

    [SerializeField]
    GridCellContent emptyPrefab = default;

    [SerializeField]
    GridCellContent wallPrefab = default;


    [SerializeField]
    GridCellContent itemPrefab = default;

    [SerializeField]
    public Dictionary<GameEnum.GridCellContentType, int> typeCounter;

    void Awake()
    {
        typeCounter = new Dictionary<GameEnum.GridCellContentType, int>();
        foreach (GameEnum.GridCellContentType e in GameEnum.GridCellContentType.GetValues(typeof(GameEnum.GridCellContentType)))
        {
            typeCounter[e] = 0;
        }
    }
    
    public int Count (GridCellContent content)
    {
        return typeCounter[content.Type];
    }

    public int Count(GameEnum.GridCellContentType type)
    {
        return typeCounter[type];
    }

    public int Reclaim (GridCellContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject); // TODO: Integrate into a real object pool later
        typeCounter[content.Type] -= 1;
        return typeCounter[content.Type];
    }

    private GridCellContent Get (GridCellContent prefab)
    {
        GridCellContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }
    public GridCellContent Get(GameEnum.GridCellContentType type)
    {
        typeCounter[type] += 1;
        switch (type)
        {
            case GameEnum.GridCellContentType.Destination: return Get(destinationPrefab);
            case GameEnum.GridCellContentType.Wall: return Get(wallPrefab);
            case GameEnum.GridCellContentType.Empty: return Get(emptyPrefab);
            case GameEnum.GridCellContentType.Item: return Get(itemPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
    void MoveToFactoryScene(GameObject o)
    {
        if (!contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                contentScene = SceneManager.GetSceneByName(name);
                if (!contentScene.isLoaded)
                {
                    contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, contentScene);
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
