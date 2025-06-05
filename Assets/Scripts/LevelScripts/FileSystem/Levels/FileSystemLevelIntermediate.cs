using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FileSystemLevelIntermediate : FileSystemLevel
{
    [Header("Spawn System")]
    [SerializeField] private List<AbstractSpawnState> spawnStates = new List<AbstractSpawnState>();
    [SerializeField] private int currentStateIndex = 0;
    
    [ShowInInspector, ReadOnly] 
    private AbstractSpawnState currentState;
    
    [ShowInInspector, ReadOnly]
    private float timeInCurrentState;
    
    public new void Start()
    {
        base.Start();
        InitializeSpawnStates();
        StartSpawnStateMachine();
    }
    
    private void InitializeSpawnStates()
    {
        // Example setup - you can configure this in inspector instead
        if (spawnStates.Count == 0)
        {
            spawnStates.Add(new IdleSpawnState { StateName = "Start", Duration = 5f });
            spawnStates.Add(new ClusterSpawnState { StateName = "Early Clusters", Duration = 30f });
            spawnStates.Add(new RingSpawnState { StateName = "Ring Phase", Duration = 40f });
            spawnStates.Add(new MixedSpawnState { StateName = "Mixed Assault", Duration = 60f });
            
            // Link states together
            for (int i = 0; i < spawnStates.Count - 1; i++)
            {
                spawnStates[i].NextState = spawnStates[i + 1];
            }
            // Last state loops back to mixed phase
            spawnStates[spawnStates.Count - 1].NextState = spawnStates[2];
        }
    }
    
    private void StartSpawnStateMachine()
    {
        if (spawnStates.Count > 0)
        {
            currentState = spawnStates[currentStateIndex];
            currentState.Enter(this);
        }
    }
    
    void Update()
    {
        if (currentState != null)
        {
            currentState.Update(this);
            timeInCurrentState += Time.deltaTime;
            
            if (currentState.ShouldTransition(this))
            {
                TransitionToNextState();
            }
        }
    }
    
    private void TransitionToNextState()
    {
        if (currentState != null)
        {
            currentState.Exit(this);
            
            AbstractSpawnState nextState = currentState.GetNextState(this);
            if (nextState != null)
            {
                currentState = nextState;
                currentState.Enter(this);
                timeInCurrentState = 0f;
                
                // Update index for debugging
                currentStateIndex = spawnStates.IndexOf(currentState);
            }
        }
    }
    
    public void CreateSpawnEvent(AbstractSpawnEvent spawnEvent, Vector3 position = default)
    {
        AbstractSpawnEvent se = Instantiate(spawnEvent, transform);
        se.transform.position = position;
        se.Initialize(this, FileSystemLevelManager.Instance.SmallZergPool, zergContainer);
        se.Spawn();
    }
    
    // Debug methods
    [Button("Force Next State")]
    private void ForceNextState()
    {
        if (Application.isPlaying && currentState != null)
        {
            TransitionToNextState();
        }
    }
    
    [Button("Reset State Machine")]
    private void ResetStateMachine()
    {
        if (Application.isPlaying)
        {
            currentStateIndex = 0;
            StartSpawnStateMachine();
        }
    }
}