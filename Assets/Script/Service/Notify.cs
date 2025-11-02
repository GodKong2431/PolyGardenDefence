using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify<T>
{
    //구독자 리스트 T타입으로 생성
    private List<T> _notifyList = new List<T>();

    public List<T> NotifyList
    {
        get { return _notifyList; }
    }

    //구독자 추가 메서드
    public void AddObserver(T Observer) 
    { 
        _notifyList.Add(Observer); 
    }

    //구독자 제거 메서드
    public void RemoveObserver(T Observer) 
    { 
        _notifyList.Remove(Observer); 
    }
}
