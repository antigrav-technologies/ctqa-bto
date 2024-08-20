using static CtqaBto.Ctqas;
using static CtqaBto.Utils;
using static Antigrav.Main;

namespace CtqaBto;

public struct SpawnMessageData(CtqaType type = CtqaType.Unknown, ulong messageId = 0, string sayToCatch = "ctqa") {
    [AntigravProperty("type")]
    public CtqaType Type = type;
    [AntigravProperty("message_id")]
    public ulong MessageId { get; set; } = messageId;
    [AntigravProperty("say_to_catch")]
    public string SayToCatch { get; set; } = sayToCatch;
}

public class Inventory : IDisposable {
    [AntigravProperty("achs")]
    public List<string> Achievements { get; private set; } = [];

    [AntigravProperty("fastest_catch")]
    public float? FastestCatch { get; private set; } = null;

    [AntigravProperty("slowest_catch")]
    public float? SlowestCatch { get; private set; } = null;

    [AntigravExtensionData]
    private Dictionary<CtqaType, long> Ctqas { get; set; } = [];
    private ulong GuildId;
    private ulong MemberId;
    private Inventory() {}

    private static string GetInventoryPath(ulong guildId, ulong memberId) => GetFilePath(["ctqas", guildId.ToString(), $"{memberId}.antigrav"], "{}");

    private long this[CtqaType type] {
        get => Ctqas.TryGetValue(type, out long value) ? value : 0;
        set => Ctqas[type] = value;
    }

    public static void IncrementCtqa(ulong guildId, ulong memberId, CtqaType type) {
        using var inv = Load(guildId, memberId);
        inv[type]++;
    }

    public static void DecrementCtqa(ulong guildId, ulong memberId, CtqaType type) {
        using var inv = Load(guildId, memberId);
        inv[type]--;
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
}
