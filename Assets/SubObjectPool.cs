using UnityEngine;
using System.Collections.Generic;

public class SubObjectPool : MonoBehaviour { //SUBTITLES
    public static SubObjectPool current;

    [Tooltip("Assign the arrow prefab.")]
    public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<Indicator> pooledObjects;

    void Awake() {
        current = this;
    }

    void Start() {
        pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator sub = Instantiate(pooledObject);
            sub.transform.SetParent(transform, false);
            sub.Activate(false);
            pooledObjects.Add(sub);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public Indicator GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            Indicator sub = Instantiate(pooledObject);
            sub.transform.SetParent(transform, false);
            sub.Activate(false);
            pooledObjects.Add(sub);
            return sub;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator sub in pooledObjects)
        {
            sub.Activate(false);
        }
    }
}
