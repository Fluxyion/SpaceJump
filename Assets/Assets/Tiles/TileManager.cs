using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject roadblockPrefab;
    public GameObject collectiblePrefab;
    public int tilesAhead = 5; // Number of tiles to load ahead of the player
    public int tilesBehind = 2; // Number of tiles to keep active behind the player
    public float tileWidth = 80f; // Width of each tile
    public float roadblockChance = 0.2f; // Chance to spawn a roadblock on a tile
    public float collectibleChance = 0.1f; // Chance to spawn a collectible on a tile
    public float minY = 0f; // Minimum height for roadblocks and collectibles
    public float maxY = 20f; // Maximum height for roadblocks and collectibles
    public float minDistanceBetween = 2f; // Minimum vertical distance between roadblocks and collectibles
    private Transform player;
    private LinkedList<GameObject> activeTiles = new LinkedList<GameObject>();
    private Queue<GameObject> tilePool = new Queue<GameObject>();

    private int currentTileIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Initialize the first set of tiles
        for (int i = -tilesBehind; i <= tilesAhead; i++)
        {
            SpawnTile(i);
        }
    }

    void Update()
    {
        int playerTileIndex = Mathf.FloorToInt(player.position.x/ tileWidth);

        // Load new tiles ahead of the player
        while (currentTileIndex < playerTileIndex + tilesAhead)
        {
            currentTileIndex++;
            SpawnTile(currentTileIndex);
        }

        // Deactivate tiles behind the player
        while (activeTiles.First.Value.transform.position.x < (playerTileIndex - tilesBehind) * tileWidth)
        {
            DeactivateTile(activeTiles.First.Value);
            activeTiles.RemoveFirst();
        }
    }

    void SpawnTile(int index)
    {
        GameObject tile;
        if (tilePool.Count > 0)
        {
            tile = tilePool.Dequeue();
            tile.SetActive(true);
        }
        else
        {
            tile = Instantiate(tilePrefab);
        }
        tile.transform.position = new Vector3(index * tileWidth, -5.6f, 0);
        activeTiles.AddLast(tile);

        // Variables to store y positions to avoid overlap
        float roadblockY = float.MinValue;
        float collectibleY = float.MinValue;

        // Randomly spawn roadblock on the tile
        if (Random.value < roadblockChance)
        {
            int attemptsY = 0;
            int attemptsX = 0;
            const int maxAttemptsY = 4;
            const int maxAttemptsX = 1;
            if (attemptsX < maxAttemptsX)
            {
                while (attemptsY < maxAttemptsY)
                {

                    roadblockY = Random.Range(minY, maxY);
                    Vector3 roadblockPosition = new Vector3(tile.transform.position.x, roadblockY, 0);
                    Instantiate(roadblockPrefab, roadblockPosition, Quaternion.identity, tile.transform);

                    attemptsY++;
                }

                attemptsX++;
            }
        }

        // Randomly spawn collectible on the tile
        if (Random.value < collectibleChance)
        {
            bool validPosition = false;
            int attempts = 0;
            const int maxAttempts = 10;

            while (!validPosition && attempts < maxAttempts)
            {
                collectibleY = Random.Range(minY, maxY);
                if (Mathf.Abs(collectibleY - roadblockY) >= minDistanceBetween || roadblockY == float.MinValue)
                {
                    validPosition = true;
                }
                attempts++;
            }

            if (validPosition)
            {
                Vector3 collectiblePosition = new Vector3(tile.transform.position.x, collectibleY, 0);
                Instantiate(collectiblePrefab, collectiblePosition, Quaternion.identity, tile.transform);
            }
        }
    }

    void DeactivateTile(GameObject tile)
    {
        tile.SetActive(false);
        tilePool.Enqueue(tile);
    }
}