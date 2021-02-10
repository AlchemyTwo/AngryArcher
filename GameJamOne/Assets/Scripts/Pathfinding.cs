using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour {
    private WorldGrid grid;
    private List<Pathnode> openCells = new List<Pathnode>();
    private List<Pathnode> path = new List<Pathnode>();
    private Pathnode current;

    public Canvas canvas;
    public Text text;
    public GameObject emptySquare;
    public GameObject filledSquare;
    public GameObject startSquare;
    public GameObject endSquare;
    public GameObject closedSquare;
    public GameObject currentSquare;
    public GameObject openSquare;

    public int startX;
    public int startY;
    public int endX;
    public int endY;

    private GameObject[] debugSquares;
    private GameObject[] debugText;
    private int status = 0;
    private int pathStep = 0;
    private float timer = 0;
    public float delay;

    public int gridWidth;
    public int gridHeight;

    private void Start() {
        grid = new WorldGrid(gridWidth, gridHeight, startX, startY, endX, endY);

        //Sets filled cells inside the grid
        grid.SetCell(2, 2, true);
        grid.SetCell(2, 3, true);
        grid.SetCell(2, 4, true);
        grid.SetCell(3, 3, true);
        grid.SetCell(4, 3, true);
        grid.SetCell(5, 3, true);
        grid.SetCell(6, 3, true);

        grid.SetCell(7, 3, true);
        grid.SetCell(7, 2, true);
        grid.SetCell(7, 1, true);

        grid.SetCell(5, 5, true);
        grid.SetCell(5, 6, true);

        grid.SetCell(6, 5, true);
        grid.SetCell(7, 5, true);
        grid.SetCell(8, 5, true);
        grid.SetCell(9, 5, true);

        grid.SetCell(9, 4, true);
        grid.SetCell(9, 3, true);
        grid.SetCell(9, 2, true);

        //Initial setup
        current = grid.GetCell(startX, startY);
        openCells = grid.GetCells(startX, startY, openCells);

        RenderCells();
    }

    private void FixedUpdate() {
        if (status != 2) {
            if (timer < delay) {
                timer += Time.deltaTime;
            }
            else {
                //Pathfinding logic
                if (current != grid.GetCell(endX, endY) && status == 0) {
                    //Gets new best cell
                    Pathnode checker = new Pathnode(0, 0, 10000, 10000);
                    for (int i = 0; i < openCells.Count; i++) {
                        if (openCells[i].F < checker.F) {
                            checker = openCells[i];
                        }
                        else if (openCells[i].F == checker.F && openCells[i].H < checker.H) {
                            checker = openCells[i];
                        }
                    }

                    current = checker;
                    openCells.Remove(current);
                    openCells = grid.GetCells(current.x, current.y, openCells);

                    if (current == grid.GetCell(endX, endY)) {
                        do {
                            path.Insert(0, current);
                            current = current.lastNode;
                        }
                        while (current != grid.GetCell(startX, startY));

                        path.Insert(0, current);
                        //openCells.Clear();
                        status = 1;
                    }
                }

                if (status == 1) {
                    current = path[pathStep];
                    pathStep++;
                    if (pathStep == path.Count) status = 2;
                }

                RenderCells();
                timer = 0;
            }
        }
    }

    private void RenderCells() {
        debugSquares = GameObject.FindGameObjectsWithTag("Square");
        debugText = GameObject.FindGameObjectsWithTag("Text");

        for (int i = 0; i < debugSquares.Length; i++)
            Destroy(debugSquares[i]);
        for (int i = 0; i < debugText.Length; i++)
            Destroy(debugText[i]);

        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                if (x == current.x && y == current.y) {
                    Instantiate(currentSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                    RenderText(x, y);
                }
                else if (x == startX && y == startY) {
                    Instantiate(startSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                }
                else if (x == endX && y == endY) {
                    Instantiate(endSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                }
                else if (grid.GetCell(x, y).IsWall) {
                    Instantiate(filledSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                }
                else if (grid.GetCell(x, y).closed) {
                    Instantiate(closedSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                    RenderText(x, y);
                }
                else if (openCells.Contains(grid.GetCell(x, y))) {
                    Instantiate(openSquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                    RenderText(x, y);
                }
                else {
                    Instantiate(emptySquare, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
                }
            }
        }
    }

    private void RenderText(int x, int y) {
        Pathnode cell = grid.GetCell(x, y);

        Text temp1 = Instantiate(text, new Vector3(x - 6, y - 3, 0), Quaternion.identity);
        temp1.transform.SetParent(canvas.transform, true);
        temp1.text = cell.F.ToString();

        Text temp2 = Instantiate(text, new Vector3(x - 6.3f, y - 2.7f, 0), Quaternion.identity);
        temp2.transform.SetParent(canvas.transform, true);
        temp2.text = cell.G.ToString();

        Text temp3 = Instantiate(text, new Vector3(x - 5.7f, y - 3.3f, 0), Quaternion.identity);
        temp3.transform.SetParent(canvas.transform, true);
        temp3.text = cell.H.ToString();
    }
}

public class Pathnode {
    private bool isWall;
    public bool IsWall { get { return isWall; } }
    public bool closed;
    public bool beenChecked;
    public int x;
    public int y;

    //Walking distance from start (may change throughout the pathfinding process)
    private int g;
    public int G {
        get { return g; }
        set {
            g = value;
            f = g + h;
        }
    }
    //Walking distance to the end
    private int h;
    public int H {
        get { return h; }
        set {
            h = value;
            f = g + h;
        }
    }
    //Sum of the two (Updates automatically
    private int f;
    public int F {
        get { return f; }
    }

    public Pathnode lastNode;

    //Constructor for setting coords and isWall (false by default)
    public Pathnode(int x, int y, bool isWall = false) {
        this.x = x;
        this.y = y;
        this.beenChecked = false;
        this.isWall = isWall;
        if (!isWall) this.closed = false;
    }
    //Constructor for setting coords, lastNode and isWall (false by default)
    public Pathnode(int x, int y, Pathnode lastNode, bool isWall = false) {
        this.x = x;
        this.y = y;
        this.g = 10000;
        this.beenChecked = false;
        this.isWall = isWall;
        if (!isWall) this.closed = false;
        this.lastNode = lastNode;
    }
    //Constructor for setting coords, G, H and isWall (false by default)
    public Pathnode(int x, int y, int g, int h, bool isWall = false) {
        this.x = x;
        this.y = y;
        this.beenChecked = false;
        this.isWall = isWall;
        if (!isWall) this.closed = false;
        this.g = g;
        this.h = h;
        this.f = g + h;
    }

    public void SetCell(int g, int h, int f, Pathnode lastNode) {
        this.lastNode = lastNode;
    }

    public string Debug(int x, int y) {
        return (x.ToString() + ", " + y.ToString());
    }
}