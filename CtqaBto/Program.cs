using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using static CtqaBto.Utils;
using static CtqaBto.Ctqas;
using кансоль = System.Console;

namespace CtqaBto;

internal class Program {
    readonly static string token = File.ReadAllText("TOKEN.txt");
    public readonly static DiscordSocketClient client = new(new DiscordSocketConfig() {
        GatewayIntents = GatewayIntents.All,
        UseInteractionSnowflakeDate = false
    });
    readonly InteractionService interactionService = new(client.Rest);

    static Task Main() => new Program().MainAsync();
    // bot setup is done here
    async Task MainAsync() {
        await interactionService.AddModuleAsync<CommandModule>(null);
        client.Log += Log;
        client.Ready += Ready;
        client.SlashCommandExecuted += SlashCommandExecuted;
        client.SelectMenuExecuted += Client_SelectMenuExecuted;
        client.MessageReceived += Client_MessageReceived;
        client.ButtonExecuted += InteractionExecuted;
        interactionService.Log += Log;
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        await Task.Delay(-1);
    }

    private async Task<Task> Client_SelectMenuExecuted(SocketMessageComponent cmd) {
        await interactionService.ExecuteCommandAsync(new InteractionContext(client, cmd, cmd.Channel), null);
        return Task.CompletedTask;
    }

    private async Task<Task> SlashCommandExecuted(SocketSlashCommand cmd) {
        await interactionService.ExecuteCommandAsync(new InteractionContext(client, cmd, cmd.Channel), null);
        return Task.CompletedTask;
    }

    private async Task<Task> InteractionExecuted(SocketMessageComponent component) {
        string h = component.Data.CustomId;
        return Task.CompletedTask;
    }

    private async Task<Task> Ready() {
        кансоль.WriteLine($"@{client.CurrentUser} is now ready");
        await interactionService.RegisterCommandsGloballyAsync();
        Data.StartTime = DateTime.Now;
        await SpawnCtqaLoopAsync();
        return Task.CompletedTask;
    }

    private async Task<Task> Client_MessageReceived(SocketMessage message) {
        return Task.CompletedTask;
    }

    private Task Log(LogMessage msg) {
        кансоль.WriteLine(msg);
        return Task.CompletedTask;
    }
}

internal static class Data {
    public static Random random = new();
    public static DateTime StartTime = DateTime.MinValue;
    public static readonly string CtqaChannelsPath = GetFilePath(["ctqa channels.antigrav"], "[]");
    public static readonly string CtqasPath = GetFilePath(["ctqas.antigrav"], "{}");
    public static async Task<RestMessage> ReplyAsync(this IMessage msg, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None) {
        return (RestMessage)await msg.Channel.SendMessageAsync(text, isTTS, embed, options, allowedMentions, new MessageReference(msg.Id), components, stickers, embeds, flags);
    }
    public static readonly ulong[] TrustedPeople = [
        558979299177136164,   // tema5002
        1204799892988629054,  // cake64
        1127903408179904662,  // firewall6
        801078409076670494    // hexahedron1
    ];
}
internal class CommandModule : InteractionModuleBase {
    public required InteractionService Service { get; set; }

    [SlashCommand("ping", "get ping")]
    public async Task Ping() {
        int h = ((DiscordSocketClient)Context.Client).Latency;
        string message = $"ctqa has bran delayt of {h}ms <:syating_ctqa:1178288745435385896>";
        if      (h >= 10000) message = $"ctqa  iahbr an d<:syating_ctqa:1178288745435385896>lsy o{h} fmsae";
        else if (h >= 8000)  message = $"ci a<:syating_ctqa:1178288745435385896>has bratqmdelay o n{h}  sf";
        else if (h >= 5000)  message = $"cn a <:syating_ctqa:1178288745435385896>at br isadel yaofq{h}ms h";
        else if (h >= 3000)  message = $"htqa cysrb fan delai oa {h}ms <:syating_ctqa:1178288745435385896>";
        else if (h >= 2000)  message = $"cdqa{h}hastbrain  e  ylof  msa<:syating_ctqa:1178288745435385896>";
        else if (h >= 1500)  message = $"ct a<:syating_ctqa:1178288745435385896>ahsqbrain delay of {h} s m";
        else if (h >= 1000)  message = $"ctsbohas arain delay  f {h}mq <:syating_ctqa:1178288745435385896>";
        else if (h >= 500)   message = $"ctqa<:syating_ctqa:1178288745435385896>has brain delay of {h}ms";
        else if (h >= 300)   message = $"ctqa hasnbrai  delay of {h}ms <:syating_ctqa:1178288745435385896>";
        await RespondAsync(message);
    }

    [SlashCommand("setup", "make bot spawn ctqas here (ADMIN ONLY)")]
    public async Task Setup() {
        if (Context.User.SkillIssued()) {
            await RespondAsync("lmao perms fail imagine having a skill issue <:pointlaugh:1178287922756194394>");
            return;
        }
        if (Context.Guild == null || Context.Channel == null) {
            await RespondAsync("nuh uh");
            return;
        }
        List<Tuple<ulong, ulong>> channels = GetCtqasChannels();
        Tuple<ulong, ulong> tuple = new(Context.Guild.Id, Context.Channel.Id);
        if (channels.Remove(tuple)) {
            await RespondAsync($"**#{Context.Channel}** was removed from ctqa spawn list ❌");
            SetCtqasChannels(channels);
            return;
        }
        foreach (var guildChannel in channels.Where(x => x.Item1 == Context.Guild.Id)) {
            SocketGuild guild = ((DiscordSocketClient)Context.Client).GetGuild(guildChannel.Item1);
            if (guild == null) {
                channels.Remove(guildChannel);
                SetCtqasChannels(channels);
                return;
            }
            SocketGuildChannel? channel = guild.GetChannel(guildChannel.Item2);
            if (channel == null) {
                await RespondAsync("there is ctqa spawn loop linked to some non existent channel (removed it, run /setup again)");
                channels.Remove(guildChannel);
                SetCtqasChannels(channels);
                return;
            }
            await RespondAsync($"there is already a ctqa spawn loop in {channel.GetURL()}");
            return;
        }
        channels.Add(tuple);
        await RespondAsync($"**#{Context.Channel}** was added to ctqa spawn list ✅");
        SetCtqasChannels(channels);
    }
}