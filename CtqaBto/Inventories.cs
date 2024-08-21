﻿using static CtqaBto.Ctqas;
using static CtqaBto.Utils;
using static CtqaBto.Achievements;
using static Antigrav.Main;
using Discord;
using Discord.WebSocket;

namespace CtqaBto;

public struct SpawnMessageData(CtqaType type = CtqaType.Unknown, ulong messageId = 0, string sayToCatch = "ctqa") {
    [AntigravProperty("type")]
    public CtqaType Type { get; private set; } = type;
    [AntigravProperty("message_id")]
    public ulong MessageId { get; private set; } = messageId;
    [AntigravProperty("say_to_catch")]
    public string SayToCatch { get; private set; } = sayToCatch;
}

public class Inventory : IDisposable {
    [AntigravProperty("achs")]
    public List<AchievementId> Achievements { get; private set; } = [];

    [AntigravProperty("fastest_catch", float.PositiveInfinity)]
    public double FastestCatch { get; private set; } = float.PositiveInfinity;

    [AntigravProperty("slowest_catch", float.NegativeInfinity)]
    public double SlowestCatch { get; private set; } = float.NegativeInfinity;

    [AntigravExtensionData]
    private Dictionary<CtqaType, long> Ctqas { get; set; } = [];
    private ulong GuildId;
    private ulong MemberId;

    private static string GetInventoryPath(ulong guildId, ulong memberId) => GetFilePath(["ctqas", guildId.ToString(), $"{memberId}.antigrav"], "{}");
    private long this[CtqaType type] {
        get => Ctqas.TryGetValue(type, out long value) ? value : 0;
        set => Ctqas[type] = value;
    }
    public long GiveCtqa(CtqaType type) => ++this[type];
    public long RemoveCtqa(CtqaType type) => --this[type];
    public bool HasAch(AchievementId id) => Achievements.Contains(id);
    public void UpdateCatchTime(double time) {
        FastestCatch = Math.Min(time, FastestCatch);
        SlowestCatch = Math.Max(Math.Round(time / 3600, 2), SlowestCatch);
    }
    public static long IncrementCtqa(ulong guildId, ulong memberId, CtqaType type) {
        long v;
        using (var inv = Load(guildId, memberId)) {
            inv[type]++;
            v = inv[type];
        }
        return v;
    }
    public static long DecrementCtqa(ulong guildId, ulong memberId, CtqaType type) {
        long v;
        using (var inv = Load(guildId, memberId)) {
            inv[type]--;
            v = inv[type];
        }
        return v;
    }
    public static Inventory Load(ulong guildId, ulong memberId) {
        var inv = LoadFromFile<Inventory>(GetInventoryPath(guildId, memberId)) ?? new Inventory();
        inv.GuildId = guildId;
        inv.MemberId = memberId;
        return inv;
    }
    public void Save() => DumpToFile(this, GetInventoryPath(GuildId, MemberId));
    public void Dispose() {
        Save();
        GC.SuppressFinalize(this);
    }
    public static Embed GetEmbed(ulong guildId, IUser member, bool self) {
        var inv = Load(guildId, member.Id);
        return new EmbedBuilder() {
            Title = self ? "Your ctqas" : $"{member.FullName()}'s ctqas",
            Description = inv.Ctqas.Count == 0 ? "you have no ctqas go and cry about it <:pointlaugh:1178287922756194394>" : $"{(self ? "You" : "The")}r fastest catch is: {(double.IsFinite(inv.FastestCatch) ? (inv.FastestCatch + "s") : "never")}\n{(self ? "You" : "The")}r slowest catch is: {(double.IsFinite(inv.SlowestCatch) ? (inv.SlowestCatch + "h") : "never")}",
            Fields = inv.Ctqas.Select(c => new EmbedFieldBuilder() { Name = $"{c.Key.Emoji()} {c.Key.Name()}", Value = c.Value.ToString(), IsInline = true }).ToList(),
            Footer = new() { Text = $"Total ctqas: {inv.Ctqas.Select(x => x.Value).Sum()}" }
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
            if (inv.HasAch(id)) return Task.CompletedTask;
            inv.Achievements.Add(id);
            await channel.SendMessageAsync(embed: MakeAchEmbed(id, user.FullName()));
        }
        return Task.CompletedTask;
    }
}
