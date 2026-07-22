using System;
using System.Collections.Generic;

using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.Projectiles.Projectile_Behaviour;
using OmoriMod.Content.Util;

namespace OmoriMod.Content.Systems.State_Management.Projectiles;

/// <summary>
/// Manages and executes AI behaviours for an <see cref="OmoriBehaviourProjectile"/>. 
/// <para>
/// This class allows for the scheduling, selection, and execution of multiple types of behaviours:
/// </para>
/// <list type="bullet">
/// <item>
/// <description><see cref="ProjectileBehaviour"/> instances, which represent the main actions the projectile can perform.</description>
/// </item>
/// <item>
/// <description><see cref="ProjectileBackgroundBehaviour"/> instances, which run every tick regardless of the current main behaviour.</description>
/// </item>
/// <item>
/// <description>An idle behaviour that executes when the projectile is between active behaviours.</description>
/// </item>
/// </list>
/// <para>
/// Behaviours can be selected using different strategies: random selection, in-order sequencing, or based on the exit status of the previously executed behaviour. 
/// The manager also tracks time spent on idle behaviour between active behaviours using a <see cref="TickTimer"/>.
/// </para>
/// <para>
/// The class integrates with <see cref="BehaviourInfo"/> to manage both animation and AI state for each behaviour.
/// </para>
/// </summary>
public class ProjectileBehaviourManager(OmoriBehaviourProjectile projectile, int totalFrames)
{
    /// <summary>
    /// The currently selected <see cref="ProjectileBehaviour"/> that this manager is running.
    /// </summary>
    public ProjectileBehaviour SelectedBehaviour { get => _selectedBehaviour; }

    /// <summary>
    /// The internal reference to the currently selected behaviour. Use <see cref="SelectedBehaviour"/> to access publicly.
    /// </summary>
    private ProjectileBehaviour _selectedBehaviour;

    /// <summary>
    /// The index of the currently selected behaviour in <see cref="_behaviourList"/>.
    /// </summary>
    private int _selectedBehaviourIndex;

    /// <summary>
    /// Holds animation and AI information for the behaviours managed by this class.
    /// </summary>
    private readonly BehaviourInfo _behaviourInfo = new(totalFrames);

    /// <summary>
    /// The projectile that this behaviour manager is controlling.
    /// </summary>
    private readonly OmoriBehaviourProjectile _projectile = projectile;

    /// <summary>
    /// Timer used to track the duration spent on the idle behaviour between active behaviours.
    /// </summary>
    private TickTimer _timeBetweenBehaviours = new();

    /// <summary>
    /// Random number generator used for randomly selecting behaviours.
    /// </summary>
    private readonly Random _random = new();
    private readonly List<ProjectileBehaviour> _behaviourList = [];
    private readonly List<ProjectileBackgroundBehaviour> _backgroundBehaviourList = [];
    private ProjectileBehaviour _idleBehaviour;


    /// <summary>
    /// Adds a new animation to <see cref="_behaviourInfo"/> using a <see cref="EntityAnimation"/>.
    /// <para>
    /// You can select this animation later via <see cref="SelectAnimation(string)"/> by passing in <paramref name="animationName"/>.
    /// </para>
    /// </summary>
    /// <param name="animationName">The name of the animation you are adding.</param>
    /// <param name="animation">The <see cref="EntityAnimation"/> object containing the frame indices.</param>
    /// <returns><c>true</c> if the animation was successfully added, <c>false</c> if an animation with the same name already exists.</returns>
    public bool AddAnimation(string animationName, EntityAnimation animation)
    {
        return _behaviourInfo.AddAnimation(animationName, animation);
    }

    /// <summary>
    /// Adds a new animation to <see cref="_behaviourInfo"/> using a range of frame indices.
    /// <para>
    /// You can select this animation later via <see cref="SelectAnimation(string)"/> by passing in <paramref name="animationName"/>.
    /// </para>
    /// </summary>
    /// <param name="animationName">The name of the animation you are adding.</param>
    /// <param name="beginIndex">The starting frame index of the animation.</param>
    /// <param name="endIndex">The ending frame index of the animation.</param>
    /// <returns><c>true</c> if the animation was successfully added, <c>false</c> if an animation with the same name already exists.</returns>
    public bool AddAnimation(string animationName, int beginIndex, int endIndex)
    {
        return AddAnimation(animationName, new EntityAnimation(beginIndex, endIndex));
    }


    /// <summary>
    /// Selects the animation to run for the projectile based on its name.
    /// </summary>
    /// <param name="animationName">The name of the animation to select.</param>
    /// <returns><c>true</c> if the animation was successfully selected; <c>false</c> if no animation with that name exists.</returns>
    public bool SelectAnimation(string animationName)
    {
        return _behaviourInfo.SelectAnimation(animationName);
    }

    /// <summary>
    /// Adds a new <see cref="ProjectileBehaviour"/> to the behaviour manager.
    /// </summary>
    /// <param name="behaviour">The behaviour to add to <see cref="_behaviourList"/>.</param>
    public void AddBehaviour(ProjectileBehaviour behaviour)
    {
        _behaviourList.Add(behaviour);
    }

    /// <summary>
    /// Sets the idle behaviour of the projectile.
    /// <para>
    /// The idle behaviour runs when no active behaviour is being executed, such as during 
    /// <see cref="_timeBetweenBehaviours"/> delays.
    /// </para>
    /// </summary>
    /// <param name="behaviour">The behaviour to set as the idle behaviour.</param>
    public void SetIdleBehaviour(ProjectileBehaviour behaviour)
    {
        _idleBehaviour = behaviour;
    }

    /// <summary>
    /// Adds a <see cref="ProjectileBackgroundBehaviour"/> to this manager.
    /// </summary>
    /// <param name="behaviour">The <see cref="ProjectileBackgroundBehaviour"/> to be added to <see cref="_backgroundBehaviourList"/></param>
    public void AddBackgroundBehaviour(ProjectileBackgroundBehaviour behaviour)
    {
        _backgroundBehaviourList.Add(behaviour);
    }

    /// <summary>
    /// Sets the duration spent on the idle behaviour between active behaviours for this manager.
    /// </summary>
    /// <param name="tickTimer">The <see cref="TickTimer"/> representing the idle time.</param>
    public void SetTimeBetweenBehaviours(TickTimer tickTimer)
    {
        _timeBetweenBehaviours = tickTimer;
    }

    /// <summary>
    /// Resets the current <see cref="_selectedBehaviour"/> and selects 
    /// a new one from <see cref="_behaviourList"/> at the specified index.
    /// </summary>
    /// <param name="behaviourIndex">
    /// The index of the new behaviour to select.
    /// </param>
    private void SelectNewBehaviour(int behaviourIndex)
    {
        _selectedBehaviour.Reset();
        _behaviourInfo.ResetExitStatus();
        _selectedBehaviour = _behaviourList[behaviourIndex];
    }

    /// <summary>
    /// Executes the given <paramref name="selectedBehaviour"/> by calling its 
    /// <see cref="ProjectileBehaviour.PerformAI(OmoriBehaviourProjectile, BehaviourInfo)"/> method.
    /// </summary>
    /// <param name="selectedBehaviour">The behaviour to perform.</param>
    /// <param name="info">The <see cref="BehaviourInfo"/> to pass to the behaviour.</param>
    /// <returns>
    /// <c>true</c> if the behaviour has completed (see <see cref="ProjectileBehaviour.IsDone"/>), 
    /// otherwise <c>false</c>.
    /// </returns>
    private bool PerformBehaviour(ProjectileBehaviour selectedBehaviour, BehaviourInfo info)
    {
        if (selectedBehaviour == null) { return true; }
        selectedBehaviour.PerformAI(_projectile, info);
        return selectedBehaviour.IsDone;
    }

    /// <summary>
    /// Selects a new behaviour at random from <see cref="_behaviourList"/>.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAi"/>, 
    /// and the current <see cref="_selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, a new random behaviour will be selected, replacing the current one.
    /// </param>
    private void RandomSelector(bool init)
    {
        if (init) { _selectedBehaviour ??= _behaviourList[_random.Next(_behaviourList.Count)]; }
        else
        {
            SelectNewBehaviour(_random.Next(_behaviourList.Count));
        }
    }

    /// <summary>
    /// Selects a new behaviour from <see cref="_behaviourList"/> in order, looping 
    /// back to the beginning when the end is reached.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAi"/>, 
    /// and the current <see cref="_selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, the next behaviour in sequence will be selected.
    /// </param>
    private void InOrderSelector(bool init)
    {
        if (init) { _selectedBehaviour ??= _behaviourList[_selectedBehaviourIndex]; }
        else
        {
            _selectedBehaviourIndex = (_selectedBehaviourIndex + 1) % _behaviourList.Count;
            SelectNewBehaviour(_selectedBehaviourIndex);
        }
    }

    /// <summary>
    /// Selects a new behaviour from <see cref="_behaviourList"/> based on the 
    /// <see cref="BehaviourInfo.ExitStatus"/> returned by the previously executed behaviour.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAi"/>, 
    /// and the current <see cref="_selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, a new behaviour will be selected using the last exit status.
    /// </param>
    private void ExitStatusSelector(bool init)
    {
        if (init) { _selectedBehaviour ??= _behaviourList[_selectedBehaviourIndex]; }
        else
        {
            SelectNewBehaviour(_behaviourInfo.ExitStatus);
        }
    }

    /// <summary>
    /// Core AI update loop. 
    /// <para>
    /// Executes all <see cref="_backgroundBehaviourList"/> behaviours every tick, then performs the 
    /// <see cref="_selectedBehaviour"/>. If the selected behaviour has finished, this method either 
    /// runs <see cref="_idleBehaviour"/> (if there is time remaining between behaviours) or selects a new behaviour 
    /// using the provided <paramref name="selector"/>.
    /// </para>
    /// </summary>
    /// <param name="selector">
    /// The behaviour selection strategy to use (random, in order, or based on exit status).
    /// This action is called twice:
    /// <list type="bullet">
    /// <item><description>Once at the beginning (with <c>true</c>) to initialize the selected behaviour.</description></item>
    /// <item><description>Once after the current behaviour completes (with <c>false</c>) to select the next behaviour.</description></item>
    /// </list>
    /// </param>
    private void PerformAi(Action<bool> selector)
    {
        selector?.Invoke(true);

        foreach (var backgroundBehaviour in _backgroundBehaviourList)
        {
            PerformBehaviour(backgroundBehaviour, _behaviourInfo);
        }
        if (PerformBehaviour(_selectedBehaviour, _behaviourInfo))
        {
            if (_timeBetweenBehaviours.TotalTicks > 0 && !_selectedBehaviour.JustCompleted)
            {
                _timeBetweenBehaviours--;
                PerformBehaviour(_idleBehaviour, _behaviourInfo);
            }
            else if (_timeBetweenBehaviours.TotalTicks <= 0)
            {
                selector?.Invoke(false);
            }
        }
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours at random.
    /// <para>
    /// This method randomly selects behaviours from <see cref="_behaviourList"/> to run.
    /// </para>
    /// <para>
    /// All <see cref="ProjectileBackgroundBehaviour"/> in <see cref="_backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="_selectedBehaviour"/> will run every tick 
    /// once <see cref="_timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="_timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="_idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAiViaRandomBehaviour()
    {
        PerformAi(RandomSelector);
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours in order.
    /// <para>
    /// This method selects behaviours from <see cref="_behaviourList"/> in order to run.
    /// </para>
    /// <para>
    /// All <see cref="ProjectileBackgroundBehaviour"/> in <see cref="_backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="_selectedBehaviour"/> will run every tick 
    /// once <see cref="_timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="_timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="_idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAiViaInOrderBehaviour()
    {
        PerformAi(InOrderSelector);
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours based on exit status.
    /// <para>
    /// This method selects behaviours from <see cref="_behaviourList"/>
    /// based on the previous <see cref="SelectedBehaviour"/> exit status to run.
    /// </para>
    /// <para>
    /// All <see cref="ProjectileBackgroundBehaviour"/> in <see cref="_backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="_selectedBehaviour"/> will run every tick 
    /// once <see cref="_timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="_timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="_idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAiViaExitStatus()
    {
        PerformAi(ExitStatusSelector);
    }

    /// <summary>
    /// Determines which animation frame should be rendered for the currently 
    /// <see cref="_selectedBehaviour"/>.
    /// <para>
    /// This method delegates frame selection to the active behaviour's 
    /// <see cref="ProjectileBehaviour.FindFrame(OmoriBehaviourProjectile, BehaviourInfo, int)"/> implementation, allowing each behaviour to control 
    /// its own animation state.
    /// </para>
    /// </summary>
    /// <param name="frameHeight">
    /// The height (in pixels) of a single animation frame, used to calculate 
    /// which portion of the sprite sheet to display.
    /// </param>
    public void PerformFindFrame(int frameHeight)
    {
        _selectedBehaviour.PerformFindFrame(_projectile, _behaviourInfo, frameHeight);
    }
}