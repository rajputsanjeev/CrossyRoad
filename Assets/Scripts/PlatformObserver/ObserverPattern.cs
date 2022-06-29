using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ObserverPattern
{
    //Invokes the notificaton method
    public class ObserverHandler
    {
        //A list with observers that are waiting for something to happen
        private List<Observer> observers = new List<Observer>();

        //Send notifications if something has happened
        public void Notify()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                //Notify all observers even though some may not be interested in what has happened
                //Each observer should check if it is interested in this event
                observers[i].OnAddToPool();
            }
        }

        public void AddObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(Observer observer)
        {
        }
    }
}