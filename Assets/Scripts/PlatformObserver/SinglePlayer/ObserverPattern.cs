using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ObserverPattern.SinglePlayer
{
    //Invokes the notificaton method
    public class ObserverHandler
    {
        //A list with observers that are waiting for something to happen
        private List<Observer> observers = new List<Observer>();

        //Send notifications if something has happened
        public void Notify()
        {
            Debug.Log("Add");
            for (int i = 0; i < observers.Count; i++)
            {
                //Notify all observers even though some may not be interested in what has happened
                //Each observer should check if it is interested in this event
                observers[i].Add(i , observers);
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