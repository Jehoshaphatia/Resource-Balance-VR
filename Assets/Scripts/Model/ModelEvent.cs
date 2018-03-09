using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace ResourceBalancing
{
    [System.Serializable]
    public class ModelEvent : UnityEvent<int, int>
    {
    }
}