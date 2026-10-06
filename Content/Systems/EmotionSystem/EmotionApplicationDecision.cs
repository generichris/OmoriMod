namespace OmoriMod.Content.Systems.EmotionSystem;

internal enum EmotionApplicationRequest
{
    RegularItem,
    Amplifier
}

internal enum EmotionApplicationAction
{
    Block,
    ApplyTierOne,
    RefreshCurrent,
    PromoteNextTier,
    AmplifyCurrent
}

internal readonly record struct EmotionApplicationDecision(
    EmotionApplicationAction Action,
    int? BuffType)
{
    internal bool CanApply => Action != EmotionApplicationAction.Block;

    internal static EmotionApplicationDecision Blocked { get; } =
        new(EmotionApplicationAction.Block, null);
}