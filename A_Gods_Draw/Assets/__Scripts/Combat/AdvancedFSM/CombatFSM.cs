/* 
 * Adapted by 
 * Henrik
*/

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is adapted and modified from the FSM implementation class available on UnifyCommunity website
/// The license for the code is Creative Commons Attribution Share Alike.
/// It's originally the port of C++ FSM implementation mentioned in Chapter01 of Game Programming Gems 1
/// You're free to use, modify and distribute the code in any projects including commercial ones.
/// Please read the link to know more about CCA license @http://creativecommons.org/licenses/by-sa/3.0/
/// </summary>
public enum Transition
{
    None = 0,
    StartCombat,
    EnterDraw,
    EnterMain,
    EnterDiscard,
    EnterCombatStart,
    EnterCombatCard,
    EnterCombatEnemy,
    EnterEnd
}

[System.Serializable] // # Making it serializable temporarily to se it in the inspector
public enum CombatState
{
    None = 0,
    EnterCombat,
    DrawStep,
    MainPhase,
    DiscardStep,
    CombatStartStep,
    CombatCardStep,
    CombatEnemyStep,
    EndStep
}

public class CombatFSM : MonoBehaviour 
{
    private List<CombatFSMState> fsmStates;

    //The fsmStates are not changing directly but updated by using transitions
    private CombatState currentStateID;
    public CombatState CurrentStateID { get { return currentStateID; } }

    private CombatFSMState currentState;
    public CombatFSMState CurrentState { get { return currentState; } }

    public CombatFSM()
    {
        fsmStates = new List<CombatFSMState>();
    }

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }
	void Start () => Initialize(); // Use this for initialization
	void Update () => FSMUpdate(); // Update is called once per frame
    void FixedUpdate() => FSMFixedUpdate();

    /// <summary>
    /// Add New State into the list
    /// </summary>
    public void AddFSMState(CombatFSMState fsmState)
    {
        // Check for Null reference before deleting
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state
        //   the state the machine is in when the simulation begins
        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        // Add the state to the List if it�s not inside it
        foreach (CombatFSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list: FSMSTATE: " + fsmState);
                return;
            }
        }

        //If no state in the current then add the state to the list
        fsmStates.Add(fsmState);
    }

    /// <summary>
    /// This method delete a state from the FSM List if it exists, 
    ///   or prints an ERROR message if the state was not on the List.
    /// </summary>
    public void DeleteState(CombatState fsmState)
    {
        // Check for NullState before deleting
        if (fsmState == CombatState.None)
        {
            Debug.LogError("FSM ERROR: bull id is not allowed");
            return;
        }

        // Search the List and delete the state if it�s inside it
        foreach (CombatFSMState state in fsmStates)
        {
            if (state.ID == fsmState)
            {
                fsmStates.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: The state passed was not on the list. Impossible to delete it");
    }

    /// <summary>
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. If current state
    ///  doesn�t have a target state for the transition passed, 
    /// an ERROR message is printed.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Check for NullTransition before changing the current state
        if (trans == Transition.None)
        {
            Debug.LogError("FSM ERROR: Null transition is not allowed");
            return;
        }

        // Check if the currentState has the transition passed as argument
        CombatState id = currentState.GetOutputState(trans);
        if (id == CombatState.None)
        {
            Debug.LogError("FSM ERROR: " + currentState + " does not have a target state for this transition; " + trans);
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (CombatFSMState state in fsmStates)
        {
            if (state.ID == currentStateID)
            {
                currentState = state;
                break;
            }
        }

        // Debug.Log("transition done to: " + currentStateID);
    }
}
