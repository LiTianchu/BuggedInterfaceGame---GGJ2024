using UnityEngine;

[System.Serializable]
public abstract class AbstractSpawnState
{
    [SerializeField] protected string stateName = "Default State";
    [Tooltip("Duration in seconds. If negative, the state will not automatically transition to the next state.")]
    [SerializeField] protected float duration = 10f; // in duration is negative, it will not automatically transit to next state after duration
    [SerializeField] protected int nextStateIndex;

    private bool _shouldTransitNow = false;
    public string StateName { get => stateName; set => stateName = value; }
    public float Duration { get => duration; set => duration = value; }
    public int NextStateIndex { get => nextStateIndex; set => nextStateIndex = value; }


    protected float timeInState = 0f;

    public virtual void Enter(FileSystemLevelBattle level)
    {
        timeInState = 0f;
        Debug.Log($"Entering spawn state: {stateName}");
    }

    public virtual void Update(FileSystemLevelBattle level)
    {
        timeInState += Time.deltaTime;
    }

    public virtual void Exit(FileSystemLevelBattle level)
    {
        Debug.Log($"Exiting spawn state: {stateName}");
    }

    public virtual bool ShouldTransition(FileSystemLevelBattle level)
    {
        if (_shouldTransitNow)
        {
            _shouldTransitNow = false;
            return true;
        }

        return duration >= 0 && timeInState >= duration;
    }

    public virtual int GetNextState(FileSystemLevelBattle level)
    {
        return nextStateIndex;
    }

    public void TransitNow()
    {
        _shouldTransitNow = true;
    }
}