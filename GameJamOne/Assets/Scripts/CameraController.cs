using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //Debugging of the pathfinding algorithm
    //public Canvas canvas;
    //public Text text;
    public GameObject emptySquare;
    public GameObject filledSquare;
    public GameObject startSquare;
    public GameObject endSquare;
    //public GameObject closedSquare;
    public GameObject currentSquare;
    //public GameObject openSquare;
    private GameObject[] debugSquares;
    //private GameObject[] debugText;
    private float timer;
    public float timerDelay;
    private int currentStep = 0;
    //Also temporary
    public int gridWidth;
    public int gridHeight;
    public int startX;
    public int startY;
    public int endX;
    public int endY;

    private WorldGrid grid;
    private Pathfinder pathFinder;
    private List<Vector2> path;

    public Vector2 offset;
    public float cellSize;

    void Start() {
        grid = new WorldGrid(gridWidth, gridHeight, startX, startY, endX, endY);
        pathFinder = new Pathfinder(grid);
        path = pathFinder.FindPath();

        timer = timerDelay;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && currentStep < path.Count) {
            RenderCells(currentStep);
            currentStep++;
        }
    }

    void FixedUpdate() {
        /*if (currentStep < path.Count) {
            if (timer > timerDelay) {
                RenderCells(currentStep);
                currentStep++;

                timer = 0;
            }
            else timer += Time.deltaTime;
        }*/
    }

    private void RenderCells(int currentStep) {
        debugSquares = GameObject.FindGameObjectsWithTag("Square");
        //debugText = GameObject.FindGameObjectsWithTag("Text");

        for (int i = 0; i < debugSquares.Length; i++)
            Destroy(debugSquares[i]);
        //for (int i = 0; i < debugText.Length; i++)
            //Destroy(debugText[i]);

        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                if (x == path[currentStep].x && y == path[currentStep].y) {
                    Instantiate(currentSquare, new Vector3(path[currentStep].x * cellSize + offset.x, path[currentStep].y * cellSize + offset.y, 0), Quaternion.identity);
                }
                else if (x == startX && y == startY) {
                    Instantiate(startSquare, new Vector3(x * cellSize + offset.x, y * cellSize + offset.y, 0), Quaternion.identity);
                }
                else if (x == endX && y == endY) {
                    Instantiate(endSquare, new Vector3(x * cellSize + offset.x, y * cellSize + offset.y, 0), Quaternion.identity);
                }
                else if (grid.GetCell(x, y).IsWall) {
                    Instantiate(filledSquare, new Vector3(x * cellSize + offset.x, y * cellSize + offset.y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(emptySquare, new Vector3(x * cellSize + offset.x, y * cellSize + offset.y, 0), Quaternion.identity);
                }
            }
        }

        /*for (int x = 0; x < gridWidth; x++) {
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
        }*/
    }

    /*private void RenderText(int x, int y) {
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
    }*/
}
