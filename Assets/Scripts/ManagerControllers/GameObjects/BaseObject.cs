using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class BaseObject : MonoBehaviourPunCallbacks
{
    public static string Name<T>() where T : BaseObject => typeof(T).Name;

    public new string name => gameObject.name;

    public static BaseObject From(GameObject obj) => obj.GetComponent<BaseObject>();

    public static bool Is<T>(GameObject gameObject, out T t) where T : BaseObject {
        t = null;
        if (TryFrom(gameObject, out var baseObject)) {
            if (baseObject.name == Name<T>()) {
                t = (T)baseObject;
                return true;
            }
        }
        return false;
    }
    public static bool TryFrom(GameObject gameObject, out BaseObject baseObject) {
        return gameObject.TryGetComponent<BaseObject>(out baseObject);
    }

    protected void Awake() {
        gameObject.name = GetType().Name;
        Manager.GameObjectManager.Add(gameObject);
    }
}
