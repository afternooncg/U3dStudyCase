using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseSingleton<T> where T : BaseSingleton<T>
{
    protected static T sm_instance = null;

    public static T getInstance()
    {
        if (sm_instance == null)
        {
            // get all public constructor method
            ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            // get the method with no parameter from ctors 
            ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
                throw new Exception("Non-public ctor() not found!");

            // return the method
            sm_instance = ctor.Invoke(null) as T;
        }

        return sm_instance;
    }

    protected BaseSingleton()
    {
        init();
    }

    public virtual void init()
    {

    }
}