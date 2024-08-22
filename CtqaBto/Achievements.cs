using Discord;
using static CtqaBto.Utils;

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
        CtqaHunt = 0,
        Random = 1,
        Unfair = 2,
        Secret = 3
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
        new Achievement(AchievementId.Unknown, AchievementCategory.Secret,            "this is an achievement test",               "курсор в слинкс атик"),
        new Achievement(AchievementId.FirstCtqa, AchievementCategory.CtqaHunt,        "First ctqa!",                               "Catch your first ctqa", "The journey begins..."),
        new Achievement(AchievementId.Donator, AchievementCategory.CtqaHunt,          "Donator",                                   "Gift your ctqas to someone"),
        new Achievement(AchievementId.AntiDonator, AchievementCategory.CtqaHunt,      "Anti-donator",                              "Get donated to", "get rich!!!!!111!!!"),
        new Achievement(AchievementId.FastCatcher, AchievementCategory.CtqaHunt,      "Fast Catcher",                              "Catch a ctqa in under 5 seconds"),
        new Achievement(AchievementId.SlowCatcher, AchievementCategory.CtqaHunt,      "Slow Catcher",                              "Catch a ctqa in over a hour"),
        new Achievement(AchievementId.GetAllCtqas, AchievementCategory.CtqaHunt,      "Collecter",                                 "Get all ctqa types"),
        new Achievement(AchievementId.DMBot, AchievementCategory.Random,              "DM",                                        "dm ctqa", "???"),
        new Achievement(AchievementId.PingBot, AchievementCategory.Random,            "who ping",                                  "ping ctqa bto", "???"),
        new Achievement(AchievementId.PleaseDoTheCtqa, AchievementCategory.Random,    "please do the ctqa",                        "Say \"please do the ctqa\"", "big success!!!!!", "accept ctqa"),
        new Achievement(AchievementId.PleaseDoNotTheCtqa, AchievementCategory.Random, "please do not the ctqa",                    "Say \"please do not the ctqa\"", "huge mistake!!!!!", "reject ctqa"),
        new Achievement(AchievementId.Worship, AchievementCategory.Random,            "Ctqa Worshiper",                            "Have both 🛐 and <:syating_ctqa:1178288745435385896> in your message", "worship ctqa only"),
        new Achievement(AchievementId.Holy, AchievementCategory.Random,               "Holy ctqa",                                 "Run /holy", "holy"),
        new Achievement(AchievementId.Trolled, AchievementCategory.Random,            "trolled",                                   "actually idfk", "LMAO GOT TROLLED SO FUNNY LMAO 😂😂😂"),
        new Achievement(AchievementId.IAmATeapot, AchievementCategory.Random,         "Brew coffee",                               "Run /brew", "418"),
        new Achievement(AchievementId.NotQuite, AchievementCategory.Random,           "Not Quite",                                 "Say \"cat\" in channel where ctqa is sitting already", "wrong bot"),
        new Achievement(AchievementId.CTQA, AchievementCategory.Random,               "Calm Down!",                                "Say \"CTQA\"", "???"),
        new Achievement(AchievementId.NOTCTQA, AchievementCategory.Random,            "new JTextComponent();",                     "Say uppercased what you wanted to catch", "now this gets interesting"),
        new Achievement(AchievementId.Datamine, AchievementCategory.Secret,           "It's a mystery to nobody.",                $"say `{Data.Datamine}`", "bushes hid the description 😔😔😂😂", "bushes hid the description 😔😔😂😂"),
        new Achievement(AchievementId.Сейф, AchievementCategory.Secret,               "сейф 🔐🛡️🦈🦈😂 и не знаешь где 😼😼😼", "Say сейф", "???", "Ф цшдв ашту сейф рфы фззуфкув!")
    ];
    public static readonly int TotalNotSecretAchs = Achs.Where(x => x.Category != AchievementCategory.Secret).Count();
    public static Embed MakeAchEmbed(AchievementId id, string username) => new EmbedBuilder() {
        Title = "<:ctqa_trophy:1200918336444309524> New achievement!",
        Fields = [new() { Name = GetAch(id).Name, Value = GetAch(id).DescrptionWhenGot }],
        Footer = new() { Text = $"Unlocked by @{username}" }
    }.Build();
    public static Achievement GetAch(AchievementId id) => Achs.First(x => x.Id == id);
    public static string GetAchsCount(IEnumerable<AchievementId> ids) {
        int nonSecretAchs = ids.Where(id => GetAch(id).Category != AchievementCategory.Secret).Count();
        int secretAchs = ids.Where(id => GetAch(id).Category == AchievementCategory.Secret).Count();
        return $"{nonSecretAchs}/{TotalNotSecretAchs}" + (secretAchs > 0 ? $" + {secretAchs}" : "");
    }
    public static string GetAchsCountStatic(ulong guildId, ulong memberId) {
        var ids = Inventory.Load(guildId, memberId).Achievements;
        int nonSecretAchs = ids.Where(id => GetAch(id).Category != AchievementCategory.Secret).Count();
        int secretAchs = ids.Where(id => GetAch(id).Category == AchievementCategory.Secret).Count();
        return $"{nonSecretAchs}/{TotalNotSecretAchs}" + (secretAchs > 0 ? $" + {secretAchs}" : "");
    }
    private static string Name(this AchievementCategory category) => category switch {
        AchievementCategory.CtqaHunt => "Ctqa Hunt",
        _ => category.ToString()
    };
    private static IEnumerable<Achievement> GetAchs(AchievementCategory category, List<AchievementId> achievements) => Achs.Where(x => (category != AchievementCategory.Secret && x.Category == category) || (x.Category == AchievementCategory.Secret && achievements.Contains(x.Id)));
    public static MessageComponent GetAchComponents(AchievementCategory category) => MakeComponents(Enum.GetValues<AchievementCategory>().Select(e => new Button(category == e ? "Refresh" : e.Name(), $"UPDATEACHS;{(int)e}", category == e ? ButtonStyle.Success : ButtonStyle.Primary)));
    public static Embed GetAchEmbed(ulong guildId, ulong memberId, AchievementCategory category) {
        var inv = Inventory.Load(guildId, memberId);
        return new EmbedBuilder() {
            Title = "Your achievements",
            Description = $"Achievements unlocked: {GetAchsCount(inv.Achievements)}",
            Fields = GetAchs(category, inv.Achievements).Select(ach => new EmbedFieldBuilder() { Name = $"{(inv.Achievements.Contains(ach.Id) ? Data.TrophyUnlocked : Data.TrophyLocked)} {ach.Name}", Value = inv.Achievements.Contains(ach.Id) ? ach.Description : ach.DescriptionIfNotUnlocked, IsInline = true }).ToList()
        }.Build();
    }
}
