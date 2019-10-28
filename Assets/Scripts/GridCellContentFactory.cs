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

    public void Reclaim (GridCellContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject); // TODO: Integrate into a real object pool later
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
        switch (type)
        {
            case GameEnum.GridCellContentType.Destination: return Get(destinationPrefab);
            case GameEnum.GridCellContentType.Wall: return Get(wallPrefab);
            case GameEnum.GridCellContentType.Empty: return Get(emptyPrefab);
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
