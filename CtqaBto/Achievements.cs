namespace CtqaBto;

public enum AchievementCategory {
    CtqaHunt,
    Random,
    Unfair,
    Secret
}

public readonly struct Achievement(AchievementCategory category, string name, string description, string? descriptionIfNotUnlocked = null) {
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string DescriptionIfNotUnlocked { get; } = descriptionIfNotUnlocked ?? description;
    public AchievementCategory Category { get; } = category;
}

public static class Achievements {
    public static void 
    public static Achievement[] GetAchs(string category) {
        int lowerBound = category
        .Where();
    }

    public enum AchievementType {
        // [0, 1000): Ctqa Hunt
        FirstCtqa = 0,
        Donator = 1,
        AntiDonator = 2,
        FastCatcher = 3,
        SlowCatcher = 4,
        GetAllCtqas = 5,
        // [1000, 2000): Random
        // [2000, 3000): Unfair, Reserved
        // [3000, 4000): Hidden 
    }
}
