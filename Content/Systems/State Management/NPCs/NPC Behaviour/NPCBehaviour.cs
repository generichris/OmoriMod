using System;
using System.Collections.Generic;

using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Util;

namespace OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;

/// <summary>
/// Helper class that can store behaviours for NPCs for state management
/// </summary>
public abstract class NPCBehaviour
{

    protected List<float> behaviourParameters;

    public bool IsDone { get => !_inProgress && _hasStarted; }
    private bool _inProgress;

    public bool HasStarted { get => _hasStarted; }
    private bool _hasStarted;
    public bool JustCompleted { get => _justCompleted; }
    private bool _justCompleted;

    public string BehaviourName { get => behaviourName; }
    private string behaviourName;

    protected int _defaultExitStatus;

    /// <summary>
    /// In case you need to run a sub-behaviour in your behaviour. NOTE: Sub-behaviours should NOT be used for massive changes.
    /// They should be used if you need to keep information from the parent behaviour after the sub behaviour's execution.
    /// A good example of this is <see cref="ChasePlayerExitOnTimeOut"/> which needs to retain its timer after it switches 
    /// into the <see cref="ChasePlayerJump"/> behaviour
    /// </summary>
    protected NPCBehaviour subBehaviour;
    private BehaviourInfo _behaviourSnapshot;

    private bool _runSubBehaviour;



    private void Init(string name, int defaultExitStatus)
    {
        behaviourParameters = [];
        _inProgress = false;
        _hasStarted = false;
        _justCompleted = false;
        _runSubBehaviour = false;
        behaviourName = name.OmoriModString();
        _defaultExitStatus = defaultExitStatus;
    }

    /// <summary>
    /// Creates a <see cref="NPCBehaviour"/> with no name. NOTE: No name != (name = null)
    /// </summary>
    public NPCBehaviour(int defaultExitStatus)
    {
        string name = (behaviourName ?? GetType().Name).OmoriModString();
        Init(name, defaultExitStatus);
    }

    /// <summary>
    /// Creates a <see cref="NPCBehaviour"/>.
    /// </summary>
    /// <param name="behaviourName">In case of special naming conventions. Typically do not pass in.</param>
    public NPCBehaviour(int defaultExitStatus, string behaviourName)
    {
        Init(behaviourName, defaultExitStatus);
    }

    /// <summary>
    /// Resets the attack. NOTE: this is NOT where parameter initilization occurs. Please check <see cref="OnStart"/>
    /// </summary>
    public void Reset()
    {
        _inProgress = false;
        _hasStarted = false;
        _justCompleted = false;
        _runSubBehaviour = false;
    }

    /// <summary>
    /// Schedules the <see cref="subBehaviour"/> to run on the next available tick.
    /// Suspends the parent behaviour during execution.
    /// </summary>
    protected void ReadySubBehaviour(BehaviourInfo behaviourInfo)
    {
        _runSubBehaviour = true;
        _behaviourSnapshot = behaviourInfo.Copy();
    }

    private void RevertToSnapshot(BehaviourInfo behaviourInfo)
    {
        behaviourInfo.Set(_behaviourSnapshot);
        _behaviourSnapshot = null;
    }

    /// <summary>
    /// A hook method to get parameters ready.
    /// </summary>
    protected virtual void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// A hook method to allow you to write AI. Make sure to edit <paramref name="behaviourInfo"/> in this function.
    /// Runs AFTER <see cref="GuaranteedAI(OmoriBehaviourNPC, BehaviourInfo)"/>
    /// </summary>
    /// <returns>Void</returns>
    protected virtual void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// A hook method to allow you to write AI. Ran regardless of whether the <see cref="subBehaviour"/>
    /// is executing or not. Runs BEFORE <see cref="AI(OmoriBehaviourNPC, BehaviourInfo)"/>
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="behaviourInfo"></param>
    protected virtual void GuaranteedAI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// hook method to allow you to pick your frame from the NPC. Use <paramref name="frameHeight"/> to determine frame height
    /// and <see cref="CurrentFrame"/> for frame info. Set frame info in <see cref="behaviourInfo"/> in order for it to 
    /// carry over to the next <see cref="NPCBehaviour"/>
    /// </summary>
    /// <param name="frameHeight"></param>
    protected virtual void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight) { }


    private void Start(OmoriBehaviourNPC npc, BehaviourInfo info)
    {
        _inProgress = true;
        _hasStarted = true;
        info.ExitStatus = -1;
        OnStart(npc, info);
    }

    /// <summary>
    /// General method to run an AI.
    /// </summary>
    /// <param name="ai">The AI method being called</param>
    /// <param name="npc">The <see cref="OmoriBehaviourNPC"/> this is being performed on</param>
    /// <param name="info">The <see cref="BehaviourInfo"/>.</param>
    /// <param name="isSubBehaviour">True if this AI happends to be running on a subbehaviour</param>
    private bool RunAI(Action<OmoriBehaviourNPC, BehaviourInfo> ai, OmoriBehaviourNPC npc, BehaviourInfo info, bool isSubBehaviour)
    {
        ai?.Invoke(npc, info);
        if (info.ExitStatus >= 0)
        {
            // if exiting, reset these values
            _runSubBehaviour = false;
            subBehaviour?.Reset();

            if (isSubBehaviour)
            {
                // if it is a sub-behaviour, then revert to snapshot
                RevertToSnapshot(info);
            }
            else
            {
                // regular ending stuff if not sub-behaviour
                _inProgress = false;
                _justCompleted = true;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Call this method to use your AI. Performs the <see cref="AI(OmoriBehaviourNPC, BehaviourInfo)"/> method.
    /// This will also perform the <see cref="GuarenteedAI(OmoriBehaviourNPC, BehaviourInfo)"/> every tick.
    /// The <see cref="AI(OmoriBehaviourNPC, BehaviourInfo)"/> method will NOT be run if the <see cref="subBehaviour"/> is running.
    /// When the submethod is finished, the <see cref="BehaviourInfo"/> <paramref name="info"/> will be reset to the 
    /// value it was before the submethod started running.
    /// </summary>
    public void PerformAI(OmoriBehaviourNPC npc, BehaviourInfo info)
    {
        if (IsDone)
        {
            _justCompleted = false;
            return;
        }

        if (!_hasStarted)
        {
            Start(npc, info);
        }

        if (RunAI(GuaranteedAI, npc, info, false)) { return; }
        if (_runSubBehaviour)
        {
            RunAI(subBehaviour.PerformAI, npc, info, true);
        }
        else
        {
            if (RunAI(AI, npc, info, false)) { return; }
        }
    }

    /// <summary>
    /// Call this method to animate your <paramref name="npc"/>.
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="behaviourInfo"></param>
    /// <param name="frameHeight"></param>
    public void PerformFindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        FindFrame(npc, behaviourInfo, frameHeight);
    }

    public override bool Equals(object obj)
    {
        return obj is NPCBehaviour other ? BehaviourName == other.BehaviourName : false;
    }

    public override int GetHashCode()
    {
        return BehaviourName.GetHashCode();
    }
}