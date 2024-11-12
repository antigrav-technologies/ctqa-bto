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
        DmBot = 6,
        PingBot = 7,
        PleaseDoTheCtqa = 8,
        PleaseDoNotTheCtqa = 9,
        Worship = 10,
        Holy = 11,
        Trolled = 12,
        NotQuite = 13,
        CTQA = 14,
        NOTCTQA = 15,
        // Unfair
        IAmATeapot = 18,
        GetAllStatusCodes = 19,
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
    private readonly struct Achievement(AchievementId id, AchievementCategory category, string name, string description, string? descriptionIfNotUnlocked = null, string? descriptionWhenGot = null) {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string DescriptionIfNotUnlocked { get; } = descriptionIfNotUnlocked ?? description;
        public string DescriptionWhenGot { get; } = descriptionWhenGot ?? description;
        public AchievementId Id { get; } = id;
        public AchievementCategory Category { get; } = category;
    }
    private static readonly Achievement[] Achs = [
        new(AchievementId.Unknown, AchievementCategory.Secret,            "this is an achievement test",               "курсор в слинкс атик"),
        new(AchievementId.FirstCtqa, AchievementCategory.CtqaHunt,        "First ctqa!",                               "Catch your first ctqa", "The journey begins..."),
        new(AchievementId.Donator, AchievementCategory.CtqaHunt,          "Donator",                                   "Gift your ctqas to someone"),
        new(AchievementId.AntiDonator, AchievementCategory.CtqaHunt,      "Anti-donator",                              "Get donated to", "get rich!!!!!111!!!"),
        new(AchievementId.FastCatcher, AchievementCategory.CtqaHunt,      "Fast Catcher",                              "Catch a ctqa in under 5 seconds"),
        new(AchievementId.SlowCatcher, AchievementCategory.CtqaHunt,      "Slow Catcher",                              "Catch a ctqa in over a hour"),
        new(AchievementId.GetAllCtqas, AchievementCategory.CtqaHunt,      "Collector",                                 "Get all ctqa types"),
        new(AchievementId.DmBot, AchievementCategory.Random,              "DM",                                        "dm ctqa", "???"),
        new(AchievementId.PingBot, AchievementCategory.Random,            "who ping",                                  "ping ctqa bto", "???"),
        new(AchievementId.PleaseDoTheCtqa, AchievementCategory.Random,    "please do the ctqa",                        "Say \"please do the ctqa\"", "big success!!!!!", "accept ctqa"),
        new(AchievementId.PleaseDoNotTheCtqa, AchievementCategory.Random, "please do not the ctqa",                    "Say \"please do not the ctqa\"", "huge mistake!!!!!", "reject ctqa"),
        new(AchievementId.Worship, AchievementCategory.Random,            "Ctqa Worshiper",                            "Have both 🛐 and <:syating_ctqa:1178288745435385896> in your message", "worship ctqa only"),
        new(AchievementId.Holy, AchievementCategory.Random,               "Holy ctqa",                                 "Run /holy", "holy"),
        new(AchievementId.Trolled, AchievementCategory.Random,            "trolled",                                   "actually idfk", "LMAO GOT TROLLED SO FUNNY LMAO 😂😂😂"),
        new(AchievementId.NotQuite, AchievementCategory.Random,           "Not Quite",                                 "Say \"cat\" in channel where ctqa is sitting already", "wrong bot"),
        new(AchievementId.CTQA, AchievementCategory.Random,               "Calm Down!",                                "Say \"CTQA\"", "???"),
        new(AchievementId.NOTCTQA, AchievementCategory.Random,            "new JTextComponent();",                     "Say uppercased what you wanted to catch", "now this gets interesting"),
        new(AchievementId.IAmATeapot, AchievementCategory.Unfair,         "Brew coffee",                               "Get 418 status code in /brew", "418"),
        new(AchievementId.GetAllStatusCodes, AchievementCategory.Unfair,  "Teapot Troubleshooter",                               "Get 418 status code in /brew", "418"),
        new(AchievementId.Datamine, AchievementCategory.Secret,           "It's a mystery to nobody.",                $"say `{Data.Datamine}`", "bushes hid the description 😔😔😂😂", "bushes hid the description 😔😔😂😂"),
        new(AchievementId.Сейф, AchievementCategory.Secret,               "сейф 🔐🛡️🦈🦈😂 и не знаешь где 😼😼😼", "Say сейф", "???", "Ф цшдв ашту сейф рфы фззуфкув!")
    ];
    private static readonly int TotalNotSecretAchs = Achs.Count(x => x.Category != AchievementCategory.Secret);
    public static Embed MakeAchEmbed(AchievementId id, string username) => new EmbedBuilder() {
        Title = $"{GetEmojiString("ctqa_trophy")} New achievement!",
        Fields = [new EmbedFieldBuilder { Name = GetAch(id).Name, Value = GetAch(id).DescriptionWhenGot }],
        Footer = new EmbedFooterBuilder { Text = $"Unlocked by @{username}" }
    }.Build();
    private static Achievement GetAch(AchievementId id) => Achs.First(x => x.Id == id);
    private static string GetAchsCount(IEnumerable<AchievementId> ids) {
        int nonSecretAchs = ids.Count(id => GetAch(id).Category != AchievementCategory.Secret);
        int secretAchs = ids.Count(id => GetAch(id).Category == AchievementCategory.Secret);
        return $"{nonSecretAchs}/{TotalNotSecretAchs}" + (secretAchs > 0 ? $" + {secretAchs}" : "");
    }
    public static string GetAchsCountStatic(ulong guildId, ulong memberId) => GetAchsCount(Inventory.Load(guildId, memberId).Achievements);
    
    private static string Name(this AchievementCategory category) => category switch {
        AchievementCategory.CtqaHunt => "Ctqa Hunt",
        _ => category.ToString()
    };
    private static IEnumerable<Achievement> GetAchs(AchievementCategory category, List<AchievementId> achievements) => Achs.Where(x => (category != AchievementCategory.Secret && x.Category == category) || (x.Category == AchievementCategory.Secret && x.Category == category && achievements.Contains(x.Id)));
    public static MessageComponent GetAchComponents(AchievementCategory category) => MakeComponents(Enum.GetValues<AchievementCategory>().Select(e => new Button(category == e ? "Refresh" : e.Name(), $"UPDATEACHS;{(int)e}", category == e ? ButtonStyle.Success : ButtonStyle.Primary)));
    public static Embed GetAchEmbed(ulong guildId, ulong memberId, AchievementCategory category) {
        var inv = Inventory.Load(guildId, memberId);
        return new EmbedBuilder() {
            Title = "Your achievements",
            Description = $"Achievements unlocked: {GetAchsCount(inv.Achievements)}",
            Fields = GetAchs(category, inv.Achievements).Select(ach => new EmbedFieldBuilder() { Name = $"{GetEmojiString(inv.Achievements.Contains(ach.Id) ? "ctqa_trophy" : "no_ctqa_trophy")} {ach.Name}", Value = inv.Achievements.Contains(ach.Id) ? ach.Description : ach.DescriptionIfNotUnlocked, IsInline = true }).ToList()
        }.Build();
    }
}
