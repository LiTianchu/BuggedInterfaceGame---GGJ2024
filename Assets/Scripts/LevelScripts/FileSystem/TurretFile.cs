using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretFile : MonoBehaviour
{
    public abstract void Fire();
    public abstract void OnDeploy();
    public abstract void OnDestroy();
}
