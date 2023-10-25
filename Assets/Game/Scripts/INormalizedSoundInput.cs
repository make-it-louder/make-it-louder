using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INormalizedSoundInput
{
    float normalizedDB
    {
        get;
    }
    GameObject gameObject
    {
        get;
    }
}
