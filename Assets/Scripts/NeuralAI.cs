﻿using System;
using System.Collections.Generic;
using System.Linq;

//Inputs from Bug
//---------------
//age
//isFoodInfront
//isBugInfront
//isWallInfront
//isFoodLeft
//isBugLeft
//isWallLeft
//isFoodRight
//isBugRight
//isWallRight
//EnergyCount
//isBirthAble
//Bias

//Outputs to bug
//--------------
//Rotate Left
//Rotate Right
//Move Forward
//Birth
//Do nothing
//The value with the highest output is read
//If all output 0, do nothing



public class NeuralAI : AI
{
    public List<Node> nodeList;
    public List<Connect> connectList;

    //NEW NEURAL AI! Gotta make sure it has the proper inputs
    public NeuralAI()
    {
        nodeList = new List<Node>();
        connectList = new List<Connect>();
        //Inputs
        nodeList.Add(new Node(NodeType.Sensor, "age"));
        nodeList.Add(new Node(NodeType.Sensor, "isFoodInfront"));
        nodeList.Add(new Node(NodeType.Sensor, "isBugInfront"));
        nodeList.Add(new Node(NodeType.Sensor, "isWallInfront"));
        nodeList.Add(new Node(NodeType.Sensor, "isFoodLeft"));
        nodeList.Add(new Node(NodeType.Sensor, "isBugLeft"));
        nodeList.Add(new Node(NodeType.Sensor, "isWallLeft"));
        nodeList.Add(new Node(NodeType.Sensor, "isFoodRight"));
        nodeList.Add(new Node(NodeType.Sensor, "isBugRight"));
        nodeList.Add(new Node(NodeType.Sensor, "isWallRight"));
        //nodeList.Add(new Node(NodeType.Sensor, "EnergyCount"));
        nodeList.Add(new Node(NodeType.Sensor, "percentFull"));
        nodeList.Add(new Node(NodeType.Sensor, "isBirthAble"));
        nodeList.Add(new Node(NodeType.Sensor, "iffSame"));
        nodeList.Add(new Node(NodeType.Sensor, "Bias"));

        //Outputs
        nodeList.Add(new Node(NodeType.Output, "rotateLeft"));
        nodeList.Add(new Node(NodeType.Output, "rotateRight"));
        nodeList.Add(new Node(NodeType.Output, "moveForward"));
        nodeList.Add(new Node(NodeType.Output, "birth"));
        nodeList.Add(new Node(NodeType.Output, "doNothing"));

        //Connections between in and out
        foreach (Node n in nodeList)
        {
            foreach (Node n2 in nodeList)
            {
                if ((n.getNodeType() == NodeType.Sensor) && (n2.getNodeType() == NodeType.Output))
                {
                    connectList.Add(new Connect(n, n2, UnityEngine.Random.Range(0.0f, 1.0f)));
                }
            }
        }

        mutate();
    }

    public NeuralAI(List<Node> nL, List<Connect> cL)
    {
        this.nodeList = nL;
        this.connectList = cL;

        mutate();
    }

    void mutate()
    {
        foreach (Connect c in connectList)
        {
            if (rand() < 1)
            {
                c.weight += UnityEngine.Random.Range(-1.0f, 1.0f);
                c.weight = c.fakeSigmoid(c.weight);
            }
        }

        if (rand() < 1)
        {
            newNode();
        }
    }

    void newNode()
    {
        Node n = new Node(NodeType.Hidden);
        nodeList.Add(n);

        int connectionToMessUp = UnityEngine.Random.Range(0, connectList.Count - 1);
        Connect messThisUp = connectList[connectionToMessUp];
        messThisUp.enabled = false;

        Connect newConnect1 = new Connect(messThisUp.from, n, UnityEngine.Random.Range(0f, 1f));
        Connect newConnect2 = new Connect(n, messThisUp.to, UnityEngine.Random.Range(0f, 1f));
        UnityEngine.Debug.Log("MUTATION ADDED");
    }

    int rand()
    {
        return UnityEngine.Random.Range(0, 1000);
    }
    
    override
    public void move()
    {
        parent.age++;
        nodesUpdate();
    }

    void nodesUpdate()
    {
        inputUpdate();

        while (!allNodesUpdated()) {
            
            foreach (Node n in nodeList)
            {
                if (!n.isNodeUpdated())
                {
                    if (ableToUpdateNode(n))
                    {
                        List<Connect> involvedConnections = new List<Connect>();
                        foreach (Connect c in connectList)
                        {
                            if (c.to == n) involvedConnections.Add(c);
                        }
                        n.updateNode(involvedConnections);
                    }
                }
            }
        }

        outputUpdate();

        foreach (Node n in nodeList)
        {
            n.updated = false;
        }
    }

    bool allNodesUpdated()
    {
        bool output = true;
        foreach (Node n in nodeList)
        {
            if (n.isNodeUpdated() == false) output = false;
        }
        return output;
    }

    bool ableToUpdateNode(Node n)
    {
        List<Node> nodesConnectedTo = new List<Node>();

        foreach (Connect c in connectList)
        {
            if ((c.enabled) && (c.from == n))
            {
                nodesConnectedTo.Add(c.to);
            }
        }

        foreach (Node n2 in nodesConnectedTo)
        {
            if (!n2.isNodeUpdated()) return false;
        }
        return true;
    }

    void inputUpdate()
    {
        parent.see();
        setSight(parent.tileInFront, "isFoodInFront", "isBugInFront", "isWallInFront");
        setSight(parent.tileInFront, "isFoodLeft", "isBugLeft", "isWallLeft");
        setSight(parent.tileInFront, "isFoodRight", "isBugRight", "isWallRight");
        setNodeValue("isBirthAble", ((parent.energy >= 100) ? 100: 0));
        setNodeValue("percentFull", (parent.energy / 200));
        setNodeValue("age", parent.age);
        //setNodeValue("EnergyCount", UnityEngine.Mathf.FloorToInt(parent.energy));
        setNodeValue("Bias", 1.0f);
    }

    void setSight(int i, string food, string bug, string wall)
    {
        switch (parent.tileInFront)
        {
            case 0:
                //NORMAL
                setNodeValue(food, 0.0f);
                setNodeValue(bug, 0.0f);
                setNodeValue(wall, 0.0f);
                break;
            case 1:
                //WALL
                setNodeValue(food, 0.0f);
                setNodeValue(bug, 0.0f);
                setNodeValue(wall, 100.0f);
                break;
            case 2:
                //BUG
                setNodeValue(food, 0.0f);
                setNodeValue(bug, 100.0f);
                setNodeValue(wall, 0.0f);
                break;
            case 3:
                //FOOD
                setNodeValue(food, 100.0f);
                setNodeValue(bug, 0.0f);
                setNodeValue(wall, 0.0f);
                break;
        }
    }

    public void outputUpdate()
    {
        List<Node> outputNodes = new List<Node>();
        foreach(Node n in nodeList)
        {
            if (n.getNodeType() == NodeType.Output)
            {
                outputNodes.Add(n);
            }
        }

        List<Node> sorted = outputNodes.OrderBy(n => n.value).ToList();


        switch (sorted[sorted.Count-1].name)
        {
            case "rotateLeft":
                parent.rotateLeft();
                break;
            case "rotateRight":
                parent.rotateRight();
                break;
            case "moveForward":
                parent.moveForward();
                break;
            case "birth":
                parent.birth();
                break;
            default:
                parent.doNothing();
                break;
        }
        
        
    }

    public void setNodeValue(string name, float value)
    {
        foreach (Node n in nodeList)
        {
            if (n.getNodeName() == name)
            {
                n.value = value;
            }
        }
    }

    public float getNodeValue(string name)
    {
        foreach (Node n in nodeList)
        {
            if (n.getNodeName() == name) return n.value;
        }
        return 420f;
    }
}

public class Node
{
    public NodeType type;
    public string name;
    public float value;
    public bool updated = false;

    public Node (NodeType type)
    {
        this.type = type;
        this.name = "Hidden";
    }

    public Node (NodeType type, string name)
    {
        this.type = type;
        this.name = name;
    }

    public NodeType getNodeType ()
    {
        return type;
    }

    public string getNodeName()
    {
        return name;
    }

    public bool isNodeUpdated()
    {
        if ((type == NodeType.Hidden) || (type == NodeType.Output)) return updated;
        return true;
    }

    public void updateNode(List<Connect> cList)
    {

        List<float> values = new List<float>();
        foreach (Connect c in cList)
        {
            values.Add(c.from.value * c.weight);
        }

        value = 0f;
        foreach (float f in values)
        {
            value += f;
        }

        updated = true;
    }

    override
    public string ToString()
    {
        string o = "";
        o += "Name " + name + " ";
        o += " Type " + type + " ";
        o += " Value " + value + " ";
        o += " Updated " + updated;
        return o;
    }
}

public enum NodeType
{
    Sensor,
    Output,
    Hidden
}

public class Connect
{
    public Node from;
    public Node to;
    public float weight;
    public bool enabled;

    public Connect (Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = fakeSigmoid(weight);
        enabled = true;
    }

    public float fakeSigmoid(float i)
    {
        if (i > 1.0f)
        {
            i = 1.0f;
        }
        if (i < 0.0f)
        {
            i = 0.0f;
        }
        return i;
    }

    override
    public string ToString()
    {
        string o = "";
        o += "From " + from.name;
        o += " To " + to.name;
        o += " Weight " + weight;
        return o;
    }
}