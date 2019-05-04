using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgradeStruct
{
    public Stat.Type type;
    public float value;

    public StatUpgradeStruct(Stat.Type type, float val)
    {
        this.type = type;
        value = val;
    }

    static public StatUpgradeStruct GetFromPool(long randomValue)
    {
        return new StatUpgradeStruct((Stat.Type)(randomValue%Stat.NB_STAT_TYPE_ENUM), 1 + ((randomValue/long.MaxValue)/2f));
    }

}
