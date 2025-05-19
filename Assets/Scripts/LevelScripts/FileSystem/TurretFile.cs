using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretFile : MonoBehaviour
{
    [SerializeField] private string turretName = "Turret";

    public abstract void Fire();
    public abstract void OnDeploy();
    public abstract void OnDestroy();

    public override string ToString()
    {
        return turretName;
    }
    
    public override bool Equals(object other)
    {
        return turretName.Equals((other as TurretFile).turretName);
    }

    public override int GetHashCode()
    {
        return turretName.GetHashCode();
    }
}
