using System;
using System.Collections.Generic;

using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Util;

namespace OmoriMod.Content.Systems.State_Management.Projectiles.Projectile_Behaviour;

/// <summary>
/// Helper class that can store behaviours for NPCs for state management
/// </summary>
public abstract class ProjectileBehaviour
{

    protected List<float> _behaviourParameters;

    public bool IsDone { get => !_inProgress && _hasStarted; }
    private bool _inProgress;

    public bool HasStarted { get => _hasStarted; }
    private bool _hasStarted;
    public bool JustCompleted { get => _justCompleted; }
    private bool _justCompleted;

    public string BehaviourName { get => _behaviourName; }
    private string _behaviourName;

    protected int _defaultExitStatus;

    /// <summary>
    /// In case you need to run a sub-behaviour in your behaviour. NOTE: Sub-behaviours should NOT be used for massive changes.
    /// They should be used if you need to keep information from the parent behaviour after the sub behaviour's execution.
    /// A good example of this is <see cref="ChasePlayerExitOnTimeOut"/> which needs to retain its timer after it switches 
    /// into the <see cref="ChasePlayerJump"/> behaviour
    /// </summary>
    protected ProjectileBehaviour _subBehaviour;
    private BehaviourInfo _behaviourSnapshot;

    private bool _runSubBehaviour;



    private void Init(string name, int defaultExitStatus)
    {
        _behaviourParameters = [];
        _inProgress = false;
        _hasStarted = false;
        _justCompleted = false;
        _runSubBehaviour = false;
        _behaviourName = name.OmoriModString();
        _defaultExitStatus = defaultExitStatus;
    }

    /// <summary>
    /// Creates a <see cref="NPCBehaviour"/> with no name. NOTE: No name != (name = null)
    /// </summary>
    protected ProjectileBehaviour(int defaultExitStatus)
    {
        string name = (GetType().Name).OmoriModString();
        Init(name, defaultExitStatus);
    }

    /// <summary>
    /// Creates a <see cref="NPCBehaviour"/>.
    /// </summary>
    /// <param name="defaultExitStatus"></param>
    /// <param name="behaviourName">In case of special naming conventions. Typically do not pass in.</param>
    protected ProjectileBehaviour(int defaultExitStatus, string behaviourName)
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
    /// Schedules the <see cref="_subBehaviour"/> to run on the next available tick.
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
    protected virtual void OnStart(OmoriBehaviourProjectile projectile, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// A hook method to allow you to write AI. Make sure to edit <paramref name="behaviourInfo"/> in this function.
    /// Runs AFTER <see cref="GuaranteedAI(OmoriBehaviourProjectile, BehaviourInfo)"/>
    /// </summary>
    /// <returns>Void</returns>
    protected virtual void AI(OmoriBehaviourProjectile projectile, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// A hook method to allow you to write AI. Ran regardless of whether the <see cref="_subBehaviour"/>
    /// is executing or not. Runs BEFORE <see cref="AI(OmoriBehaviourProjectile, BehaviourInfo)"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="behaviourInfo"></param>
    protected virtual void GuaranteedAI(OmoriBehaviourProjectile projectile, BehaviourInfo behaviourInfo) { }

    /// <summary>
    /// hook method to allow you to pick your frame from the projectile. Use <paramref name="frameHeight"/> to determine frame height
    /// and <see cref="CurrentFrame"/> for frame info. Set frame info in <see cref="behaviourInfo"/> in order for it to 
    /// carry over to the next <see cref="NPCBehaviour"/>
    /// </summary>
    /// <param name="frameHeight"></param>
    protected virtual void FindFrame(OmoriBehaviourProjectile projectile, BehaviourInfo behaviourInfo, int frameHeight) { }


    private void Start(OmoriBehaviourProjectile projectile, BehaviourInfo info)
    {
        _inProgress = true;
        _hasStarted = true;
        info.ExitStatus = -1;
        OnStart(projectile, info);
    }

    /// <summary>
    /// General method to run an AI.
    /// </summary>
    /// <param name="ai">The AI method being called</param>
    /// <param name="projectile">The <see cref="OmoriBehaviourProjectile"/> this is being performed on</param>
    /// <param name="info">The <see cref="BehaviourInfo"/>.</param>
    /// <param name="isSubBehaviour">True if this AI happends to be running on a subbehaviour</param>
    private bool RunAI(Action<OmoriBehaviourProjectile, BehaviourInfo> ai, OmoriBehaviourProjectile projectile, BehaviourInfo info, bool isSubBehaviour)
    {
        ai?.Invoke(projectile, info);
        if (info.ExitStatus >= 0)
        {
            // if exiting, reset these values
            _runSubBehaviour = false;
            _subBehaviour?.Reset();

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
    /// Call this method to use your AI. Performs the <see cref="AI(OmoriBehaviourProjectile, BehaviourInfo)"/> method.
    /// This will also perform the <see cref="GuarenteedAI(OmoriBehaviourProjectile, BehaviourInfo)"/> every tick.
    /// The <see cref="AI(OmoriBehaviourProjectile, BehaviourInfo)"/> method will NOT be run if the <see cref="_subBehaviour"/> is running.
    /// When the submethod is finished, the <see cref="BehaviourInfo"/> <paramref name="info"/> will be reset to the 
    /// value it was before the submethod started running.
    /// </summary>
    public void PerformAI(OmoriBehaviourProjectile projectile, BehaviourInfo info)
    {
        if (IsDone)
        {
            _justCompleted = false;
            return;
        }

        if (!_hasStarted)
        {
            Start(projectile, info);
        }

        if (RunAI(GuaranteedAI, projectile, info, false)) { return; }
        if (_runSubBehaviour)
        {
            RunAI(_subBehaviour.PerformAI, projectile, info, true);
        }
        else
        {
            if (RunAI(AI, projectile, info, false)) { return; }
        }
    }

    /// <summary>
    /// Call this method to animate your <paramref name="projectile"/>.
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="behaviourInfo"></param>
    /// <param name="frameHeight"></param>
    public void PerformFindFrame(OmoriBehaviourProjectile projectile, BehaviourInfo behaviourInfo, int frameHeight)
    {
        FindFrame(projectile, behaviourInfo, frameHeight);
    }

    public override bool Equals(object obj)
    {
        return obj is ProjectileBehaviour other ? BehaviourName == other.BehaviourName : false;
    }

    public override int GetHashCode()
    {
        return BehaviourName.GetHashCode();
    }
}