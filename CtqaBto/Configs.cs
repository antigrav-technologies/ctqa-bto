using Discord;
using Antigrav;
using static CtqaBto.Ctqas;
using static CtqaBto.Utils;

namespace CtqaBto;

public static class Configs {
    public struct Coupon(string name, CtqaType type, int amount) {
        [AntigravSerializable("name")]
        public string Name { get; private set; } = name;
        [AntigravSerializable("type")]
        public CtqaType Type { get; private set; } = type;
        [AntigravSerializable("amount")]
        public int Amount { get; private set; } = amount;
    }
    public class ServerConfig : IDisposable {
        [AntigravSerializable("whitelist")]
        private List<ulong> Whitelist { get; set; } = [];

        [AntigravSerializable("blacklist")]
        private List<ulong> Blacklist { get; set; } = [];

        [AntigravSerializable("coupons")]
        private List<Coupon> Coupons { get; set; } = [];

        [AntigravSerializable("teapot_blacklist")]
        private readonly List<ulong> NotAllowedToUseTeapot = [];

        private ulong GuildId;
        public bool DisposeIt { private get; set; } = true;

        public static bool TryUseCouponStatic(ulong id, string name, out Coupon? coupon) {
            using var config = Load(id);
            coupon = null;
            if (config.Coupons.All(x => x.Name != name)) {
                config.DisposeIt = false;
                return false;
            }
            coupon = config.Coupons.First(x => x.Name == name);
            config.Coupons.Remove((Coupon)coupon);
            return true;
        }

        public bool IsNotAllowToUseTeapot(ulong id) => NotAllowedToUseTeapot.Contains(id);

        public static bool IsNotAllowToUseTeapotStatic(ulong guildId, ulong id) => Load(guildId).NotAllowedToUseTeapot.Contains(id);

        public bool IsWhitelisted(IUser user) => (!user.IsBot || Whitelist.Contains(user.Id)) && !Blacklist.Contains(user.Id);

        public static bool IsWhitelistedStatic(ulong guildId, IUser user) => Load(guildId).IsWhitelisted(user);

        public bool UpdateWhitelist(ulong id) {
            if (Whitelist.Remove(id)) return false;
            Whitelist.Add(id);
            return true;
        }

        public static bool UpdateWhitelistStatic(ulong guildId, ulong id) {
            using var config = Load(guildId);
            return config.UpdateWhitelist(id);
        }

        public bool UpdateBlacklist(ulong id) {
            if (Blacklist.Remove(id)) return false;
            Blacklist.Add(id);
            return true;
        }

        public static bool UpdateBlacklistStatic(ulong guildId, ulong id) {
            using var config = Load(guildId);
            return config.UpdateBlacklist(id);
        }

        private static string GetServerConfigPath(ulong guildId) => GetFilePath(["server configs", $"{guildId}.antigrav"], "null");

        public static ServerConfig Load(ulong guildId) {
            var config = AntigravConvert.LoadFromFile<ServerConfig>(GetServerConfigPath(guildId)) ?? new ServerConfig();
            config.GuildId = guildId;
            return config;
        }

        public void Save() => AntigravConvert.DumpToFile(this, GetServerConfigPath(GuildId));

        public void Dispose() {
            if (DisposeIt) Save();
            GC.SuppressFinalize(this);
        }
    }
    public class UserConfig : IDisposable {
        [AntigravSerializable("custom")]
        private CtqaType? customCtqa = null;
        public CtqaType? CustomCtqa { get => customCtqa; set => customCtqa = customCtqa != null && IsCustom((CtqaType)customCtqa) ? null : value; }

        private ulong UserId;

        private static string GetUserConfigPath(ulong userId) => GetFilePath(["user configs", $"{userId}.antigrav"], "null");

        public static UserConfig Load(ulong userId) {
            var config = AntigravConvert.LoadFromFile<UserConfig>(GetUserConfigPath(userId)) ?? new UserConfig();
            config.UserId = userId;
            return config;
        }

        public static CtqaType? GetCustomCtqaStatic(ulong userId) => Load(userId).CustomCtqa;

        public static void SetCustomCtqaStatic(ulong userId, CtqaType? type) {
            using var config = Load(userId);
            config.CustomCtqa = type;
        }

        public void Save() => AntigravConvert.DumpToFile(this, GetUserConfigPath(UserId));

        public void Dispose() {
            Save();
            GC.SuppressFinalize(this);
        }
    }
}
