using Discord;
using static CtqaBto.Utils;

namespace CtqaBto;

public static class Leaderboards {
    public struct ServerLeaderboardCache {
        public readonly ulong GuildId { get; }
        public Tuple<ulong, long>[] Ctqas { get; private set; } = [];
        public Tuple<ulong, double>[] Fastest { get; private set; } = [];
        public Tuple<ulong, double>[] Slowest { get; private set; } = [];
        public DateTime TimeSinceLastUpdate = DateTime.MinValue;

        public ServerLeaderboardCache(ulong guildId) {
            GuildId = guildId;
            Update();
        }

        public void Update() {
            ulong guildId = GuildId; // linq was being a bushes so i put this
            if (Math.Abs((DateTime.Now - TimeSinceLastUpdate).TotalMinutes) > 10) {
                TimeSinceLastUpdate = DateTime.Now;
                var data = Directory.EnumerateFiles(GetFolderPath(["ctqas", guildId.ToString()])).Select(path => Inventory.Load(guildId, ulong.Parse(Path.GetFileNameWithoutExtension(path)))).Where(inv => double.IsFinite(inv.FastestCatch) && double.IsFinite(inv.SlowestCatch)).Select(inv => new { Id = inv.MemberId, Total = inv.TotalCtqas, Fastest = inv.FastestCatch, Slowest = inv.SlowestCatch }).ToList();
                data.Sort((x, y) => y.Total.CompareTo(x.Total));
                Ctqas = data.Take(15).Select(x => new Tuple<ulong, long>(x.Id, x.Total)).ToArray();
                data.Sort((x, y) => x.Fastest.CompareTo(y.Fastest));
                Fastest = data.Take(15).Select(x => new Tuple<ulong, double>(x.Id, x.Fastest)).ToArray();
                data.Sort((x, y) => y.Slowest.CompareTo(x.Slowest));
                Slowest = data.Take(15).Select(x => new Tuple<ulong, double>(x.Id, x.Slowest)).ToArray();
            }
        }
    }
    private static readonly List<ServerLeaderboardCache> cache = [];
    public static ServerLeaderboardCache GetCache(ulong guildId) {
        for (int i = 0; i < cache.Count; i++) {
            if (cache[i].GuildId == guildId) {
                cache[i].Update();
                return cache[i];
            }
        }
        cache.Add(new ServerLeaderboardCache(guildId));
        return cache.Last();
    }

    public enum LeaderboardsType {
        Ctqas,
        Fastest,
        Slowest
    }
    public static Embed GetLeaderboardsEmbed(IGuild guild, LeaderboardsType type) => type switch {
        LeaderboardsType.Ctqas => new EmbedBuilder() {Title = $"{guild.Name} leaderboards:", Description = string.Join('\n', GetCache(guild.Id).Ctqas.Select((tuple, index) => $"{index}. {tuple.Item2} ctqa{(Math.Abs(tuple.Item2) > 1 ? "s" : "")}: <@{tuple.Item1}>"))}.Build(),
        LeaderboardsType.Fastest => new EmbedBuilder() {Title = $"{guild.Name} leaderboards:", Description = string.Join('\n', GetCache(guild.Id).Fastest.Select((tuple, index) => $"{index}. {tuple.Item2}s: <@{tuple.Item1}>"))}.Build(),
        LeaderboardsType.Slowest => new EmbedBuilder() {Title = $"{guild.Name} leaderboards:", Description = string.Join('\n', GetCache(guild.Id).Slowest.Select((tuple, index) => $"{index}. {tuple.Item2}h: <@{tuple.Item1}>"))}.Build(),
        _ => new EmbedBuilder() { Title = "ты что наделал уебан бля" }.Build()
    };
    public static MessageComponent GetLeaderboardsComponents(LeaderboardsType type) => MakeComponents(Enum.GetValues<LeaderboardsType>().Select(e => new Button(type == e ? "Refresh" : e.ToString(), $"UPDATELB;{(int)e}", type == e ? ButtonStyle.Success : ButtonStyle.Primary)));
}
