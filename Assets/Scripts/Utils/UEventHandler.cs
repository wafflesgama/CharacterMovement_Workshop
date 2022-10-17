using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UEventHandler
{
    public delegate void UnsubscribeAction();

    #region UEvent
    public class UEvent
    {
        private event Action eventData;
        public void TryInvoke() => eventData?.Invoke();
        public void Subscribe(UEventHandler source, Action action)
        {
            CheckInstatiated(ref source);
            eventData += action;

            UnsubscribeAction unsubAction = null;
            unsubAction = delegate ()
            {
                source.OnSubscribe -= unsubAction;
                Unsubscribe(action);
            };
            source.OnSubscribe += unsubAction;
        }
        public void Unsubscribe(Action action) => eventData -= action;
    }



    public class UEvent<T>
    {
        private event Action<T> eventData;
        public void TryInvoke(T arg1) => eventData?.Invoke(arg1);
        public void Subscribe(UEventHandler source, Action<T> action)
        {
            CheckInstatiated(ref source);
            eventData += action;

            UnsubscribeAction unsubAction = null;
            unsubAction = delegate ()
            {
                source.OnSubscribe -= unsubAction;
                Unsubscribe(action);
            };
            source.OnSubscribe += unsubAction;
        }
        public void Unsubscribe(Action<T> action) => eventData -= action;
    }

    public class UEvent<T1, T2>
    {
        private event Action<T1, T2> eventData;
        public void TryInvoke(T1 arg1, T2 arg2) => eventData?.Invoke(arg1, arg2);
        public void Subscribe(UEventHandler source, Action<T1, T2> action)
        {
            CheckInstatiated(ref source);
            eventData += action;

            UnsubscribeAction unsubAction = null;
            unsubAction = delegate ()
            {
                source.OnSubscribe -= unsubAction;
                Unsubscribe(action);
            };
            source.OnSubscribe += unsubAction;
        }
        public void Unsubscribe(Action<T1, T2> action) => eventData -= action;

    }

    public class UEvent<T1, T2, T3>
    {
        private event Action<T1, T2, T3> eventData;
        public void TryInvoke(T1 arg1, T2 arg2, T3 arg3) => eventData?.Invoke(arg1, arg2, arg3);
        public void Subscribe(UEventHandler source, Action<T1, T2, T3> action)
        {
            CheckInstatiated(ref source);
            eventData += action;

            UnsubscribeAction unsubAction = null;
            unsubAction = delegate ()
            {
                source.OnSubscribe -= unsubAction;
                Unsubscribe(action);
            };
            source.OnSubscribe += unsubAction;
        }
        public void Unsubscribe(Action<T1, T2, T3> action) => eventData -= action;
    }

    private static void CheckInstatiated(ref UEventHandler uEventHandler)
    {
        if (uEventHandler != null) return;
        Debug.LogError($"This uEventHandler ({uEventHandler}) is not instantiated\nConsider instatiating it with: eventHandler= new UEventHandler();\nWill instantiate for debug purposes");
        uEventHandler=new UEventHandler();
    }

    #endregion UEvent

    private event UnsubscribeAction OnSubscribe;
    public void UnsubcribeAll()
    {
        OnSubscribe?.Invoke();
    }
}
