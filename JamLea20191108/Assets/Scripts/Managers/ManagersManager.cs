using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersManager : Singleton<ManagersManager>
{
    [SerializeField]
    private List<AManager> _managers = new List<AManager>();

    private Dictionary<System.Type, AManager> _managerDict = new Dictionary<System.Type, AManager>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        foreach(var manager in _managers)
        {
            AManager instance = Instantiate(manager, transform);
            _managerDict.Add(instance.GetType(), instance);

            instance.name = manager.GetType().ToString();
            instance.Initialize();
        }
    }

    public T Get<T>() where T : AManager
    {
        System.Diagnostics.Debug.Assert(_managerDict.ContainsKey(typeof(T)));

        return (T) _managerDict[typeof(T)];
    }
}
