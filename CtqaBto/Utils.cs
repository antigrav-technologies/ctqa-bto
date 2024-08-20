using Discord;
using static Antigrav.Main;

namespace CtqaBto;
// random utils here
public static class Utils {
    public static int RandRange(int start, int end) {
        lock (Data.random) {
            return Data.random.Next(start, end + 1);
        }
    }
    public static string GetFolderPath(IEnumerable<string> args) {
        string folder = "";

        foreach (string f in args) {
            folder = Path.Combine(folder, f);
            if (!Directory.Exists(folder)) {
                try {
                    Directory.CreateDirectory(folder);
                }
                catch (IOException ex) {
                    throw new IOException($"Failed to create folders: {ex.Message}", ex);
                }
            }
        }

        return folder;
    }

    public static string GetFilePath(IList<string> args, string? createFile = null) {
        string path = Path.Combine(
            GetFolderPath(args.Take(args.Count - 1)),
            args.Last()
        );

        if (createFile != null && !Path.Exists(path)) {
            File.WriteAllText(path, createFile);
        }

        return path;
    }

    public static List<Tuple<ulong, ulong>> GetCtqasChannels() => LoadFromFile<List<Tuple<ulong, ulong>>>(Data.CtqaChannelsPath) ?? [];

    public static void SetCtqasChannels(List<Tuple<ulong, ulong>> channels) => DumpToFile(channels, Data.CtqaChannelsPath);

    public static Dictionary<ulong, SpawnMessageData> GetCtqasSpawnData() => LoadFromFile<Dictionary<ulong, SpawnMessageData>>(Data.CtqasPath) ?? [];

    public static void SaveCtqasSpawnData(Dictionary<ulong, SpawnMessageData> data) => DumpToFile(data, Data.CtqasPath);

    public static string GetURL(this IGuildChannel channel) => $"https://discord.com/channels/{channel.Guild.Id}/{channel.Id}";

    public static bool SkillIssued(this IUser user) => !(Data.TrustedPeople.Contains(user.Id) || (user is IGuildUser guildUser && guildUser.GuildPermissions.Administrator));
}
