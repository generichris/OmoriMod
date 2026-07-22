using System;
using System.Collections.Generic;

using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;
using OmoriMod.Content.Util;

namespace OmoriMod.Content.Systems.State_Management.NPCs;

/// <summary>
/// Manages and executes AI behaviours for an <see cref="OmoriBehaviourNPC"/>. 
/// <para>
/// This class allows for the scheduling, selection, and execution of multiple types of behaviours:
/// </para>
/// <list type="bullet">
/// <item>
/// <description><see cref="NPCBehaviour"/> instances, which represent the main actions the NPC can perform.</description>
/// </item>
/// <item>
/// <description><see cref="NPCBackgroundBehaviour"/> instances, which run every tick regardless of the current main behaviour.</description>
/// </item>
/// <item>
/// <description>An idle behaviour that executes when the NPC is between active behaviours.</description>
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
public class NPCBehaviourManager(OmoriBehaviourNPC npc, int totalFrames)
{
    /// <summary>
    /// The currently selected <see cref="NPCBehaviour"/> that this manager is running.
    /// </summary>
    public NPCBehaviour SelectedBehaviour { get => selectedBehaviour; }

    /// <summary>
    /// The internal reference to the currently selected behaviour. Use <see cref="SelectedBehaviour"/> to access publicly.
    /// </summary>
    private NPCBehaviour selectedBehaviour = null;

    /// <summary>
    /// The index of the currently selected behaviour in <see cref="behaviourList"/>.
    /// </summary>
    private int selectedBehaviourIndex = 0;

    /// <summary>
    /// Holds animation and AI information for the behaviours managed by this class.
    /// </summary>
    private readonly BehaviourInfo behaviourInfo = new(totalFrames);

    /// <summary>
    /// The NPC that this behaviour manager is controlling.
    /// </summary>
    private readonly OmoriBehaviourNPC npc = npc;

    /// <summary>
    /// Timer used to track the duration spent on the idle behaviour between active behaviours.
    /// </summary>
    private TickTimer timeBetweenBehaviours = new();

    /// <summary>
    /// Random number generator used for randomly selecting behaviours.
    /// </summary>
    private readonly Random random = new();
    private readonly List<NPCBehaviour> behaviourList = [];
    private readonly List<NPCBackgroundBehaviour> backgroundBehaviourList = [];
    private NPCBehaviour idleBehaviour = null;


    /// <summary>
    /// Adds a new animation to <see cref="behaviourInfo"/> using a <see cref="EntityAnimation"/>.
    /// <para>
    /// You can select this animation later via <see cref="SelectAnimation(string)"/> by passing in <paramref name="animationName"/>.
    /// </para>
    /// </summary>
    /// <param name="animationName">The name of the animation you are adding.</param>
    /// <param name="animation">The <see cref="EntityAnimation"/> object containing the frame indices.</param>
    /// <returns><c>true</c> if the animation was successfully added, <c>false</c> if an animation with the same name already exists.</returns>
    public bool AddAnimation(string animationName, EntityAnimation animation)
    {
        return behaviourInfo.AddAnimation(animationName, animation);
    }

    /// <summary>
    /// Adds a new animation to <see cref="behaviourInfo"/> using a range of frame indices.
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
    /// Selects the animation to run for the NPC based on its name.
    /// </summary>
    /// <param name="animationName">The name of the animation to select.</param>
    /// <returns><c>true</c> if the animation was successfully selected; <c>false</c> if no animation with that name exists.</returns>
    public bool SelectAnimation(string animationName)
    {
        return behaviourInfo.SelectAnimation(animationName);
    }

    /// <summary>
    /// Adds a new <see cref="NPCBehaviour"/> to the behaviour manager.
    /// </summary>
    /// <param name="behaviour">The behaviour to add to <see cref="behaviourList"/>.</param>
    public void AddBehaviour(NPCBehaviour behaviour)
    {
        behaviourList.Add(behaviour);
    }

    /// <summary>
    /// Sets the idle behaviour of the NPC.
    /// <para>
    /// The idle behaviour runs when no active behaviour is being executed, such as during 
    /// <see cref="timeBetweenBehaviours"/> delays.
    /// </para>
    /// </summary>
    /// <param name="behaviour">The behaviour to set as the idle behaviour.</param>
    public void SetIdleBehaviour(NPCBehaviour behaviour)
    {
        idleBehaviour = behaviour;
    }

    /// <summary>
    /// Adds a <see cref="NPCBackgroundBehaviour"/> to this manager.
    /// </summary>
    /// <param name="behaviour">The <see cref="NPCBackgroundBehaviour"/> to be added to <see cref="backgroundBehaviourList"/></param>
    public void AddBackgroundBehaviour(NPCBackgroundBehaviour behaviour)
    {
        backgroundBehaviourList.Add(behaviour);
    }

    /// <summary>
    /// Sets the duration spent on the idle behaviour between active behaviours for this manager.
    /// </summary>
    /// <param name="tickTimer">The <see cref="TickTimer"/> representing the idle time.</param>
    public void SetTimeBetweenBehaviours(TickTimer tickTimer)
    {
        timeBetweenBehaviours = tickTimer;
    }

    /// <summary>
    /// Resets the current <see cref="selectedBehaviour"/> and selects 
    /// a new one from <see cref="behaviourList"/> at the specified index.
    /// </summary>
    /// <param name="behaviourIndex">
    /// The index of the new behaviour to select.
    /// </param>
    private void SelectNewBehaviour(int behaviourIndex)
    {
        selectedBehaviour.Reset();
        behaviourInfo.ResetExitStatus();
        selectedBehaviour = behaviourList[behaviourIndex];
    }

    /// <summary>
    /// Executes the given <paramref name="selectedBehaviour"/> by calling its 
    /// <see cref="NPCBehaviour.PerformAI(OmoriBehaviourNPC, BehaviourInfo)"/> method.
    /// </summary>
    /// <param name="selectedBehaviour">The behaviour to perform.</param>
    /// <param name="info">The <see cref="BehaviourInfo"/> to pass to the behaviour.</param>
    /// <returns>
    /// <c>true</c> if the behaviour has completed (see <see cref="NPCBehaviour.IsDone"/>), 
    /// otherwise <c>false</c>.
    /// </returns>
    private bool PerformBehaviour(NPCBehaviour selectedBehaviour, BehaviourInfo info)
    {
        if (selectedBehaviour == null) { return true; }
        selectedBehaviour.PerformAI(npc, info);
        return selectedBehaviour.IsDone;
    }

    /// <summary>
    /// Selects a new behaviour at random from <see cref="behaviourList"/>.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAI(Action{bool})"/>, 
    /// and the current <see cref="selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, a new random behaviour will be selected, replacing the current one.
    /// </param>
    private void RandomSelector(bool init)
    {
        if (init) { selectedBehaviour ??= behaviourList[random.Next(behaviourList.Count)]; }
        else
        {
            SelectNewBehaviour(random.Next(behaviourList.Count));
        }
    }

    /// <summary>
    /// Selects a new behaviour from <see cref="behaviourList"/> in order, looping 
    /// back to the beginning when the end is reached.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAI(Action{bool})"/>, 
    /// and the current <see cref="selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, the next behaviour in sequence will be selected.
    /// </param>
    private void InOrderSelector(bool init)
    {
        if (init) { selectedBehaviour ??= behaviourList[selectedBehaviourIndex]; }
        else
        {
            selectedBehaviourIndex = (selectedBehaviourIndex + 1) % behaviourList.Count;
            SelectNewBehaviour(selectedBehaviourIndex);
        }
    }

    /// <summary>
    /// Selects a new behaviour from <see cref="behaviourList"/> based on the 
    /// <see cref="BehaviourInfo.ExitStatus"/> returned by the previously executed behaviour.
    /// </summary>
    /// <param name="init">
    /// If <c>true</c>, this is being called at the start of <see cref="PerformAI(Action{bool})"/>, 
    /// and the current <see cref="selectedBehaviour"/> will be initialized if not already set.
    /// If <c>false</c>, a new behaviour will be selected using the last exit status.
    /// </param>
    private void ExitStatusSelector(bool init)
    {
        if (init) { selectedBehaviour ??= behaviourList[selectedBehaviourIndex]; }
        else
        {
            SelectNewBehaviour(behaviourInfo.ExitStatus);
        }
    }

    /// <summary>
    /// Core AI update loop. 
    /// <para>
    /// Executes all <see cref="backgroundBehaviourList"/> behaviours every tick, then performs the 
    /// <see cref="selectedBehaviour"/>. If the selected behaviour has finished, this method either 
    /// runs <see cref="idleBehaviour"/> (if there is time remaining between behaviours) or selects a new behaviour 
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
    private void PerformAI(Action<bool> selector)
    {
        selector?.Invoke(true);

        foreach (var backgroundBehaviour in backgroundBehaviourList)
        {
            PerformBehaviour(backgroundBehaviour, behaviourInfo);
        }
        if (PerformBehaviour(selectedBehaviour, behaviourInfo))
        {
            if (timeBetweenBehaviours.TotalTicks > 0 && !selectedBehaviour.JustCompleted)
            {
                timeBetweenBehaviours--;
                PerformBehaviour(idleBehaviour, behaviourInfo);
            }
            else if (timeBetweenBehaviours.TotalTicks <= 0)
            {
                selector?.Invoke(false);
            }
        }
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours at random.
    /// <para>
    /// This method randomly selects behaviours from <see cref="behaviourList"/> to run.
    /// </para>
    /// <para>
    /// All <see cref="NPCBackgroundBehaviour"/> in <see cref="backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="selectedBehaviour"/> will run every tick 
    /// once <see cref="timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAIViaRandomBehaviour()
    {
        PerformAI(RandomSelector);
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours in order.
    /// <para>
    /// This method selects behaviours from <see cref="behaviourList"/> in order to run.
    /// </para>
    /// <para>
    /// All <see cref="NPCBackgroundBehaviour"/> in <see cref="backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="selectedBehaviour"/> will run every tick 
    /// once <see cref="timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAIViaInOrderBehaviour()
    {
        PerformAI(InOrderSelector);
    }

    /// <summary>
    /// Starts the behaviour manager, selecting behaviours based on exit status.
    /// <para>
    /// This method selects behaviours from <see cref="behaviourList"/>
    /// based on the previous <see cref="SelectedBehaviour"/> exit status to run.
    /// </para>
    /// <para>
    /// All <see cref="NPCBackgroundBehaviour"/> in <see cref="backgroundBehaviourList"/> 
    /// will run every tick, regardless of what other behaviour is active.
    /// </para>
    /// <para>
    /// The currently <see cref="selectedBehaviour"/> will run every tick 
    /// once <see cref="timeBetweenBehaviours"/> has finished counting down.
    /// </para>
    /// <para>
    /// When <see cref="timeBetweenBehaviours"/> is still counting down, 
    /// the <see cref="idleBehaviour"/> will run instead.
    /// </para>
    /// </summary>
    public void PerformAIViaExitStatus()
    {
        PerformAI(ExitStatusSelector);
    }

    /// <summary>
    /// Determines which animation frame should be rendered for the currently 
    /// <see cref="selectedBehaviour"/>.
    /// <para>
    /// This method delegates frame selection to the active behaviour's 
    /// <see cref="NPCBehaviour.FindFrame(OmoriBehaviourNPC, BehaviourInfo, int)"/> implementation, allowing each behaviour to control 
    /// its own animation state.
    /// </para>
    /// </summary>
    /// <param name="frameHeight">
    /// The height (in pixels) of a single animation frame, used to calculate 
    /// which portion of the sprite sheet to display.
    /// </param>
    public void PerformFindFrame(int frameHeight)
    {
        selectedBehaviour.PerformFindFrame(npc, behaviourInfo, frameHeight);
    }
}