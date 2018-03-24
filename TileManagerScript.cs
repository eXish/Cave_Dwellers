using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileManagerScript : MonoBehaviour {
    public TileScript[] availableTiles;
    // Do not have empty places in the array - will break - must fix in getMatchingTile
    public TileScript centerTile;
    public TileScript defaultTile;
    private const int DUNGEON_SIZE = 31;
    private TileScript[,] dungeon = new TileScript[DUNGEON_SIZE, DUNGEON_SIZE];
    // Keep the dungeon as a square - needs it to perform checking calculations
    private Queue<int> toProcess = new Queue<int>();
    private const int SCALE_OF_TILES = 15;
    private int numOfTiles = 0;

    // Returns a tile that matches the tiles around it
    // isThere - if the tile surrounding the tile to be generated exists (i.e. != null)
    // tunnels - if isThere == true, this array holds true for if the adjacent tile needs a connection to the generating tile, false if it does not have a conncetion

    // For center tiles - use the arrays as normal
    // For edge and corner tiles, for the tiles outside the bounds, put a true for isThere, and a false in tunnels
    private TileScript getMatchingTile(bool[] isThere, bool[] tunnels)
    {
        if(isThere.Length != 8 || tunnels.Length != 8)
        {
            // Don't do anything if invalid data is given, 
            return null;
        }
        // Clone the array of available tiles, declare an int to keep track of how many tiles are in the array
        TileScript[] matchingTiles = new TileScript[availableTiles.Length];
        int matchingTileLength = availableTiles.Length;
        System.Array.Copy(availableTiles, matchingTiles, availableTiles.Length);
        for (int i = 0; i < isThere.Length; i++)
        {   
            // If there is no tile, disregard the tunnel connection
            if(!isThere[i])
            {
                continue;
            }
            // If there is no tunnel connection, delete any tiles that have a true on the connection
            else if(!tunnels[i])
            {
                for (int j = matchingTiles.Length - 1; j >= 0; j--)
                {
                    if(matchingTiles[j].getMarkerIndex(i))
                    {
                        ArrayUtility.RemoveAt(ref matchingTiles, j);
                    }
                }
            }
            // If there is a tunnel connection, delete any tiles that have a false on the connection
            else if (tunnels[i])
            {
                for (int j = matchingTiles.Length - 1; j >= 0; j--)
                {
                    if (!matchingTiles[j].getMarkerIndex(i))
                    {
                        ArrayUtility.RemoveAt(ref matchingTiles, j);
                    }
                }
            }
        }
        // If no matching tiles were found, return the default tile
        if(matchingTiles.Length == 0)
        {
            return defaultTile;
        }
        // Return a random tile from the array of all matches
        return matchingTiles[Random.Range(0, matchingTiles.Length)];
    }
    private TileScript getTileParameters(TileScript[,] tiles, bool[,] isOutsideBounds)
    {
        // If invalid data is given, return null - add a check for each subarray?
        if (tiles.GetLength(0) != 3 || isOutsideBounds.GetLength(0) != 3 || tiles.GetLength(0) != isOutsideBounds.GetLength(0))
        {
            return null;
        }
        // Declare 2 arrays to hold if the tile is there and if the center tile has a connection
        bool[] isThere = new bool[8];
        bool[] tunnels = new bool[8];
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                // If it is the tile we are generating the parameters for, ignore it
                if(i == 1 && j == 1)
                {
                    continue;
                }
                if(!isOutsideBounds[i, j])
                {
                    if(tiles[i, j] == null)
                    {
                        // If the tiles does not exist, isThere is false and tunnels is false
                        isThere[map2DIndextoTileIndex(i, j)] = false;
                        tunnels[map2DIndextoTileIndex(i, j)] = false;
                    } else
                    {
                        // If the tile is there, isThere is true and copy the 7 - index connection to the tunnel array
                        isThere[map2DIndextoTileIndex(i, j)] = true;
                        tunnels[map2DIndextoTileIndex(i, j)] = tiles[i, j].getMarkerIndex(7- map2DIndextoTileIndex(i, j));
                    }
                } else
                {
                    // If it is outside the bounds, true for isThere, false for connection so it doesn't connect to outside the bounds
                    isThere[map2DIndextoTileIndex(i, j)] = true;
                    tunnels[map2DIndextoTileIndex(i, j)] = false;
                }
            } 
        }
        return getMatchingTile(isThere, tunnels);
    }
    // Given a 2D array index, finds the matching tile coordinate
    private int map2DIndextoTileIndex(int first, int second)
    {
        if (first == 0 && second == 0)
        {
            return 0;
        }
        if (first == 0 && second == 1)
        {
            return 1;
        }
        if (first == 0 && second == 2)
        {
            return 2;
        }
        if (first == 1 && second == 0)
        {
            return 4;
        }
        if (first == 1 && second == 1)
        {
            return -1;
        }
        if (first == 1 && second == 2)
        {
            return 3;
        }
        if (first == 2 && second == 0)
        {
            return 5;
        }
        if (first == 2 && second == 1)
        {
            return 6;
        }
        if (first == 2 && second == 2)
        {
            return 7;
        }
        return -1;
    }
    private void processNextInQueue()
    {
        // Grab the next X and Y position
        int currentXPos = toProcess.Dequeue();
        int currentYPos = toProcess.Dequeue();
        // If there is not a tile already there, then generate
        if(dungeon[currentXPos, currentYPos] == null)
        {
            // Declare two 3x3 arrays to hold the tile parameters
            TileScript[,] tiles = new TileScript[3, 3];
            bool[,] isOutsideBounds = new bool[3, 3];
            // Find the tile parameters
            for (int i = 1; i > -2; i--)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i + currentYPos < dungeon.GetLength(1) - 1 && i + currentYPos > 0)
                    {
                        if (j + currentXPos < dungeon.GetLength(0) - 1 && j + currentXPos > 0)
                        {
                            isOutsideBounds[Mathf.Abs(i - 1), j + 1] = false;
                            tiles[Mathf.Abs(i - 1), j + 1] = dungeon[currentXPos + j, currentYPos + i];
                        }
                        else
                        {
                            isOutsideBounds[Mathf.Abs(i - 1), j + 1] = true;
                            tiles[Mathf.Abs(i - 1), j + 1] = null;
                        }
                    }
                    else
                    {
                        isOutsideBounds[Mathf.Abs(i - 1), j + 1] = true;
                        tiles[Mathf.Abs(i - 1), j + 1] = null;

                    }
                }
            }
            // Instantiate and set the tile to the proper transform
            dungeon[currentXPos, currentYPos] = Instantiate(getTileParameters(tiles, isOutsideBounds));
            numOfTiles++;
            dungeon[currentXPos, currentYPos].transform.localPosition = new Vector3(currentXPos * SCALE_OF_TILES, currentYPos * SCALE_OF_TILES, currentYPos);
            dungeon[currentXPos, currentYPos].transform.SetParent(transform);
            // Add the ungenerated tiles to the queue
            findUngeneratedTiles(currentXPos, currentYPos);
        }
       
    }
    // Format
    /* if(Statement(s) to check to make sure the tile is not outside the array bounds)
     *      if(tunnels[index] and the matching 2D coordinate != null)
     *          then add the coordinates to the queue
     */
    private void findUngeneratedTiles(int currentXPos, int currentYPos)
    {
        TileScript current = dungeon[currentXPos, currentYPos];
        bool[] tunnels = current.getAllMarkers();
        if (currentXPos - 1 >= 0 && currentYPos + 1 < dungeon.GetLength(1))
        {
            if (tunnels[0] && dungeon[currentXPos - 1, currentYPos + 1] == null)
            {
                toProcess.Enqueue(currentXPos - 1);
                toProcess.Enqueue(currentYPos + 1);
            }
        }
        if (currentXPos - 1 >= 0)
        {
            if (tunnels[4] && dungeon[currentXPos - 1, currentYPos] == null)
            {
                toProcess.Enqueue(currentXPos - 1);
                toProcess.Enqueue(currentYPos);
            }
        }
        if (currentXPos - 1 >= 0 && currentYPos - 1 >= 0)
        {
            if (tunnels[5] && dungeon[currentXPos - 1, currentYPos - 1] == null)
            {
                toProcess.Enqueue(currentXPos - 1);
                toProcess.Enqueue(currentYPos - 1);
            }
        }
        if (currentYPos + 1 < dungeon.GetLength(1))
        {
            if (tunnels[1] && dungeon[currentXPos, currentYPos + 1] == null)
            {
                toProcess.Enqueue(currentXPos);
                toProcess.Enqueue(currentYPos + 1);
            }
        }
        if (currentXPos + 1 < dungeon.GetLength(0) && currentYPos + 1 < dungeon.GetLength(1))
        {
            if (tunnels[2] && dungeon[currentXPos + 1, currentYPos + 1] == null)
            {
                toProcess.Enqueue(currentXPos + 1);
                toProcess.Enqueue(currentYPos + 1);
            }
        }
        if (currentXPos + 1 < dungeon.GetLength(0))
        {
            if (tunnels[3] && dungeon[currentXPos + 1, currentYPos] == null)
            {
                toProcess.Enqueue(currentXPos + 1);
                toProcess.Enqueue(currentYPos);
            }
            
        }
        if (currentYPos - 1 >= 0)
        {
            if (tunnels[6] && dungeon[currentXPos, currentYPos - 1] == null)
            {
                toProcess.Enqueue(currentXPos);
                toProcess.Enqueue(currentYPos - 1);
            }
        }
        if (currentXPos + 1 < dungeon.GetLength(0) && currentYPos - 1 >= 0)
        {
            if (tunnels[7] && dungeon[currentXPos + 1, currentYPos - 1] == null)
            {
                toProcess.Enqueue(currentXPos + 1);
                toProcess.Enqueue(currentYPos - 1);
            }
        }
    }
    public void GenerateDungeon()
    {
        // Start with the center tile
        int currentXPos = dungeon.GetLength(0) / 2;
        int currentYPos = dungeon.GetLength(1) / 2;
        dungeon[currentXPos, currentYPos] = Instantiate(centerTile, new Vector3(currentXPos * SCALE_OF_TILES, currentYPos * SCALE_OF_TILES, currentYPos), Quaternion.identity);
        numOfTiles++;
        dungeon[currentXPos, currentYPos].transform.SetParent(transform);
        findUngeneratedTiles(currentXPos, currentYPos);
        while (toProcess.Count != 0)
        {
            processNextInQueue();
        }
        if (numOfTiles < DUNGEON_SIZE / 5)
        {
            dungeon = new TileScript[DUNGEON_SIZE, DUNGEON_SIZE];
            GenerateDungeon();
        }
    }
    private void Awake()
    {
        GenerateDungeon();
    }
}