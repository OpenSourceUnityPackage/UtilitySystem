using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Stat : MonoBehaviour
{
    [SerializeField]
    private float ratio;
    public float Ratio
    {
        get { return ratio; }
        set 
        {
            ratio = Mathf.Clamp(value, 0, 1);
        }
    }
};
