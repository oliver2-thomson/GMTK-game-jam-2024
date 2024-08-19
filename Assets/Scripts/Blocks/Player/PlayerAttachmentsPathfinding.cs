using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttachment : MonoBehaviour
{
    private bool WillRemovingThisLeaveSurroundingBlocksOnTheirOwn(Vector2Int blockRemoved)
    {
        Vector2Int[] directions = new Vector2Int[4] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };


        BaseBlock[,] hypotheticalBlockList = new BaseBlock[BlockList.GetLength(0), BlockList.GetLength(1)];
        //Copy hypothetical
        for (int x = 0; x < BlockList.GetLength(0); x++)
        {
            for (int y = 0; y < BlockList.GetLength(1); y++)
            {
                hypotheticalBlockList[x, y] = BlockList[x, y];
            }
        }


        hypotheticalBlockList[blockRemoved.x, blockRemoved.y] = null;

        foreach (Vector2Int direction in directions)
        {
            if (CheckBlockExists(blockRemoved + direction))
            {
                HashSet<Vector2Int> dump;
                if (!FindPath(blockRemoved + direction, brain.localGridPosition, hypotheticalBlockList, out dump))
                {
                    Debug.Log("INVALID!");
                    return false;
                }
            }
        }
        return true;
    }

    private bool WillRemovingThisLeaveSurroundingBlocksOnTheirOwn(Vector2Int blockRemoved, out HashSet<Vector2Int> effectedBlocks)
    {
        Vector2Int[] directions = new Vector2Int[4] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };


        BaseBlock[,] hypotheticalBlockList = new BaseBlock[BlockList.GetLength(0), BlockList.GetLength(1)];
        //Copy hypothetical
        for (int x = 0; x < BlockList.GetLength(0); x++)
        {
            for (int y = 0; y < BlockList.GetLength(1); y++)
            {
                hypotheticalBlockList[x, y] = BlockList[x, y];
            }
        }


        hypotheticalBlockList[blockRemoved.x, blockRemoved.y] = null;

        HashSet<Vector2Int> totalBlockPositionsEffected = new HashSet<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            if (CheckBlockExists(blockRemoved + direction))
            {
                HashSet<Vector2Int> dump;

                if (!FindPath(blockRemoved + direction, brain.localGridPosition, hypotheticalBlockList, out dump))
                {
                    //No path! Find all blocks effected!

                    foreach(Vector2Int block in dump) 
                    {
                        if (!totalBlockPositionsEffected.Contains(block)) 
                        {
                            totalBlockPositionsEffected.Add(block);
                        }
                    }
                }
            }
        }

        effectedBlocks = totalBlockPositionsEffected;
        return totalBlockPositionsEffected.Count > 0;
    }

    public bool FindPath(Vector2Int start, Vector2Int goal, BaseBlock[,] BlockGrid, out HashSet<Vector2Int> attachedNodes)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal)
            {
                attachedNodes = visited;
                return true;
            }

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

        attachedNodes = visited;
        return false; // No path found
    }

    bool IsWall(Vector2Int pos, BaseBlock[,] BlockGrid)
    {
        return BlockGrid[pos.x, pos.y] == null;
    }
}
