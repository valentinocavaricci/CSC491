using System.Collections.Generic;
using UnityEngine;

public class MinecraftAi : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;
    public float chaseRange = 12f;
    public float stopDistance = 0.15f;
    public float repathTime = 0.5f;

    public LayerMask obstacleLayer;

    public int worldWidth = 40;
    public int worldLength = 40;
    public int maxIterations = 500;

    private List<Vector3> path = new List<Vector3>();
    private int currentPathIndex = 0;
    private float repathTimer = 0f;

    private class Node
    {
        public Vector3 position;
        public float gCost;
        public float hCost;
        public Node parent;

        public float fCost
        {
            get { return gCost + hCost; }
        }

        public Node(Vector3 position)
        {
            this.position = position;
            gCost = 999999f;
            hCost = 0f;
            parent = null;
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("No player assigned");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            repathTimer -= Time.deltaTime;

            if (repathTimer <= 0f)
            {
                Debug.Log("Trying to build path");
                BuildAStarPath();
                repathTimer = repathTime;
            }

            FollowPath();
        }
    }

    void BuildAStarPath()
    {
        path.Clear();

        Vector3 startPos = GetGridPosition(transform.position);
        Vector3 targetPos = GetGridPosition(player.position);

        Debug.Log("START = " + startPos);
        Debug.Log("TARGET = " + targetPos);
        Debug.Log("START BLOCKED = " + IsBlocked(startPos));
        Debug.Log("TARGET BLOCKED = " + IsBlocked(targetPos));
        Debug.Log("START INSIDE WORLD = " + IsInsideWorld(startPos));
        Debug.Log("TARGET INSIDE WORLD = " + IsInsideWorld(targetPos));

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = new Node(startPos);
        startNode.gCost = 0f;
        startNode.hCost = GetDistance(startPos, targetPos);
        openList.Add(startNode);

        int iterations = 0;

        while (openList.Count > 0 && iterations < maxIterations)
        {
            iterations++;

            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost ||
                    (Mathf.Approximately(openList[i].fCost, currentNode.fCost) && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.position == targetPos)
            {
                Debug.Log("PATH FOUND");
                RetracePath(startNode, currentNode);
                Debug.Log("FINAL PATH COUNT = " + path.Count);
                return;
            }

            List<Vector3> neighbors = GetNeighbors(currentNode.position);

            foreach (Vector3 neighborPos in neighbors)
            {
                if (!IsInsideWorld(neighborPos))
                    continue;

                if (IsBlocked(neighborPos))
                    continue;

                if (PositionInList(closedList, neighborPos))
                    continue;

                float newCostToNeighbor = currentNode.gCost + 1f;

                Node neighborNode = GetNodeFromList(openList, neighborPos);

                if (neighborNode == null)
                {
                    neighborNode = new Node(neighborPos);
                    neighborNode.gCost = newCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborPos, targetPos);
                    neighborNode.parent = currentNode;
                    openList.Add(neighborNode);
                }
                else if (newCostToNeighbor < neighborNode.gCost)
                {
                    neighborNode.gCost = newCostToNeighbor;
                    neighborNode.parent = currentNode;
                }
            }
        }

        Debug.Log("NO PATH FOUND");
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> newPath = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode.position);
            currentNode = currentNode.parent;

            if (currentNode == null)
                break;
        }

        newPath.Reverse();
        path = newPath;
        currentPathIndex = 0;
    }

    bool PositionInList(List<Node> list, Vector3 position)
    {
        foreach (Node node in list)
        {
            if (node.position == position)
                return true;
        }

        return false;
    }

    Node GetNodeFromList(List<Node> list, Vector3 position)
    {
        foreach (Node node in list)
        {
            if (node.position == position)
                return node;
        }

        return null;
    }

    float GetDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    bool IsBlocked(Vector3 point)
    {
        Vector3 checkCenter = new Vector3(point.x, 1f, point.z);
        Vector3 halfExtents = new Vector3(0.4f, 0.4f, 0.4f);
        return Physics.CheckBox(checkCenter, halfExtents, Quaternion.identity, obstacleLayer);
    }

    bool IsInsideWorld(Vector3 point)
    {
        if (point.x < 0 || point.x >= worldWidth)
            return false;

        if (point.z < 0 || point.z >= worldLength)
            return false;

        return true;
    }

    void FollowPath()
    {
        if (currentPathIndex >= path.Count)
            return;

        Vector3 targetPoint = path[currentPathIndex];
        Vector3 moveTarget = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);

        Vector3 direction = moveTarget - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= stopDistance)
        {
            currentPathIndex++;
            return;
        }

        direction = direction.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    Vector3 GetGridPosition(Vector3 position)
    {
        float x = Mathf.Round(position.x);
        float z = Mathf.Round(position.z);

        return new Vector3(x, 0f, z);
    }

    List<Vector3> GetNeighbors(Vector3 position)
    {
        List<Vector3> neighbors = new List<Vector3>();

        neighbors.Add(position + new Vector3(1f, 0f, 0f));
        neighbors.Add(position + new Vector3(-1f, 0f, 0f));
        neighbors.Add(position + new Vector3(0f, 0f, 1f));
        neighbors.Add(position + new Vector3(0f, 0f, -1f));

        return neighbors;
    }
}