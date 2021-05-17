using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
    public enum WallState{
        //0000 No hay paredes
        //1111 Abajo,Arriba,Derecha,Izquierda
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        visited = 128 //1000 0000

        
    }

public struct Position{
    public int x;
    public int y;
}
public struct Neighbour{
    public Position pos;
    public WallState SharedWall;
}
    

public static class MazeGenerator
{
    public static int[,] Distance;
    public static Position[,] Previous;
    private static WallState GetOppositeWall(WallState wall){
        switch(wall){
            case WallState.Right : return WallState.Left;
            case WallState.Left : return WallState.Right;
            case WallState.Up : return WallState.Down;
            case WallState.Down : return WallState.Up;
            default: return WallState.Left;
        }
    }
    private static WallState[,] RecursiveBackTracker(WallState[,] maze, int width , int height){
        var rand = new System.Random(/*seed*/);
        Stack<Position> pila = new Stack<Position>();
        //var pos = new Position { x = rand.Next(0,width), y = rand.Next(0, height)};
        var pos = new Position { x = 0, y = 0};
        maze[pos.x,pos.y] |= WallState.visited;
        Distance[pos.x,pos.y] = 0;
        Previous[pos.x,pos.y] = new Position{x = -1,y = -1};
        pila.Push(pos);
        while(pila.Count > 0){
            Position current = pila.Pop();
            List<Neighbour> vecinos = GetUnvisitedNeighbours(current, maze , width , height);
            if(vecinos.Count > 0){
                pila.Push(current);
                var randIndex = rand.Next(0,vecinos.Count);
                var randVecino = vecinos[randIndex];
                var vecinoPos = randVecino.pos;
                maze[current.x,current.y] &= ~randVecino.SharedWall;
                maze[randVecino.pos.x, randVecino.pos.y] &= ~ GetOppositeWall(randVecino.SharedWall);
                maze[randVecino.pos.x , randVecino.pos.y] |= (WallState.visited);
                Distance[randVecino.pos.x,randVecino.pos.y] = Distance[current.x,current.y] + 1;
                Previous[randVecino.pos.x,randVecino.pos.y] = new Position{x = current.x,y = current.y};
                pila.Push(randVecino.pos);
            }
        }
        return maze;
    }
    private static List<Neighbour> GetUnvisitedNeighbours(Position p , WallState[,]maze, int width , int height){
        var list = new List<Neighbour>();
        if(p.x > 0){
            if(!maze[p.x - 1 , p.y].HasFlag(WallState.visited)){
                list.Add(new Neighbour{
                    pos = new Position{
                        x = p.x - 1,
                        y = p.y
                    },
                    SharedWall = WallState.Left
                });
            } 
        }
        if(p.y > 0){
            if(!maze[p.x  , p.y- 1].HasFlag(WallState.visited)){
                list.Add(new Neighbour{
                    pos = new Position{
                        x = p.x ,
                        y = p.y- 1
                    },
                    SharedWall = WallState.Down
                });
            } 
        }
        if(p.y < height - 1){
            if(!maze[p.x  , p.y + 1].HasFlag(WallState.visited)){
                list.Add(new Neighbour{
                    pos = new Position{
                        x = p.x ,
                        y = p.y + 1
                    },
                    SharedWall = WallState.Up
                });
            } 
        }
        if(p.x < width - 1){
            if(!maze[p.x + 1 , p.y].HasFlag(WallState.visited)){
                list.Add(new Neighbour{
                    pos = new Position{
                        x = p.x + 1,
                        y = p.y
                    },
                    SharedWall = WallState.Right
                });
            } 
        }
        return list;
    }
    public static WallState[,] Generate(int width, int height){
        WallState[,] maze = new WallState[width+1,height+1];
        Distance = new int[width+1,height+1];
        Previous = new Position[width+1,height+1];
        WallState initial = WallState.Right | WallState.Left | WallState.Down | WallState.Up;
        for(int i = 0 ; i < width ; i++){
            for(int j = 0 ; j < height ; j++){
                maze[i,j] = initial;
                Distance[i,j] = -1;
            }
        }
        return RecursiveBackTracker(maze,width,height);
    }
}
