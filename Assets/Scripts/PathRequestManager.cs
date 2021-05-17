using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest CurrentPathRequest;
    static PathRequestManager instance;
    AStarPathFinding pathFinding;
    bool IsProssesingAPath;
    private void Awake() {
        instance = this;
        pathFinding = GetComponent<AStarPathFinding>();
    }
    public static void RequestPath(Vector2 pathStart , Vector2 PathEnd , Action<Vector2[],bool> callback){
        PathRequest newRequest = new PathRequest(pathStart,PathEnd,callback);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext(){
        if(!IsProssesingAPath && pathRequestsQueue.Count > 0){
            CurrentPathRequest = pathRequestsQueue.Dequeue();
            IsProssesingAPath = true;
            pathFinding.StartFindPath(CurrentPathRequest.pathStart,CurrentPathRequest.pathEnd);
        }
    }
    public void FinishProssesingPath(Vector2[] path , bool success){
        CurrentPathRequest.callback(path,success);
        IsProssesingAPath = false;
        TryProcessNext();
    }
    struct PathRequest{
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[],bool> callback;
        public PathRequest(Vector2 _start , Vector2 _end , Action<Vector2[],bool> _callback){
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
