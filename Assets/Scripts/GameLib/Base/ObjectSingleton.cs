using System;

public class ObjectSingleton<T> where T:class
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = CreateInstanceOfT();
            }

            return _instance;
        }
    }

    private static T CreateInstanceOfT()
    {
       return Activator.CreateInstance(typeof(T), true) as T;
    }
}
