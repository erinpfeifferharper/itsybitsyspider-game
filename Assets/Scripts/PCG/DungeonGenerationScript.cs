using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerationScript : MonoBehaviour
{
    public class Cell
    {
        public bool visited;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x,  int y)
        {
            // 0 - cannot spawn, 1 - can spawn, 2 - Has to spawn

            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }
    }

    public Vector2Int size;
    public int startPosition = 0;
    public Rule[] rooms;
    public Vector2 offset;

    List<Cell> board;

    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i + j * size.x)];
                if(currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if (p == 2)
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomGenerationScript>();
                    newRoom.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j;
                }
                
            }
        }

    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while(k < 1000)
        {
            k++;

            board[currentCell].visited = true;

            if(currentCell == board.Count-1)
            {
                break;
            }

            //Check adjacent cells
            List<int> neighbours = CheckNeigbours(currentCell);

            if(neighbours.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if(newCell > currentCell)//down or right
                {
                    if(newCell - 1 == currentCell) //right
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;//left
                    }   
                    else //down
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }

                }
                else //up or left
                {
                    if (newCell + 1 == currentCell) //left
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;//right
                    }
                    else //up
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }

        }

        GenerateDungeon();

    }

    List<int> CheckNeigbours(int cell)
    {
        List<int> neighbours = new List<int>();

        //check up
        if(cell - size.x >= 0 && !board[(cell-size.x)].visited)
        {
            neighbours.Add((cell - size.x));
        }
        //check down
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbours.Add((cell + size.x));
        }
        //check right
        if ((cell+1) % size.x != 0 && !board[(cell + 1)].visited)
        {
            neighbours.Add((cell + 1));
        }
        //check left
        if ((cell) % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbours.Add((cell - 1));
        }

        return neighbours;
    }
}
