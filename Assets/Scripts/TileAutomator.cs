using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class TileAutomator : MonoBehaviour
{
    [Range(0, 100)] public int initialChance;
    [Range(1, 8)] public int neighboursNeededForBirth;
    [Range(1, 8)] public int minimumNeighbours;
    [Range(1, 30)] public int repetitions;
    [SerializeField] GameObject tileOverlay;

    private int[,] currentTerrainMap;
    private int[,] proposedTerrainMap;

    public Vector3Int tileMapSize;

    public Tilemap wallMap;
    public Tilemap groundMap;
    public Tile wallTile;
    public Tile groundTile;

    int width;
    int height;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Simulate(repetitions);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearMap();
            NullifyTerrainMap();
        }
    }

    public void Simulate(int repetitions)
    {
        ClearMap();
        SetTileMapSize();

        if (currentTerrainMap == null)
        {
            CreateNewTerrainMap();
        }

        for (int i = 0; i < repetitions; i++)
        {
            currentTerrainMap = GenerateTilePosition(currentTerrainMap);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (currentTerrainMap[x, y] == 0)
                {
                    wallMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), wallTile);
                }
                else if (currentTerrainMap[x, y] == 1)
                {
                    groundMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), groundTile);
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileOverlayInstance = Instantiate(tileOverlay, new Vector2(-x + width / 2, -y + height / 2), Quaternion.identity);
                tileOverlayInstance.transform.parent = transform;
                tileOverlayInstance.GetComponentInChildren<TMP_Text>().text = $"{x},{y}";
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase wall = wallMap.GetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0));
                TileBase ground = groundMap.GetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0));
                if (wall != null) { Debug.Log($"wall found at ({x},{y})"); }
                else if (ground != null) { Debug.Log($"ground found at ({x},{y})"); }
            }
        }
    }

    private void CreateNewTerrainMap()
    {
        currentTerrainMap = new int[width, height];
        InitialPosition();
    }

    private void SetTileMapSize()
    {
        width = tileMapSize.x;
        height = tileMapSize.y;
    }

    public void InitialPosition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentTerrainMap[x, y] = Random.Range(1, 101) < initialChance ? 1 : 0;
            }
        }
    }

    public int[,] GenerateTilePosition(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighbours;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighbours = GetNeighbours(oldMap, ref bounds, x, y);
                if (oldMap[x, y] == 1)
                {
                    newMap[x, y] = neighbours < minimumNeighbours ? 0 : 1;
                    /*else if (neighbours > neighboursNeededForBirth) //Leave this commented out to encourage land masses
                    {
                        newMap[x, y] = 0;
                    }*/
                }
                if (oldMap[x, y] == 0)
                {
                    newMap[x, y] = neighbours > neighboursNeededForBirth ? 1 : 0; // ">" for encouraging landmass, "==" for traditional.
                }
            }
        }
        return newMap;
    }

    private int GetNeighbours(int[,] oldMap, ref BoundsInt bounds, int x, int y)
    {
        int neighbours = 0;
        foreach (var b in bounds.allPositionsWithin)
        {
            if (b.x == 0 && b.y == 0) continue; //Skipping center.
            if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
            {
                neighbours += oldMap[x + b.x, y + b.y];
            }
        }

        return neighbours;
    }

    private void ClearMap()
    {
        wallMap.ClearAllTiles();
        groundMap.ClearAllTiles();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void NullifyTerrainMap() => currentTerrainMap = null;

}
