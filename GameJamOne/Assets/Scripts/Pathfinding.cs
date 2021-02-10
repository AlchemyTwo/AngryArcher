using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {
    private WorldGrid grid;
    private List<Pathnode> openCells = new List<Pathnode>();
    private Pathnode current;

    public Pathfinder(WorldGrid grid) {
        this.grid = grid;
    }

    public List<Vector2> FindPath() {
        //Initial setup. Gets start cell and the open cells around it
        current = grid.GetStartCell();
        openCells = grid.GetCells(grid.StartX, grid.StartY, openCells);

        while (current != grid.GetEndCell()) {
            //Gets new best cell
            Pathnode checker = new Pathnode(0, 0);

            for (int i = 0; i < openCells.Count; i++) {
                if (openCells[i].F < checker.F || !checker.beenChecked) {
                    checker = openCells[i];
                }
                else if (openCells[i].F == checker.F && openCells[i].H < checker.H) {
                    checker = openCells[i];
                }
            }

            current = checker;

            //Updates open cells
            openCells.Remove(current);
            openCells = grid.GetCells(current.x, current.y, openCells);
        }

        //Writes down the path into our output array
        List<Vector2> path = new List<Vector2>();

        do {
            path.Insert(0, new Vector2(current.x, current.y));
            current = current.lastNode;
        }
        while (current != grid.GetStartCell());
        path.Insert(0, new Vector2(current.x, current.y));

        return path;
    }
}

public class Pathnode {
    public Pathnode lastNode;

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

    //Constructor for setting coords and isWall (false by default)
    public Pathnode(int x, int y, bool isWall = false) {
        this.x = x;
        this.y = y;
        this.f = 0;
        this.beenChecked = false;
        this.isWall = isWall;
        if (!isWall) this.closed = false;
    }
    //Constructor for setting coords, lastNode and isWall (false by default)
    public Pathnode(int x, int y, Pathnode lastNode, bool isWall = false) {
        this.x = x;
        this.y = y;
        this.f = 0;
        this.beenChecked = false;
        this.isWall = isWall;
        if (!isWall) this.closed = false;

        this.lastNode = lastNode;
    }
}