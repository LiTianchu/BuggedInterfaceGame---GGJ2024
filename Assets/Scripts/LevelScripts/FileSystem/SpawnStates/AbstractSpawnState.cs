using UnityEngine;

[System.Serializable]
public abstract class AbstractSpawnState
{
    [SerializeField] protected string stateName;
    [SerializeField] protected float duration = 10f;
    [SerializeField] protected AbstractSpawnState nextState;
    
    public string StateName{ get => stateName; set => stateName = value; }
    public float Duration { get => duration; set => duration = value; }
    public AbstractSpawnState NextState { get => nextState; set => nextState = value; }

    
    protected float timeInState = 0f;
    
    public virtual void Enter(FileSystemLevelIntermediate level)
    {
        timeInState = 0f;
        Debug.Log($"Entering spawn state: {stateName}");
    }
    
    public virtual void Update(FileSystemLevelIntermediate level)
    {
        timeInState += Time.deltaTime;
    }
    
    public virtual void Exit(FileSystemLevelIntermediate level)
    {
        Debug.Log($"Exiting spawn state: {stateName}");
    }
    
    public virtual bool ShouldTransition(FileSystemLevelIntermediate level)
    {
        return timeInState >= duration;
    }
    
    public virtual AbstractSpawnState GetNextState(FileSystemLevelIntermediate level)
    {
        return nextState;
    }
}