using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FileSystemLevelBattle : FileSystemLevel
{
    [TitleGroup("Spawn System")]
    [SerializeField, ListDrawerSettings(ShowIndexLabels = true)]
    [SerializeReference] private List<AbstractSpawnState> spawnStates = new();
    [SerializeField] private int currentStateIndex = 0;
    
    [ShowInInspector, ReadOnly] 
    private AbstractSpawnState currentState;
    
    [ShowInInspector, ReadOnly]
    private float timeInCurrentState;
    
    public new void Start()
    {
        base.Start();
        StartSpawnStateMachine();
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
        if(_hasWon){ return; }
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
        if(_hasWon) { return; }

        if (currentState != null)
        {
            currentState.Exit(this);

            AbstractSpawnState nextState = spawnStates[currentState.GetNextState(this)];
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
    
    public override void CreateSpawnEvent(AbstractSpawnEvent spawnEvent, Vector3 position = default)
    {
        if(_hasWon) { return; }

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
            _hasWon = false;
            currentStateIndex = 0;
            StartSpawnStateMachine();
        }
    }
}