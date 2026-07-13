using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Systems.EmotionSystem;

public interface IEmotionEntity : IEmotionObject
{
    public bool ImmuneToEmotionChange { get; }
    public EmotionBuff ActiveEmotionBuff { get; set; }

    /// <summary>
    /// Calculates which <see cref="IEmotionEntity"/> has the emotional advantage
    /// </summary>
    /// <param name="otherEntity"></param>
    /// <returns><c>true</c> if this <see cref="IEmotionEntity"/> has the advantage, 
    /// <c>false</c> if the other <see cref="IEmotionEntity"/> has the advantage,
    /// and <c>null</c> if there is no advantage
    /// </returns>
    public bool? CheckForAdvantage(IEmotionEntity otherEntity)
    {
        if (Emotion == EmotionType.ANGRY)
        {
            if (otherEntity.Emotion == EmotionType.SAD) return true;
            if (otherEntity.Emotion == EmotionType.HAPPY) return false;
            return null;
        }
        if (Emotion == EmotionType.HAPPY)
        {
            if (otherEntity.Emotion == EmotionType.ANGRY) return true;
            if (otherEntity.Emotion == EmotionType.SAD) return false;
            return null;
        }
        if (Emotion == EmotionType.SAD)
        {
            if (otherEntity.Emotion == EmotionType.HAPPY) return true;
            if (otherEntity.Emotion == EmotionType.ANGRY) return false;
            return null;
        }

        return null;
    }
}