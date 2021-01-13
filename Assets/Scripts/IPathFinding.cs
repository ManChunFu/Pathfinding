using System.Collections.Generic;
using UnityEngine;

public interface IPathFinding
{
    List<Vector3> Path { get; set; }
    bool PathCompleted { get; set; }
}