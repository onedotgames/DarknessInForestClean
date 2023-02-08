using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum TaskStatus { SUCCESS, RUNNING, FAILURE};
    public TaskStatus Status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public virtual TaskStatus Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}
