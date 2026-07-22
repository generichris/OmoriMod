
namespace OmoriMod.Content.Systems.State_Management.Behaviour_Info;

/// <summary>
/// Manages <see cref="BehaviourInfo.ExitStatus"/>
/// </summary>
public class AIInfo
{
    private int _exitStatus;

    public int ExitStatus { get => _exitStatus; set => _exitStatus = value; }

    public AIInfo()
    {
        _exitStatus = -2;
    }

    public AIInfo(int exitStatus)
    {
        _exitStatus = exitStatus;
    }

    public AIInfo Copy()
    {
        return new AIInfo(_exitStatus);
    }
}