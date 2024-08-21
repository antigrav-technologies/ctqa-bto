using Discord;

namespace CtqaBto;

public static class Achievements {
    public enum AchievementId {
        Unknown = -1,
        // Ctqa Hunt
        FirstCtqa = 0,
        Donator = 1,
        AntiDonator = 2,
        FastCatcher = 3,
        SlowCatcher = 4,
        GetAllCtqas = 5,
        // Random
        DMBot = 6,
        PingBot = 7,
        PleaseDoTheCtqa = 8,
        PleaseDoNotTheCtqa = 9,
        Worship = 10,
        Holy = 11,
        Trolled = 12,
        IAmATeapot = 418,
        NotQuite = 13,
        CTQA = 14,
        NOTCTQA = 15,
        // Hidden
        Datamine = 16,
        Сейф = 17
    }
    public enum AchievementCategory {
        Unknown = -1,
        CtqaHunt = 0,
        Random = 1,
        Unfair = 2,
        Secret = 100
    }
    public readonly struct Achievement(AchievementId id, AchievementCategory category, string name, string description, string? descriptionIfNotUnlocked = null, string? descriptionWhenGot = null) {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string DescriptionIfNotUnlocked { get; } = descriptionIfNotUnlocked ?? description;
        public string DescrptionWhenGot { get; } = descriptionWhenGot ?? description;
        public AchievementId Id { get; } = id;
        public AchievementCategory Category { get; } = category;
    }

    private static readonly Achievement[] Achs = [
        new Achievement(AchievementId.Unknown, AchievementCategory.Unknown, "this is an achievement test", "курсор в слинкс атик"),
        new Achievement(AchievementId.FirstCtqa, AchievementCategory.CtqaHunt, "First ctqa!", "Catch your first ctqa", "The journey begins..."),
        new Achievement(AchievementId.Donator, AchievementCategory.CtqaHunt, "Donator", "Gift your ctqas to someone"),
        new Achievement(AchievementId.AntiDonator, AchievementCategory.CtqaHunt, "Anti-donator", "Get donated to"),
        new Achievement(AchievementId.FastCatcher, AchievementCategory.CtqaHunt, "Fast Catcher", "Catch a ctqa in under 5 seconds"),
        new Achievement(AchievementId.SlowCatcher, AchievementCategory.CtqaHunt, "Slow Catcher", "Catch a ctqa in over a hour"),
        new Achievement(AchievementId.GetAllCtqas, AchievementCategory.CtqaHunt, "Collecter", "Get all ctqa types"),
        new Achievement(AchievementId.DMBot, AchievementCategory.Random, "DM", "dm ctqa", "???"),
        new Achievement(AchievementId.PingBot, AchievementCategory.Random, "who ping", "ping ctqa bto", "???"),
        new Achievement(AchievementId.PleaseDoTheCtqa, AchievementCategory.Random, "please do the ctqa", "Say \"please do the ctqa\"", "accept ctqa"),
        new Achievement(AchievementId.PleaseDoNotTheCtqa, AchievementCategory.Random, "please do not the ctqa", "Say \"please do not the ctqa\"", "accept ctqa"),
        new Achievement(AchievementId.Worship, AchievementCategory.Random, "Ctqa Worshiper", "Have both 🛐 and <:syating_ctqa:1178288745435385896> in your message", "worship ctqa only"),
        new Achievement(AchievementId.Holy, AchievementCategory.Random, "Holy ctqa", "Run /holy", "holy"),
        new Achievement(AchievementId.Trolled, AchievementCategory.Random, "trolled", "actually idfk", "LMAO GOT TROLLED SO FUNNY LMAO 😂😂😂"),
        new Achievement(AchievementId.IAmATeapot, AchievementCategory.Random, "Brew coffee", "Run /brew", "418"),
        new Achievement(AchievementId.NotQuite, AchievementCategory.Random, "Not Quite", "Say \"cat\" in channel where ctqa is sitting already", "wrong bot"),
        new Achievement(AchievementId.CTQA, AchievementCategory.Random, "Calm Down!", "Say \"CTQA\"", "???"),
        new Achievement(AchievementId.NOTCTQA, AchievementCategory.Random, "new JTextComponent();", "Say uppercased what you wanted to catch", "now this gets interesting"),
        new Achievement(AchievementId.Datamine, AchievementCategory.Random, "It's a mystery to nobody.", $"say `{Data.Datamine}`", "bushes hid the description 😔😔😂😂"),
        new Achievement(AchievementId.Сейф, AchievementCategory.Random, "сейф 🔐🛡️🦈🦈😂 и не знаешь где 😼😼😼", "Say сейф")
    ];
    public static Embed MakeAchEmbed(AchievementId id, string username) => new EmbedBuilder() {
        Title = "<:ctqa_trophy:1200918336444309524> New achievement!",
        Fields = [new EmbedFieldBuilder() { Name = GetAch(id).Name, Value = GetAch(id).Description }],
        Footer = new() { Text = $"Unlocked by @{username}" }
    }.Build();
    public static Achievement GetAch(AchievementId id) => Achs.First(x => x.Id == id);
    public static IEnumerable<Achievement> GetAchs(AchievementCategory category) => Achs.Where(x => x.Category == category);
}
