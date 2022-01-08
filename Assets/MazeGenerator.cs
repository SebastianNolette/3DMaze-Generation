using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public GameObject[] tiles;
    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] int depth;



    int totalTiles;
    int sliceSize;

    // Start is called before the first frame update
    void Start()
    {
        totalTiles = totalTiles = columns * rows * depth;
        sliceSize = columns*rows;
        LoadMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    List<int>[] Prims(int start)
    {
        //string DebugString = "";
        List<int> mazeTiles = new List<int>();
        List<int> mazeFrontier = new List<int>();
        List<int> mazeTotal = new List<int>();

        List<int>[] connections = new List<int>[totalTiles];
        for (int i=0; i< totalTiles; i++)
        {
            connections[i] = new List<int>();
        }

        //mazeTiles.Add(start);
        mazeFrontier.Add(start);
        mazeTotal.Add(start);

        while (mazeFrontier.Count != 0)
        {
            int frontLen = mazeFrontier.Count;

            int addFrontirIndex = Random.Range(0, frontLen);

            int addTile = mazeFrontier[addFrontirIndex];          

            //DebugString = "addTile:" + addTile + " frontLen:" + frontLen + " addFrontirIndex:" + addFrontirIndex + " mazeTotalLen:" + mazeTotal.Count + " mazeTiles:" + mazeTiles.Count;
            //Debug.Log(DebugString);


            // Adding a connection
            List<int> mazeConnectTiles = FindOldNeighbors(mazeTiles, addTile);
            int connectTileLen = mazeConnectTiles.Count;
            int addConnectionIndex = Random.Range(0, connectTileLen);
            
            //DebugString = "connectTileLen:" + connectTileLen + " addTile:" + addTile + " addConnectionIndex:" + addConnectionIndex;
            //Debug.Log(DebugString);
            if (connectTileLen != 0) { 
                connections[addTile].Add(mazeConnectTiles[addConnectionIndex]);
                connections[mazeConnectTiles[addConnectionIndex]].Add(addTile);
            }

            // Changing the frontier
            mazeFrontier.RemoveAt(addFrontirIndex);
            mazeTiles.Add(addTile);
            
            List<int> newFrontier = GetNewTiles(mazeTotal, addTile);

            mazeTotal.AddRange(newFrontier);
            mazeFrontier.AddRange(newFrontier);

        }


        return connections;
    }
    
    // Gets Tiles that are not within the generated maze
    List<int> GetNewTiles(List<int> tilesIn, int addedTile)
    {
        //List<int> neighbor = AddNeighbor(addedTile);
        List<int> neighbor = AddNeighbor3D(addedTile);
        List<int> siftedNeighbor = new List<int>();
        int neighborLen = neighbor.Count;

        for (int i=0;i< neighborLen; i++)
        {
            if (!tilesIn.Contains(neighbor[i]))
            {
                siftedNeighbor.Add(neighbor[i]);
            }
        }

        return siftedNeighbor;
    }

    // Gets Tiles that are within the generated maze
    List<int> FindOldNeighbors(List<int> tilesIn, int addedTile)
    {
        //List<int> neighbor = AddNeighbor(addedTile);
        List<int> neighbor = AddNeighbor3D(addedTile);
        List<int> siftedNeighbor = new List<int>();
        int neighborLen = neighbor.Count;
        for (int i = 0; i < neighborLen; i++)
        {
            if (tilesIn.Contains(neighbor[i]))
            {
                siftedNeighbor.Add(neighbor[i]);
            }
        }

        return siftedNeighbor;

    }

    List<int> AddNeighbor3D(int addedTile)
    {
        List<int> addedVals = new List<int>();

        int northCell = addedTile + columns;
        int southCell = addedTile - columns;
        int westCell = addedTile + 1;
        int eastCell = addedTile - 1;

        int highCell = addedTile + sliceSize;
        int lowCell = addedTile - sliceSize;

        // If west cell is less than AddedTiles, then it is in the next row
        if (westCell < totalTiles && (westCell % columns > addedTile % columns)) // westCell % rows > addedTile % rows
        {
            addedVals.Add(westCell);
        }
        // If west cell is less than AddedTiles, then it is in the next 
        if (eastCell >= 0 && (eastCell % columns < addedTile % columns)) // eastCell % rows < addedTile % rows
        {
            addedVals.Add(eastCell);
        }
        if (southCell >= 0 && (southCell % sliceSize < addedTile % sliceSize))
        {
            addedVals.Add(southCell);
        }
        if (northCell < totalTiles && (northCell % sliceSize > addedTile % sliceSize))
        {
            addedVals.Add(northCell);
        }

        if (lowCell >= 0)
        {
            addedVals.Add(lowCell);
        }
        if (highCell < totalTiles)
        {
            addedVals.Add(highCell);
        }


        return addedVals;
    }

    List<int> AddNeighbor(int addedTile)
    {
        List<int> addedVals = new List<int>();

        int northCell = addedTile + columns; //columns;
        int southCell = addedTile - columns; //columns;
        int westCell = addedTile+1;
        int eastCell = addedTile-1;

        int total = columns * rows;

        // If west cell is less than AddedTiles, then it is in the next row
        if (westCell < total && (westCell % columns > addedTile % columns)) // westCell % rows > addedTile % rows
        { 
            addedVals.Add(westCell);
        }
        // If west cell is less than AddedTiles, then it is in the next 
        if (eastCell >= 0 && (eastCell % columns < addedTile % columns)) // eastCell % rows < addedTile % rows
        {    
            addedVals.Add(eastCell);
        }
        if (southCell >= 0 && (southCell % sliceSize < addedTile % sliceSize))
        {
            addedVals.Add(southCell);
        }
        if (northCell < total && (northCell % sliceSize > addedTile % sliceSize))
        {
            addedVals.Add(northCell);
        }
        return addedVals;
    }

    // Default Test To see if the Maze Genration can generate a maze, without Prim's Algorithm
    List<int>[] Connection()
    {

        List<int>[] neighbors = new List<int>[totalTiles];

        // 2x2x2 maze
        neighbors[0] = new List<int> { 1 };
        neighbors[1] = new List<int> { 0, 5 };
        neighbors[2] = new List<int> { 3,6 };
        neighbors[3] = new List<int> { 2,7 };
        neighbors[4] = new List<int> { 5,6};
        neighbors[5] = new List<int> { 1,4,};
        neighbors[6] = new List<int> { 2,4 };
        neighbors[7] = new List<int> { 3 };

        // 2x3 maze
        //neighbors[0] = new List<int> { 1, 2 };
        //neighbors[1] = new List<int> { 0 };
        //neighbors[2] = new List<int> { 0, 3, 4 };
        //neighbors[3] = new List<int> { 2, 5 };
        //neighbors[4] = new List<int> { 2 };
        //neighbors[5] = new List<int> { 3 };
        
        // 3x3 maze
        //neighbors[0] = new List<int> { 1, 4 };
        //neighbors[1] = new List<int> { 0, 2, 4 };
        //neighbors[2] = new List<int> { 1 };
        //neighbors[3] = new List<int> { 6 };
        //neighbors[4] = new List<int> { 1,5,7};
        //neighbors[5] = new List<int> { 4,};
        //neighbors[6] = new List<int> { 3,7 };
        //neighbors[7] = new List<int> { 4,6,8 };
        //neighbors[8] = new List<int> { 7 };

        return neighbors;
    }

    bool[] GenTile3D(List<int> connect, int tileNum, int x, int z, int y)
    {
        //Walls = [Pos Z, Neg Z, Pos X, Neg X, Pos Y Neg Y]
        bool[] walls = { true, true, true, true,true,true };

        int numConn = connect.Count;
        int numByCol = tileNum % columns;
        int numByRow = tileNum % rows;
        int numByDepth = tileNum % sliceSize;

        // Top level Can see the sky
        if (tileNum/sliceSize == depth-1)
        {
            walls[4] = false;
        }

        //Debug.Log(tileNum);
        for (int i = 0; i < numConn; i++)
        {
            int testByCol = connect[i] % columns;
            int testByRow = connect[i] % rows;
            int testByDepth = connect[i] % sliceSize;

            if (numByDepth==testByDepth) {
                if (tileNum < connect[i])
                {
                    //Debug.Log("PosY Wall False");
                    walls[4] = false;
                }
                else
                {
                    //Debug.Log("NegY Wall False");
                    walls[5] = false;
                }
            }
            else { 
                if (numByCol == testByCol)
                {
                    if (tileNum > connect[i])
                    {
                        //Debug.Log("PosZ Wall False");
                        walls[0] = false;
                    }
                    else
                    {
                        //Debug.Log("NegZ Wall False");
                        walls[1] = false;
                    }
                }
                else
                {
                    if (numByCol > testByCol)
                    {
                        //Debug.Log("PosX Wall False");
                        walls[2] = false;
                    }
                    else
                    {
                        //Debug.Log("NegX Wall False");
                        walls[3] = false;
                    }
                }
            }
        }

        return walls;
    }


    bool[] GenTile(List<int> connect, int tileNum,int x, int z)
    {
        //Walls = [Pos Z, Neg Z, Pos X, Neg X]
        bool[] walls = { true, true, true, true };

        int numConn = connect.Count;
        int numByCol = tileNum % columns;
        int numByRow = tileNum % rows;

        //Debug.Log(tileNum);
        for (int i=0;i<numConn;i++)
        {
            int testByCol = connect[i] % columns;
            int testByRow = connect[i] % rows;

            if (numByCol == testByCol)
            {
                if (tileNum > connect[i])
                {
                    //Debug.Log("PosZ Wall False");
                    walls[0] = false;
                }
                else
                {
                    //Debug.Log("NegZ Wall False");
                    walls[1] = false;
                }
            }
            else
            {
                if (numByCol > testByCol)
                {
                    //Debug.Log("PosX Wall False");
                    walls[2] = false;
                }
                else
                {
                    //Debug.Log("NegX Wall False");
                    walls[3] = false;
                }
            }
        }

        return walls;
    }

    List<int> PathFinding(int startTile, int endTile, List<int>[] conn)
    {
        List<int> path = new List<int>();
        List<int> fringe = new List<int>();
        List<int> knownTiles = new List<int>();

        int[] parent = new int[totalTiles];
        parent[startTile] = -1; 

        fringe.Add(startTile);
        knownTiles.Add(startTile);

        Debug.Log("Fringe Loop");
        while (fringe.Count > 0)
        {

            int curTile = fringe[0];
            fringe.RemoveAt(0);

            if (curTile == endTile)
            {
                Debug.Log("Parent:" + parent[curTile] + " Child:"+ curTile);
                break;
            }

            List<int> addedFrontier = new List<int>();
            List<int> tileCon = conn[curTile];

            foreach (int x in tileCon)
            {
                Debug.Log("CheckTile:"+x);
                if (!knownTiles.Contains(x))
                {
                    Debug.Log("AddedTile:" + x);
                    addedFrontier.Add(x);
                }
            }
            foreach (int x in addedFrontier)
            {
                parent[x] = curTile;
            }
            knownTiles.AddRange(addedFrontier);

            fringe.InsertRange(0, addedFrontier);
        }

        path.Insert(0,endTile);
        int prevNode = parent[endTile];
        Debug.Log("Parent Loop");
        while (prevNode >= 0)
        {
            Debug.Log(prevNode);
            path.Insert(0, prevNode);
            prevNode = parent[prevNode];
        }
        return path;
    }

    void LoadMaze()
    {
        
        //List<int>[] mazeCon = Connection();
        List<int>[] mazeCon = Prims(0);
        //List<int> Path = PathFinding(0, sliceSize, MazeCon);

        int total = 0;
        for (int y =0; y<depth;y++)
        {
            for (int z = 0; z < rows; z++) // 
            {
                for (int x = 0; x < columns; x++) // 
                {
                    //string debugString = "Total:" + total + " x:" + x + " z:" + z;
                    //Debug.Log(debugString);

                    //Walls = [Pos Z, Neg Z, Pos X, Neg X]
                    bool[] wallsBool = GenTile3D(mazeCon[total], total, x, z,y);
                    // bool[] WallsBool = GenTile3D(MazeCon[total], total, x, z , y);


                    // Generate Base
                    GameObject obj;
                    obj = Instantiate(tiles[0], new Vector3(x * -5, y*3.5f, z * -5), Quaternion.identity);
                    obj.transform.parent = transform;

                    ////if (total==0 || total== totalTiles-1)
                    ////{
                    ////    break;
                    ////}

                    // k determines what wall side is being created
                    for (int k = 0; k < 6; k++)
                    {
                        // wallsBool determine if a wall exists or not. wallsBool[k] is true means a wall exists there.  

                        // total != 0 && total != sliceSize-1 is used to determine exit and entrance. Both are on the same leval and in opposite corners
                        if (wallsBool[k] && (total != 0 || k == 5) && (total != sliceSize-1 || k == 5)) { 
                            // To generate the Wall.
                            GameObject wallObj;

                            // Code for generating the random Torches
                            // This is the 1 in n chance in where n is the upper value of Random.Range. 
                            int torchAdd = Random.Range(0, 10);
                            GameObject torchObj;
                            // Generete the center value of Position and Rotation for the Objects
                            Vector3 torchPos = new Vector3(x * -5, 2f + y * 3.5f, z * -5);
                            Quaternion torchRot = Quaternion.Euler(0, 0, 0);
                            Vector3 wallPos = new Vector3(x * -5, 1.75f+y*3.5f, z * -5);

                            // Determine the offsets for position and rotation based on the wall being generated
                            switch (k)
                            {
                                case 0:
                                    torchRot = Quaternion.Euler(0, 180, 0); // The torch is rotated based on the wall
                                    torchPos = torchPos + new Vector3(0, 0, 1.9f);
                                    wallPos = wallPos + new Vector3(0, 0, 2.25f);
                                    break;
                                case 1:
                                    torchRot = Quaternion.Euler(0, 0, 0);
                                    torchPos = torchPos + new Vector3(0, 0, -1.9f);
                                    wallPos = wallPos + new Vector3(0, 0, -2.25f);
                                    break;
                                case 2:
                                    torchRot = Quaternion.Euler(0, 270, 0);
                                    torchPos = torchPos + new Vector3(1.9f, 0, 0);
                                    wallPos = wallPos + new Vector3(2.25f, 0, 0);
                                    break;
                                case 3:
                                    torchRot = Quaternion.Euler(0, 90, 0);
                                    torchPos = torchPos + new Vector3(-1.9f, 0, 0);
                                    wallPos = wallPos + new Vector3(-2.25f, 0, 0);
                                    break;
                                case 4:
                                    torchAdd = 0;
                                    wallPos = wallPos + new Vector3(0, 1.375f, 0);
                                    break;
                                case 5:
                                    torchAdd = 0;
                                    wallPos = wallPos + new Vector3(0, -1.75f, 0);
                                    break;
                            }
                            // Determine to generate the torch
                            if (torchAdd== k)
                            {
                                torchObj = Instantiate(tiles[7], torchPos, torchRot);
                                torchObj.transform.parent = transform;
                            }
                            wallObj = Instantiate(tiles[k + 1], wallPos, Quaternion.identity);
                            wallObj.transform.parent = transform;
                        }
                    }
                    total= total+1;
                }
            }
        }

    }
}
