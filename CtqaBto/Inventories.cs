using static CtqaBto.Ctqas;
using static CtqaBto.Utils;
using static CtqaBto.Achievements;
using Antigrav;
using Discord;
using Discord.WebSocket;
using static CtqaBto.Configs;

namespace CtqaBto;

public struct SpawnMessageData(CtqaType type = CtqaType.Unknown, ulong messageId = 0, string sayToCatch = "ctqa") {
    [AntigravSerializable("type")]
    public CtqaType Type { get; private set; } = type;
    [AntigravSerializable("message_id")]
    public ulong MessageId { get; private set; } = messageId;
    [AntigravSerializable("say_to_catch")]
    public string SayToCatch { get; private set; } = sayToCatch;
}

public class Inventory : IDisposable {
    [AntigravSerializable("achs")]
    public List<AchievementId> Achievements = []; // fuck it you can encapsulate objects anyway

    [AntigravSerializable("fastest_catch", double.PositiveInfinity)]
    private double fastestCatch = double.PositiveInfinity;

    public double FastestCatch { get => fastestCatch; }

    [AntigravSerializable("slowest_catch", double.NegativeInfinity)]
    private double slowestCatch = double.NegativeInfinity;

    public double SlowestCatch { get => slowestCatch; }

    [AntigravExtensionData]
    private readonly Dictionary<CtqaType, long> Ctqas = [];

    private ulong GuildId;
    public ulong MemberId;
    public bool DisposeIt = true;

    private static string GetInventoryPath(ulong guildId, ulong memberId) => GetFilePath([Data.InventoriesData, guildId.ToString(), $"{memberId}.antigrav"], "null");
    public long this[CtqaType type] {
        get => Ctqas.TryGetValue(type, out long value) ? value : 0;
        private set {
            Ctqas[type] = value;
            if (value == 0) Ctqas.Remove(type);
        }
    }
    public bool HasAch(AchievementId id) => Achievements.Contains(id);
    public void UpdateCatchTime(double time) {
        fastestCatch = Math.Min(time, fastestCatch);
        slowestCatch = Math.Max(Math.Round(time / 3600, 2), slowestCatch);
    }
    public long IncrementCtqa(CtqaType type) => ++this[type];
    public long DecrementCtqa(CtqaType type) => --this[type];
    public static void RecieveCoupon(ulong guildId, ulong memberId, Coupon coupon) {
        using var inv = Load(guildId, memberId);
        inv[coupon.Type] += coupon.Amount;
    }
    public static void GiftCtqas(Inventory from, Inventory to, CtqaType type, long amount) {
        from[type] -= amount;
        to[type] += amount;
    }
    public static long IncrementCtqaStatic(ulong guildId, ulong memberId, CtqaType type) {
        using var inv = Load(guildId, memberId);
        return inv.IncrementCtqa(type);
    }
    public static long DecrementCtqaStatic(ulong guildId, ulong memberId, CtqaType type) {
        using var inv = Load(guildId, memberId);
        return inv.DecrementCtqa(type);
    }
    public bool HasAmount(CtqaType type, long amount) => this[type] >= amount;
    public static Inventory Load(ulong guildId, ulong memberId) {
        Console.WriteLine($"getting inventory {guildId}-{memberId}");
        var inv = AntigravConvert.LoadFromFile<Inventory>(GetInventoryPath(guildId, memberId)) ?? new Inventory();
        Console.WriteLine($"readed inventory {guildId}-{memberId}");
        inv.GuildId = guildId;
        inv.MemberId = memberId;
        return inv;
    }
    public void Save() => AntigravConvert.DumpToFile(this, GetInventoryPath(GuildId, MemberId), sortKeys: true, indent: 2);
    public void Dispose() {
        if (DisposeIt) Save();
        GC.SuppressFinalize(this);
    }
    public long TotalCtqas { get => Ctqas.Select(x => x.Value).Sum(); }
    public static Embed GetInvEmbed(ulong guildId, IUser member, bool self) {
        var inv = Load(guildId, member.Id);
        CtqaType? custom = UserConfig.GetCustomCtqaStatic(member.Id);
        if (custom != null) inv.IncrementCtqa((CtqaType)custom);
        return new EmbedBuilder() {
            Title = self ? "Your ctqas" : $"{member.FullName()}'s ctqas",
            Description = inv.Ctqas.Count == 0 ? "you have no ctqas go and cry about it <:pointlaugh:1178287922756194394>" : $"{(self ? "You" : "Thei")}r fastest catch is: {(double.IsFinite(inv.FastestCatch) ? (inv.FastestCatch + "s") : "never")}\n{(self ? "You" : "Thei")}r slowest catch is: {(double.IsFinite(inv.SlowestCatch) ? (inv.SlowestCatch + "h") : "never")}",
            Fields = inv.Ctqas.Select(c => new EmbedFieldBuilder() { Name = $"{c.Key.Emoji()} {c.Key.Name()}", Value = c.Value.ToString(), IsInline = true }).ToList(),
            Footer = new() { Text = $"Total ctqas: {inv.TotalCtqas - (custom == null ? 0 : 1)}" }
        }.Build();
    }
    public async Task<Task> GiveAchAsync(IMessageChannel channel, IUser user, AchievementId id) {
        if (channel is SocketDMChannel) {
            await channel.SendMessageAsync("hell naw you cant get achs in dms");
            return Task.CompletedTask;
        }
        if (id == AchievementId.Unknown) {
            await channel.SendMessageAsync(embed: MakeAchEmbed(id, user.FullName()));
            return Task.CompletedTask;
        }
        if (HasAch(id)) return Task.CompletedTask;
        Achievements.Add(id);
        await channel.SendMessageAsync(embed: MakeAchEmbed(id, user.FullName()));
        return Task.CompletedTask;
    }
    public static async Task<Task> GiveAchAsyncStatic(IMessageChannel channel, ulong guildId, IUser user, AchievementId id) {
        if (channel is SocketDMChannel) {
            await channel.SendMessageAsync("hell naw you cant get achs in dms");
            return Task.CompletedTask;
        }
        if (id == AchievementId.Unknown) {
            await channel.SendMessageAsync(embed: MakeAchEmbed(id, user.FullName()));
            return Task.CompletedTask;
        }
        using (var inv = Load(guildId, user.Id)) {
            if (inv.HasAch(id)) {
                inv.DisposeIt = false;
                return Task.CompletedTask;
            }
            inv.Achievements.Add(id);
            await channel.SendMessageAsync(embed: MakeAchEmbed(id, user.FullName()));
        }
        return Task.CompletedTask;
    }
}
