using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttachment : MonoBehaviour
{
    public bool FindPath(Vector2Int start, Vector2Int goal, BaseBlock[,] BlockGrid)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal) return true;

            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = current + dir;
                if (!visited.Contains(neighbor) && !IsWall(neighbor, BlockGrid))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }
        return false; // No path found
    }

    bool IsWall(Vector2Int pos, BaseBlock[,] BlockGrid)
    {
        return BlockGrid[pos.x, pos.y] == null;
    }
}
