using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionItem.Emotion"/> to <see cref="EmotionType.Happy"/>.
/// </summary>
public abstract class HappyItem : EmotionItem
{
    protected HappyItem()
    {
        SetEmotionType(EmotionType.Happy);
    }
}
