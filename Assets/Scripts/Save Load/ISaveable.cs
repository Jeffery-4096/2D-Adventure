using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefinition GetDataID();
    void RegisterSaveData()
    {
        if (DataManager.instance != null)
            DataManager.instance.RegisterSaveData(this);
    }
    void UnRegisterSaveData()
    {
        if (DataManager.instance != null)
            DataManager.instance.UnRegisterSaveData(this);
    }

    void GetSaveData(Data data);
    void LoadData(Data data);
}
