using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionItem.Emotion"/> to <see cref="EmotionType.Sad"/>.
/// </summary>
public abstract class SadItem : EmotionItem
{
    protected SadItem()
    {
        SetEmotionType(EmotionType.Sad);
    }
}
