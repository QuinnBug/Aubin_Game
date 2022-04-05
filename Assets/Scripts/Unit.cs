using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable
{
    public virtual void Start() 
    {
    }

    public virtual void OnDeselected()
    {
    }

    public virtual void OnSelected()
    {
    }
}
