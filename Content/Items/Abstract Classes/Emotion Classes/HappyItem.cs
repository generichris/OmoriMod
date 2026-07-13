using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionItem.Emotion"/> to <see cref="EmotionType.HAPPY"/>.
/// </summary>
public abstract class HappyItem : EmotionItem
{
    public HappyItem()
    {
        SetEmotionType(EmotionType.HAPPY);
    }
}