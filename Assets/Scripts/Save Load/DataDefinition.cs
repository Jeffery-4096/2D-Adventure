using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefinition : MonoBehaviour
{
    public string ID;

    private void OnValidate()
    {
        if (ID == string.Empty)
        {
            ID = System.Guid.NewGuid().ToString();
        }
    }
}
