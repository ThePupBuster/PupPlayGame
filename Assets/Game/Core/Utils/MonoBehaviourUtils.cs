using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MonoBehaviourUtils
{
    public static void ResolveOptionalAssignment<T>(this MonoBehaviour behaviour, ref T target)
        where T : class
    {
        if (target == null)
        {
            target = behaviour.GetComponent<T>();
        }

        if (target == null)
        {
            throw new NullReferenceException("Unable to resolve type: " + typeof(T).Name);
        }
    }

    public static T GetOrCreateComponent<T>(this Behaviour behaviour)
        where T : MonoBehaviour
    {
        T cmp = behaviour.GetComponent<T>();
        if (!cmp)
        {
            cmp = behaviour.gameObject.AddComponent<T>();
        }
        return cmp;
    }

    public static void DestoryBehaviourGameObject(MonoBehaviour behaviour)
    {
        if (behaviour && behaviour.gameObject)
        {
            GameObject.Destroy(behaviour.gameObject);
        }
    }

    public static StringBuilder PrintDebugHierarchy(this MonoBehaviour behaviour, StringBuilder builder = null)
    {
        return PrintDebugHierarchy(behaviour.transform, builder);
    }

    public static StringBuilder PrintDebugHierarchy(this Transform transform, StringBuilder builder = null)
    {
        if (transform.parent)
        {
            builder = PrintDebugHierarchy(transform.parent, builder);
        }

        if (builder == null)
        {
            builder = new StringBuilder();
        }
        else if (builder.Length != 0)
        {
            builder.Append('/');
        }
        return builder.Append(transform.name);
    }
}
