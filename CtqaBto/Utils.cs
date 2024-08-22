using Discord;
using Discord.WebSocket;
using static Antigrav.Main;

namespace CtqaBto;
// random utils here
public static class Utils {
    public static string GetVersion() {
        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
        DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
        return $"build {version} ({buildDate})";
    }
    public static string FormatTime(double time) {
        int days = (int)(time / 86400);
        int hours = (int)(time / 3600) % 24;
        int mins = (int)(time / 60) % 60;
        double secs = time % 60;
        return (days == 0 ? "" : $"{days} days ") +
               (hours == 0 ? "" : $"{hours} hours ") +
               (mins == 0 ? "" : $"{mins} minutes ") +
               (secs == 0 ? "" : $"{Math.Round(secs, 2)} seconds");
    }

    public static int RandRange(int start, int end) {
        lock (Data.random) {
            return Data.random.Next(start, end + 1);
        }
    }

    public static T Choice<T>(IEnumerable<T> enumerable) {
        lock (Data.random) {
            return enumerable.ElementAt(Data.random.Next(enumerable.Count()));
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

    public static string GetImage(string name) => Path.Combine(Data.ImagesPath, name);

    public static List<Tuple<ulong, ulong>> GetCtqasChannels() => LoadFromFile<List<Tuple<ulong, ulong>>>(Data.CtqaChannelsPath) ?? [];

    public static void SetCtqasChannels(List<Tuple<ulong, ulong>> channels) => DumpToFile(channels, Data.CtqaChannelsPath);

    public static Dictionary<ulong, SpawnMessageData> GetCtqasSpawnData() => LoadFromFile<Dictionary<ulong, SpawnMessageData>>(Data.CtqasPath) ?? [];

    public static void SetCtqasSpawnData(Dictionary<ulong, SpawnMessageData> data) => DumpToFile(data, Data.CtqasPath);

    public static string GetURL(this IGuildChannel channel) => $"https://discord.com/channels/{channel.Guild.Id}/{channel.Id}";

    public static ulong GuildId(this SocketMessage message) => ((IGuildChannel)message.Channel).Guild.Id;

    public static string FullName(this IUser user) => user.Username + (user.DiscriminatorValue == 0 ? "" : $"#{user.Discriminator}");

    public static string GetEmoji(string name) {
        GuildEmote? e = Program.client.GetGuild(1178285875608698951).Emotes.FirstOrDefault(e => e.Name == name);
        return e == null ? "emoji fail" : e.ToString();
    }

    public static bool SkillIssued(this IUser user) => !(Data.TrustedPeople.Contains(user.Id) || (user is IGuildUser guildUser && guildUser.GuildPermissions.Administrator));

    public static async Task<IUserMessage> ReplyAsync(this IMessage msg, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None, PollProperties? poll = null) => await msg.Channel.SendMessageAsync(text, isTTS, embed, options, allowedMentions, new MessageReference(msg.Id), components, stickers, embeds, flags, poll);
    
    public static async Task<IUserMessage> ReplyFileAsync(this IMessage msg, string filePath, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, bool isSpoiler = false, AllowedMentions? allowedMentions = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None, PollProperties? poll = null) => await msg.Channel.SendFileAsync(filePath, text, isTTS, embed, options, isSpoiler, allowedMentions, new MessageReference(msg.Id), components, stickers, embeds, flags, poll);

    public readonly struct Button(string? label = null, string? customId = null, ButtonStyle style = ButtonStyle.Primary, string? url = null, IEmote? emote = null, bool isDisabled = false, ulong? skuId = null) {
        public string? CustomId { get; } = customId;
        public ButtonStyle Style { get; } = style;
        public string? Url { get; } = url;
        public string? Label { get; } = label;
        public bool IsDisabled { get; } = isDisabled;
        public IEmote? Emote { get; } = emote;
        public ulong? SkuId { get; } = skuId;
    }

    public static MessageComponent MakeComponents(IEnumerable<Button> buttons) => new ComponentBuilder().AddRow(new ActionRowBuilder() {
        Components = buttons.Select(b => new ButtonBuilder() {
            CustomId = b.CustomId,
            Emote = b.Emote,
            IsDisabled = b.IsDisabled,
            Label = b.Label,
            SkuId = b.SkuId,
            Style = b.Style,
            Url = b.Url
        }.Build()).Cast<IMessageComponent>().ToList()
    }).Build();
}
