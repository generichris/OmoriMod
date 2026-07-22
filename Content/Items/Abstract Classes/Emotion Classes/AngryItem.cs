using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionItem.Emotion"/> to <see cref="EmotionType.Angry"/>.
/// </summary>
public abstract class AngryItem : EmotionItem
{
    protected AngryItem()
    {
        SetEmotionType(EmotionType.Angry);
    }
}
