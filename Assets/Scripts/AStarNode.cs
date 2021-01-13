using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AStarNode
    {
        public List<AStarNode> Parent = new List<AStarNode>();
        public Vector2 Position;
        public float GCost;
        public float HCost;
        public float FCost => GCost + HCost;


        public bool IsStartPoint;
        public bool NodeCompleted;

        public AStarNode(Vector2 position, float hCost, AStarNode parent)
        {
            if (parent != null)
            {
                Parent.Add(parent);
                var difference = parent.Position - position;
                GCost = Mathf.Abs(difference.x) == 1 && Mathf.Abs(difference.y) == 1 ? 1.4f : 1.0f;
            }
            else
            {
                IsStartPoint = true;
                GCost = 0;
            }

            Position = position;
            HCost = hCost;

        }
    }
}
