using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGL;

public class Graph : MonoBehaviour
{
    public float nodeRadius = 1f;
    public LayerMask unwalkableMask;

    public Node[,] nodes;

    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    private Vector3 scale;
    private Vector3 halfScale;

    private List<Node> path;


    // Use this for initialization
    void Start()
    {
        CreateGrid();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        // Check if nodes have been created
        if (nodes != null)
        {
            // Loop through all nodes
            for(int x = 0; x < nodes.GetLength(0); x++)
            {
                for(int z = 0; z < nodes.GetLength(1); z++)
                {
                    // Get the node and store it in variable
                    Node node = nodes[x, z];
    
                    Gizmos.color = node.walkable ? new Color(0, 0, 1, 0.5f) : 
                                                   new Color(1, 0, 0, 0.5f);
    
                    if(path != null && path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                    
                    // Draw a sphere to represent the node
                    Gizmos.DrawSphere(node.position, nodeRadius);
                }
            }
        }
    }

    private void Update()
    {
        UpdateWalkables();
    }

    // Generates a 2D grid on the X and Z axis
    public void CreateGrid()
    {
        // Calculate the node diameter
        nodeDiameter = nodeRadius * 2f;

        // Get transform's scale
        scale = transform.localScale;

        // Half the scale
        halfScale = scale / 2f;

        // Calculate grid size in (int) form
        gridSizeX = Mathf.RoundToInt(scale.x / nodeDiameter); // OR halfScale.x / nodeRadius
        gridSizeZ = Mathf.RoundToInt(scale.z / nodeDiameter);

        // Create a grid of that size
        nodes = new Node[gridSizeX, gridSizeZ];

        // Get the bottom left point of the position
        Vector3 bottomLeft = transform.position - Vector3.right * halfScale.x
                                                - Vector3.forward * halfScale.z;

        // Loop throgh all nodes in grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                // Calculate offset for x and z
                float xOffset = x * nodeDiameter + nodeRadius;
                float zOffset = z * nodeDiameter + nodeRadius;
                // Create position using offsets
                Vector3 nodePoint = bottomLeft + Vector3.right * xOffset
                                               + Vector3.forward * zOffset;
                // Use Physics to check if node collided with non-walkable object
                bool walkable = !Physics.CheckSphere(nodePoint, nodeRadius, unwalkableMask);
                // Create the node and put it in the 2D array
                nodes[x, z] = new Node(walkable, nodePoint, x, z);
            }
        }
    }

    public void UpdateWalkables()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Node currentNode = nodes[x, z];
                currentNode.walkable = !Physics.CheckSphere(currentNode.position, nodeRadius, unwalkableMask);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbours.Add(nodes[checkX, checkZ]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + scale.x / 2) / scale.x;
        float percentZ = (worldPosition.z + scale.z / 2) / scale.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return nodes[x, z];
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }

            foreach (Node neighbour in GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }
}
