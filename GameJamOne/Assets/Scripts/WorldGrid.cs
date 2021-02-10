using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid {
    private int width;
    public int Width { get { return width; } }
    private int height;
    public int Height { get { return height; } }

    private int startX;
    private int startY;
    private int endX;
    private int endY;

    private Pathnode[,] grid;

    public WorldGrid(int width, int height, int startX, int startY, int endX, int endY) {
        this.width = width;
        this.height = height;

        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;

        grid = new Pathnode[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y] = new Pathnode(x, y);
            }
        }

        //Sets walls of the grid
        for (int x = 0; x < width; x++)
            grid[x,0] = new Pathnode(x, 0, true);
        for (int x = 0; x < width; x++)
            grid[x, height - 1] = new Pathnode(x, height - 1, true);
        for (int y = 0; y < height; y++)
            grid[0, y] = new Pathnode(0, y, true);
        for (int y = 0; y < height; y++)
            grid[width - 1, y] = new Pathnode(width - 1, y, true);
    }

    public Pathnode GetCell(int posX, int posY, bool update = false) {
        return grid[posX, posY];
    }

    public List<Pathnode> GetCells(int posX, int posY, List<Pathnode> cells) {
        for (int x = posX - 1; x <= posX + 1; x++) {
            for (int y = posY - 1; y <= posY + 1; y++) {
                if (x != startX || y != startY) {
                    Pathnode temp = GetCell(x, y);
                    if (x != posX || y != posY) {
                        if (!temp.IsWall) {
                            if (!temp.closed && !cells.Contains(temp)) {
                                cells.Add(temp);
                                SetH(x, y);
                            }
                            SetG(x, y, GetCell(posX, posY));
                        }
                    }
                    else grid[x, y].closed = true;
                }
            }
        }

        return cells;
    }

    public void SetCell(int x, int y, bool available) {
        grid[x, y] = new Pathnode(x, y, available);
    }
    public void SetCell(int x, int y, Pathnode lastNode, bool available) {
        grid[x, y] = new Pathnode(x, y, lastNode, available);
    }

    public void SetH(int x, int y) {
        int checkerX = x;
        int checkerY = y;
        int h = 0;
        int xDif;
        int yDif;

        while (checkerX != endX || checkerY != endY) {
            xDif = Mathf.Abs(endX - checkerX);
            yDif = Mathf.Abs(endY - checkerY);

            if (xDif > yDif) {
                if (checkerX < endX) checkerX++;
                else checkerX--;
                h += 10;
            }
            else if (xDif < yDif) {
                if (checkerY < endY) checkerY++;
                else checkerY--;
                h += 10;
            }
            else {
                if (checkerX < endX) checkerX++;
                else checkerX--;
                if (checkerY < endY) checkerY++;
                else checkerY--;
                h += 14;
            }
        }

        grid[x, y].H = h;
    }

    //Sets the G and the Last Cell for a specified cell
    public void SetG(int x, int y, Pathnode checkingNode) {
        Pathnode checker;
        int g = 0;

        if (!grid[x, y].beenChecked) {
            grid[x, y].lastNode = checkingNode;
            checker = grid[x, y];
        }
        else {
            checker = new Pathnode(x, y, checkingNode);
        }

        //Calculates G until we get back to the start coordinates
        while (checker.x != startX || checker.y !=  startY) {
            if (checker.x == checker.lastNode.x || checker.y == checker.lastNode.y) {
                g += 10;
            }
            else {
                g += 14;
            }
            
            checker = checker.lastNode;
        }

        if (!grid[x, y].beenChecked) {
            grid[x, y].G = g;
            grid[x, y].beenChecked = true;
        }
        else if (grid[x, y].G > g) {
            grid[x, y].G = g;
            grid[x, y].lastNode = checkingNode;
        }
    }
}
