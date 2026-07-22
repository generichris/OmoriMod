
namespace OmoriMod.Content.Systems.State_Management.Behaviour_Info;

public class BehaviourInfo
{
    private AIInfo aiInfo;
    private AnimationInfo animationInfo;

    public int ExitStatus { get => aiInfo.ExitStatus; set => aiInfo.ExitStatus = value; }
    public int CurrentFrame { get => animationInfo.GetCurrentFrame(); set => animationInfo.SetCurrentFrame(value); }

    public string SelectedAnimation { get => animationInfo.SelectedAnimation; }
    public BehaviourInfo()
    {
        aiInfo = new AIInfo();
        animationInfo = new AnimationInfo(-1);
    }

    public BehaviourInfo(int totalFrames)
    {
        aiInfo = new AIInfo();
        animationInfo = new AnimationInfo(totalFrames);
    }

    public BehaviourInfo(BehaviourInfo other)
    {
        aiInfo = other.aiInfo.Copy();
        animationInfo = other.animationInfo.Copy();
    }

    public void ResetExitStatus()
    {
        ExitStatus = -2;
    }

    public BehaviourInfo Copy()
    {
        return new BehaviourInfo(this);
    }

    public void Set(BehaviourInfo info)
    {
        aiInfo = info.aiInfo.Copy();
        animationInfo = info.animationInfo.Copy();
    }

    public bool AddAnimation(string name, EntityAnimation iter)
    {
        return animationInfo.AddAnimation(name, iter);
    }

    public bool SelectAnimation(string animationName)
    {
        return animationInfo.SelectAnimation(animationName);
    }

    /// <summary>
    /// Used to increment the animation
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static BehaviourInfo operator ++(BehaviourInfo b)
    {
        b.animationInfo++;
        return b;
    }
}