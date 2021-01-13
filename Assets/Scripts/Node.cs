using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Node
    {
        public Node Parent;
        public Vector2 Position;
        public float GCost;
        public float HCost;
        public float FCost => GCost + HCost;
        public bool IsStartPoint => (Parent == null);
        public bool NodeCompleted;

        public bool Walkable;
        public Vector3 WorldPosition;

        public Node(Vector2 position, float gCost, float hCost, Node parent)
        {
            Position = position;
            this.GCost = gCost;
            HCost = hCost;
            Parent = parent;
        }

        public Node (bool walkable, Vector3 worldPos)
        {
            Walkable = walkable;
            WorldPosition = worldPos;
        }
    }
}
