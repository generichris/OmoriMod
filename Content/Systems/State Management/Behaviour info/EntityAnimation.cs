
namespace OmoriMod.Content.Systems.State_Management.Behaviour_Info;

public class EntityAnimation
{
    private readonly int _beginningIndex;
    private readonly int _endingIndex;

    public int CurrentFrame { get => _currentIndex; set => _currentIndex = value; }
    private int _currentIndex;

    public EntityAnimation(int begin, int end)
    {
        _beginningIndex = begin;
        _currentIndex = begin;
        _endingIndex = end;
    }

    public EntityAnimation(int end)
    {
        _beginningIndex = 0;
        _currentIndex = 0;
        _endingIndex = end;
    }

    public EntityAnimation(EntityAnimation animation)
    {
        _beginningIndex = animation._beginningIndex;
        _currentIndex = animation._currentIndex;
        _endingIndex = animation._endingIndex;
    }

    public void Reset()
    {
        _currentIndex = _beginningIndex;
    }

    public bool ContainsIndex(int index)
    {
        return _beginningIndex <= index || index <= _endingIndex;
    }

    public EntityAnimation Copy()
    {
        return new EntityAnimation(this);
    }


    public static EntityAnimation operator ++(EntityAnimation f)
    {
        if (f._currentIndex < f._endingIndex)
        {
            f._currentIndex++;
        }
        else
        {
            f._currentIndex = f._beginningIndex;
        }
        return f;
    }
}