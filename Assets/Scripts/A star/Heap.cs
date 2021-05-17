using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currenItemCount;
    public Heap(int maxHeapSize){
        items = new T[maxHeapSize];
    }

    public void Add(T item){
        item.HeapIndex = currenItemCount;
        items[currenItemCount] = item;
        SortUp(item);
        currenItemCount++;
    }
    
     //la raiz del arbol
    public T RemoveFirst(){
        currenItemCount--;
        T firstItem = items[0];
        items[0] = items[currenItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item){
        SortUp(item);
    }

    public int Count{
        get{
            return currenItemCount;
        }
    }
    public bool ContainsItem (T item){
        return Equals(items[item.HeapIndex],item);
    }
    void SortUp(T item){
        int parentIndex = (item.HeapIndex-1) / 2;
        while(true){
            T parentItem = items[parentIndex];
            //if tiene mayor prioridad
            if(item.CompareTo(parentItem) > 0){
                Swap(item,parentItem);
            }
            else{
                break;
            }
            parentIndex = (item.HeapIndex-1) / 2;
        }
    }

    void SortDown(T item){
        while(true){
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int SwapIndex;
            if(childIndexLeft < currenItemCount){
                SwapIndex = childIndexLeft;
                if(childIndexRight < currenItemCount){
                    //ver si el de la derecha tiene mayor prioridad entonces intercambiar con ese
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0){
                        SwapIndex = childIndexRight;
                    }
                }
                //si mi item actual tiene menos prioridad q el candidato a swap
                if(item.CompareTo(items[SwapIndex]) < 0){
                    Swap(item, items[SwapIndex]);
                }
                else return;
            }
            else return;
        }
    }


    void Swap(T itemA, T itemB){
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int temp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = temp;
    }
}

public interface IHeapItem<T> : IComparable{
    int HeapIndex{
        get;
        set;
    }
}
