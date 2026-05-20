using UnityEngine;

public class Node
{
    private GameObject item;
    private Node next;

    public Node(GameObject item)
    {
        this.item = item;
        this.next = null;
    }
    public void Next(Node next)
    {
        this.next = next;
    }
    public Node Next()
    {
        return next;
    }
    public GameObject getItem()
    {
        return item;
    }
}
public class CircularLinkedList : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Node head;
    private Node tail;

   public void AddNode(GameObject item)
   {
       Node newNode = new Node(item);
       if (head == null)
       {
           head = newNode;
           tail = newNode;
           tail.Next(head);
       }
       else
       {
           tail.Next(newNode);
           tail = newNode;
           tail.Next(head);
       }
   }
   public Node SearchNode(GameObject item)
   {
       Node current = head;
       while (current != null)
       {
           if (current.getItem() == item)
           {
               Debug.Log("Node found: " + item.name);
               return current;
           }
           current = current.Next();
           if (current == head) break;
       }
       Debug.Log("Node not found: " + item.name);
       return null;
   }
   public Node getHead()
   {
       return head;
   }
   public Node getTail()
   {
       return tail;
   }
}

