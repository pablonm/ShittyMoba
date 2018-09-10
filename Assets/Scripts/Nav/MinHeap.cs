using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMinHeap
{
    /**
        Constructs an empty heap.
    */
    public NavMinHeap()
    {
        elements = new List<NodoAE>();
        elementsDict = new Dictionary<Vector2, NodoAE>();
        elements.Add(null);
    }

    /**
        Adds a new element to this heap.
        @param newElement the element to add
    */
    public void Add(NodoAE newElement)
    {
        if (elementsDict.ContainsKey(new Vector2(newElement.punto.x, newElement.punto.y)))
        {
            elementsDict[new Vector2(newElement.punto.x, newElement.punto.y)] = newElement;
        }
        else
        {
            elementsDict.Add(new Vector2(newElement.punto.x, newElement.punto.y), newElement);
        }


        // Add a new leaf
        elements.Add(null);
        int index = elements.Count - 1;

        // Demote parents that are larger than the new element
        while (index > 1 && getParent(index).CompareTo(newElement) > 0)
        {
            elements[index] = getParent(index);
            index = getParentIndex(index);
        }

        // Store the new element into the vacant slot
        elements[index] = newElement;
    }

    public NodoAE find(NodoAE nodo)
    {
        if (elementsDict.ContainsKey(new Vector2(nodo.punto.x, nodo.punto.y)))
        {
            return elementsDict[new Vector2(nodo.punto.x, nodo.punto.y)];
        }
        else
        {
            return null;
        }
    }

    /**
        Gets the minimum element stored in this heap.
        @return the minimum element
    */
    public NodoAE peek()
    {
        return elements[1];
    }

    /**
        Removes the minimum element from this heap.
        @return the minimum element
    */
    public NodoAE remove()
    {
        NodoAE minimum = elements[1];

        // Remove last element
        int lastIndex = elements.Count - 1;
        NodoAE last = elements[lastIndex];
        elements.RemoveAt(lastIndex);


        if (lastIndex > 1)
        {
            elements[1] = last;
            fixHeap();
        }

        return minimum;
    }

    /**
        Turns the tree back into a heap, provided only the root 
        node violates the heap condition.
    */
    private void fixHeap()
    {
        NodoAE root = elements[1];

        int lastIndex = elements.Count - 1;
        // Promote children of removed root while they are larger than last      

        int index = 1;
        bool more = true;
        while (more)
        {
            int childIndex = getLeftChildIndex(index);
            if (childIndex <= lastIndex)
            {
                // Get smaller child 

                // Get left child first
                NodoAE child = getLeftChild(index);

                // Use right child instead if it is smaller
                if (getRightChildIndex(index) <= lastIndex && getRightChild(index).CompareTo(child) < 0)
                {
                    childIndex = getRightChildIndex(index);
                    child = getRightChild(index);
                }

                // Check if larger child is smaller than root
                if (child.CompareTo(root) < 0)
                {
                    // Promote child
                    elements[index] = child;
                    index = childIndex;
                }
                else
                {
                    // Root is smaller than both children
                    more = false;
                }
            }
            else
            {
                // No children
                more = false;
            }
        }

        // Store root element in vacant slot
        elements[index] = root;
    }

    /**
        Returns the number of elements in this heap.
    */
    public int size()
    {
        return elements.Count - 1;
    }

    /**
        Returns the index of the left child.
        @param index the index of a node in this heap
        @return the index of the left child of the given node
    */
    private static int getLeftChildIndex(int index)
    {
        return 2 * index;
    }

    /**
        Returns the index of the right child.
        @param index the index of a node in this heap
        @return the index of the right child of the given node
    */
    private static int getRightChildIndex(int index)
    {
        return 2 * index + 1;
    }

    /**
        Returns the index of the parent.
        @param index the index of a node in this heap
        @return the index of the parent of the given node
    */
    private static int getParentIndex(int index)
    {
        return index / 2;
    }

    /**
        Returns the value of the left child.
        @param index the index of a node in this heap
        @return the value of the left child of the given node
    */
    private NodoAE getLeftChild(int index)
    {
        return elements[2 * index];
    }

    /**
        Returns the value of the right child.
        @param index the index of a node in this heap
        @return the value of the right child of the given node
    */
    private NodoAE getRightChild(int index)
    {
        return elements[2 * index + 1];
    }

    /**
        Returns the value of the parent.
        @param index the index of a node in this heap
        @return the value of the parent of the given node
    */
    private NodoAE getParent(int index)
    {
        return elements[index / 2];
    }

    public List<NodoAE> elements;
    public Dictionary<Vector2, NodoAE> elementsDict;
}
