﻿using Discord;
using Discord.WebSocket;
using static CtqaBto.Utils;

namespace CtqaBto;

public static class Ctqas {
    public enum CtqaType {
        Unknown = -1,
        Fine = 1,
        Nice = 2,
        Good = 3,
        Uncommon = 4,
        Rare = 5,
        Wild = 6,
        Baby = 7,
        Old = 8,
        Epic = 9,
        Brave = 10,
        Reverse = 11,
        Inverted = 12,
        Superior = 13,
        Tema5002 = 14,
        Legendary = 15,
        Mythic = 16,
        EightBit = 17,
        Corrupted = 18,
        Professor = 19,
        Real = 20,
        Ultimate = 21,
        Cool = 22,
        Silly = 1000,
        Icosahedron = 10001,
        Aflyde = 10002,
        Octopus = 10003,
        typing = 10004,
        Kesslon = 10005,
        Bread = 10006,
        Blep = 10007,
        cake64 = 10008,
        antaegeav = 10009,
        Jeremy = 10010,
        Maxwell = 10011,
        Pentachoron = 10012,
        NetscapeAd = 10013
    }
    static readonly Dictionary<CtqaType, int> typeDict = new() {
        { CtqaType.Fine, 1000 },
        { CtqaType.Nice, 750 },
        { CtqaType.Good, 650 },
        { CtqaType.Uncommon, 500 },
        { CtqaType.Rare, 350 },
        { CtqaType.Wild, 275 },
        { CtqaType.Baby, 230 },
        { CtqaType.Old, 200 },
        { CtqaType.Epic, 175 },
        { CtqaType.Brave, 150 },
        { CtqaType.Reverse, 125 },
        { CtqaType.Inverted, 100 },
        { CtqaType.Superior, 80 },
        { CtqaType.Tema5002, 50 },
        { CtqaType.Legendary, 35 },
        { CtqaType.Mythic, 25 },
        { CtqaType.EightBit, 20 },
        { CtqaType.Corrupted, 15 },
        { CtqaType.Professor, 10 },
        { CtqaType.Real, 8 },
        { CtqaType.Ultimate, 5 },
        { CtqaType.Cool, 2 }
    };
    private static readonly CtqaType[] CustomTypes = [
        CtqaType.Silly,
        CtqaType.Icosahedron,
        CtqaType.Aflyde,
        CtqaType.Octopus,
        CtqaType.typing,
        CtqaType.Kesslon,
        CtqaType.Bread,
        CtqaType.Blep,
        CtqaType.cake64,
        CtqaType.antaegeav,
        CtqaType.Jeremy,
        CtqaType.Maxwell,
        CtqaType.Pentachoron,
        CtqaType.NetscapeAd
    ];
    public static readonly CtqaType[] CtqaTypes = [.. typeDict.Keys];
    public static CtqaType RandomCtqaType() {
        int totalWeight = typeDict.Values.Sum();
        int randomValue = new Random().Next(totalWeight);
        foreach (var kvp in typeDict) {
            randomValue -= kvp.Value;
            if (randomValue < 0) {
                return kvp.Key;
            }
        }
        return CtqaType.Unknown;
    }
    public static string Name(this CtqaType type) => type switch {
        CtqaType.EightBit => "8bit",
        CtqaType.NetscapeAd => "Netscape Ad",
        _ => Enum.GetName(typeof(CtqaType), type) ?? "Unknown"
    };
    public static string Emoji(this CtqaType type) => GetEmoji(type.Name().ToLower() + "ctqa");
    public static async Task<bool> SpawnCtqaAsync(IMessageChannel channel) {
        var data = GetCtqasSpawnData();
        if (!data.ContainsKey(channel.Id)) {
            CtqaType type = RandomCtqaType();
            string emoji = type.Emoji();
            var message = await channel.SendFileAsync(GetCtqaImage(type), $"NO FUCJING WAY A {type.Name()} CTQA APPEARED HERE IT IS TYPE \"ctqa\" AND YOU WILL CATCH IT ↪️↪️↪️➡️➡️➡️➡️↪️➡️ {emoji}‼️‼️");
            data[channel.Id] = new(type, message.Id);
            SetCtqasSpawnData(data);
            return true;
        }
        return false;
    }
    public static async Task<Task> SpawnCtqaLoopAsync() {
        List<Tuple<ulong, ulong>> channels = GetCtqasChannels();
        foreach (Tuple<ulong, ulong> tuple in channels) {
            SocketGuild? guild = Program.client.GetGuild(tuple.Item1);
            if (guild != null) {
                SocketGuildChannel? channel = guild.GetChannel(tuple.Item2);
                if (channel != null) {
                    await ((IMessageChannel)channel).SendMessageAsync(Choice(Data.StartText));
                }
                else channels.Remove(tuple);
            }
            else channels.Remove(tuple);
        }
        SetCtqasChannels(channels);
        while (true) {
            channels = GetCtqasChannels();
            foreach (Tuple<ulong, ulong> tuple in channels) {
                SocketGuild? guild = Program.client.GetGuild(tuple.Item1);
                if (guild != null) {
                    SocketGuildChannel? channel = guild.GetChannel(tuple.Item2);
                    if (channel != null) {
                        await SpawnCtqaAsync((IMessageChannel)channel);
                    }
                    else channels.Remove(tuple);
                }
                else channels.Remove(tuple);
            }
            SetCtqasChannels(channels);
            await Task.Delay(RandRange(2 * 1000 * 60, 20 * 1000 * 60));
        }
    }
    public static string GetCtqaImage(CtqaType type) => GetImage(type switch {
        CtqaType.Inverted => "inverted_ctqa.webp",
        CtqaType.Reverse => "reverse_ctqa.webp",
        CtqaType.Professor => "syating_professor_ctqa.webp",
        CtqaType.Baby => "baby_ctqa.webp",
        _ => "syating_ctqa.webp"
    });
}
