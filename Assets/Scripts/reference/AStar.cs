using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public AStar()
    {
    }
    private float Estimate(GameObject start, GameObject end)
    {
        return Vector3.Distance(start.transform.position, end.transform.position);
    }
    public List<Connection> PathfindAStar(Graph aGraph, GameObject start, GameObject end)
    {
        // Set up the start record
        NodeRecord StartRecord = new NodeRecord();
        StartRecord.Node = start;
        StartRecord.Connection = null;
        StartRecord.CostSoFar = 0;
        // Heuristic method; g = 0, for start node
        StartRecord.EstimatedTotalCost = Estimate(start, end);

        // List to store explore nodes
        // open list for storing nodes whose neighbours are yet to be explored
        PathfindingList OpenList = new PathfindingList();
        // closed list for nodes whose all neighbours have been explored
        PathfindingList ClosedList = new PathfindingList();

        // Add the start record to the open list
        OpenList.AddNodeRecord(StartRecord);

        // Iterate through and process each node.
        NodeRecord CurrentRecord = null;
        List<Connection> Connections;

        while (OpenList.GetSize() > 0)
        {
            // Find the smallest element in the open list (using the estimatedTotalCost, which is f = g + h)
            CurrentRecord = OpenList.GetSmallestElement();

            // If it is the goal node, then terminate.
            if (CurrentRecord.Node.Equals(end))
            {
                break;
            }

            // get its outgoing connections.
            Connections = aGraph.GetConnections(CurrentRecord.Node);
            // Loop through each connection in turn.
            GameObject NeighbourNode;
            float NeighbourNodeCost;
            NodeRecord NeighbourNodeRecord;
            float NeighbourNodeHeuristic;

            foreach (Connection aConnection in Connections)
            {
                // Get the cost estimate for the neighbour node.
                NeighbourNode = aConnection.GetToNode();
                NeighbourNodeCost = CurrentRecord.CostSoFar + aConnection.GetCost(); // f = g + h

                // If the node is closed we may have to skip, or remove it from the closed list.
                if (ClosedList.Contains(NeighbourNode))
                {
                    // Here we find the record in the closed list corresponding to the NeighbourNode.
                    NeighbourNodeRecord = ClosedList.Find(NeighbourNode);
                    // bharkharai calculate gareko cost if purano bhanda badi cha bhane skip hanne
                    if (NeighbourNodeRecord.CostSoFar <= NeighbourNodeCost)
                    {                     
                        continue;
                    }
                    // thorai cha bhane chai, closed list bata remove garne
                    ClosedList.RemoveNodeRecord(NeighbourNodeRecord);
                    // h = purano f - g
                    NeighbourNodeHeuristic = NeighbourNodeRecord.EstimatedTotalCost - NeighbourNodeRecord.CostSoFar;
                }
                // Skip if the node is open and we’ve not found a better route.
                else if (OpenList.Contains(NeighbourNode))
                {
                    // Here we find the record in the open list corresponding to the NeighbourNode.
                    NeighbourNodeRecord = OpenList.Find(NeighbourNode);
                    // mathi closed list jastai
                    if (NeighbourNodeRecord.CostSoFar <= NeighbourNodeCost)
                    {
                        continue;
                    }
                    NeighbourNodeHeuristic = NeighbourNodeRecord.EstimatedTotalCost - NeighbourNodeRecord.CostSoFar;
                }
                // Otherwise we know we’ve got an unvisited node, so make a record for it.
                else
                {
                    NeighbourNodeRecord = new NodeRecord();
                    NeighbourNodeRecord.Node = NeighbourNode;
                    NeighbourNodeHeuristic = Estimate(NeighbourNode, end);
                }

                // We’re here if we need to update the node. Update the cost, estimate and connection.
                NeighbourNodeRecord.CostSoFar = NeighbourNodeCost;
                NeighbourNodeRecord.Connection = aConnection;
                NeighbourNodeRecord.EstimatedTotalCost = NeighbourNodeCost + NeighbourNodeHeuristic; //f = g+h

                // And add it to the open list.
                if (!(OpenList.Contains(NeighbourNode)))
                {
                    OpenList.AddNodeRecord(NeighbourNodeRecord);
                }
            } // #End of for loop: Looping through Connections.
              
            // We’ve finished looking at the connections for the current node, so add it to the closed list
            // and remove it from the open list
            OpenList.RemoveNodeRecord(CurrentRecord);
            ClosedList.AddNodeRecord(CurrentRecord);
        }// end of while

        // We’re here if we’ve either found the goal, or if we’ve no more nodes to search, find which.
        List<Connection> tempList = new List<Connection>();

        if (!CurrentRecord.Node.Equals(end))
        {
            // We’ve run out of nodes without finding the goal, so there’s no solution
            return tempList;
        }
        else
        {
            while (!CurrentRecord.Node.Equals(start))
            {
                tempList.Add(CurrentRecord.Connection);
                CurrentRecord = ClosedList.Find(CurrentRecord.Connection.GetFromNode());
            }
            // The path is in the wrong order. Reverse the path, and return it.
            List<Connection> tempList2 = new List<Connection>();
            for (int i = (tempList.Count - 1); i >= 0; i--)
            {
                tempList2.Add(tempList[i]);
            }
            return tempList2;
        }
    }
}