using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using static CtqaBto.Utils;
using static CtqaBto.Ctqas;
using static CtqaBto.Achievements;
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
        try {
            if (message.Channel is SocketDMChannel) {
                await message.Channel.SendMessageAsync("good job! use ctqa!lol_i_have_dmed_ctqa_and_got_an_ach");
                return Task.CompletedTask;
            }
            var ctqasSpawnData = GetCtqasSpawnData();
            SpawnMessageData? spawnMessageData = null;
            string sayToCatch = "ctqa";
            if (ctqasSpawnData.TryGetValue(message.Channel.Id, out var value)) spawnMessageData = value;
            if (spawnMessageData != null) sayToCatch = ((SpawnMessageData)spawnMessageData).SayToCatch;
            string msg = message.Content;
            string msgl = msg.ToLower();
            if (message.MentionedUsers.Any(x => x.Id == client.CurrentUser.Id)) {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.PingBot);
            }
            if (msg == "полимерная глина в шкиле 🦈 и тысяча рублей за 48 сообщений в теме на солнце ☀️ бесплатно и смерть 💀 и не только у 😎") {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Unknown);
            }
            if (sayToCatch == msg.ToUpper()) {
                if (sayToCatch == "ctqa") {
                    await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.CTQA);
                }
                else {
                    await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.NOTCTQA);
                }
            }

            if (spawnMessageData != null && msgl == "cat") {
                await message.AddReactionAsync(Emote.Parse("<:pointlaugh:1178287922756194394>"));
            }
            if (msg.Equals(sayToCatch, StringComparison.CurrentCultureIgnoreCase)) {
                if (spawnMessageData != null) {
                    ctqasSpawnData.Remove(message.Channel.Id);
                    SetCtqasSpawnData(ctqasSpawnData);
                    IMessage? ctqaMessage = await message.Channel.GetMessageAsync(((SpawnMessageData)spawnMessageData).MessageId);
                    double time = Math.Abs(Math.Round((message.CreatedAt - ctqaMessage.CreatedAt).TotalSeconds, 2));
                    CtqaType type = ((SpawnMessageData)spawnMessageData).Type;

                    long amount;
                    using (var inv = Inventory.Load(message.GuildId(), message.Author.Id)) {
                        await inv.GiveAchAsync(message.Channel, message.Author, AchievementId.FirstCtqa);
                        inv.UpdateCatchTime(time);
                        if (inv.FastestCatch < 5) {
                            await inv.GiveAchAsync(message.Channel, message.Author, AchievementId.FastCatcher);
                        }
                        if (inv.SlowestCatch > 1) {
                            await inv.GiveAchAsync(message.Channel, message.Author, AchievementId.SlowCatcher);
                        }
                        amount = inv.GiveCtqa(type);
                    }
                    try {
                        await ctqaMessage.DeleteAsync();
                        await message.DeleteAsync();
                    }
                    catch (Exception ex) {
                        await ((IMessageChannel)message).SendMessageAsync($"failed to delete ctqa spawn or \"ctqa\" message\n```\n{ex.Message}\n```");
                    }
                    string emoji = type.Emoji();
                    await message.Channel.SendMessageAsync(@$"{message.Author} COUGHT... {type.Name()} CTQA!!!!1! {emoji}{emoji}{emoji} (100% REAL NOT CLICKBAIT)
bro now has {amount} ctqas of dat type ‼️😱🔥
OMG OMG IT WAS COUGHT IN {FormatTime(time)} ??? 1 ? 1 ? 1!1! ⁉️⁉️⁉️ HOW! ? 1 ? 1 ? 1!1!1 ? 1");
                }
                else {
                    await message.AddReactionAsync(Emote.Parse("<:pointlaugh:1178287922756194394>"));
                }
            }

            if (msg == Data.Datamine) {
                await message.DeleteAsync();
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Datamine);
            }

            if (msgl == "сейф") {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Сейф);
            }

            if (msgl == "please do the ctqa") {
                await message.ReplyFileAsync("socialcredit.png");
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.PleaseDoTheCtqa);
            }

            if (msgl == "please do not the ctqa") {
                await message.ReplyAsync($"ok then\n{message.Author.Mention} lost one fine ctqa!!!!11");
                using var inv = Inventory.Load(message.GuildId(), message.Author.Id);
                inv.RemoveCtqa(CtqaType.Fine);
                await inv.GiveAchAsync(message.Channel, message.Author, AchievementId.PleaseDoNotTheCtqa);
            }
                
            if (msgl.Contains(":syating_ctqa:") && msgl.Contains("🛐")) {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Worship);
            }
            
            if ("ctqa!lol_i_have_dmed_ctqa_and_got_an_ach" == msg) {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.DMBot);
            }
        }
        catch (Exception ex) {
            await message.ReplyAsync($"гавно в шкиле\n```\n{ex}\n```");
        }
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
    public static readonly string ImagesPath = "D:\\CtqaBto\\src";
    public static readonly ulong[] TrustedPeople = [
        558979299177136164,   // tema5002
        1204799892988629054,  // cake64
        1127903408179904662,  // firewall6
        801078409076670494    // hexahedron1
    ];
    public static readonly string[] StartText = [
        "hello fellow kids i have started",
        "wake up its RUIN CTQA SOURCE CODE 🔥🔥🔥🔥 o'clock",
        "```\nAntigrav.Decoder.ANTIGRAVDecodeError\n  HResult=0x80131500\n  Сообщение = Expecting value: line 1 column 1 (char 0)\n  Источник = Antigrav\n  Трассировка стека:\n   в Antigrav.Decoder.Decode[T](String s) в C:\\Users\\User\\source\\repos\\Antigrav\\Antigrav\\Decoder.cs:строка 508\n   в Antigrav.Main.LoadFromString[T](String s) в C:\\Users\\User\\source\\repos\\Antigrav\\Antigrav\\Main.cs:строка 156\\   в CtqaBto.Program.Main() в D:\\CtqaBto\\CtqaBto\\Program.cs:строка 20\n   в CtqaBto.Program.<Main>()\n```",
        "also try kat bot",
        "gaming",
        $"ctqa bto stats:\n{Program.client.Guilds.Count} servers\n{Program.client.Guilds.Select(x => x.MemberCount).Sum()} total members",
        "gaming",
        "Also try NBTExplorer!",
        "int qwertyuiop[]",
        "hello i am mister balls",
        "start Ctqa bto Now or something else 🐎🤯🤯😉😉🎓😊😊",
        "Who the Hell Started my Ctqa bto ‼️",
        "kreisi purglar paking peanuts",
        "HELLO THERE EVERYONE my name is INSANE and i am the CEO of your MOTHER",
        "insert some epic motivational uplifting text here",
        "cool but unfortunately nobody asked",
        "simon says say h",
        "cellua",
        "#minecraftphysics",
        "download tema app for unlimited slinx attic invite",
        "CtqaLink",
        "29A:AA79//@A>@4-->.4>"
    ];
    public static readonly string Datamine = "ctqa!ΔπβΔ©🐙αλ1Σhh1π1π©🐙Σ1π©βπΔΔ1βππhαββπλβππ🐙ΔhhαΔΔΣ1π🐙βλhαπβ©βββ1πΣβ🐙πΔβΣΔ🐙©αλαh🐙hΣβπh©ΣΔΔ🐙πλΣλλ11λhα🐙Δh©β©©πΔ©ΣβhΔλ🐙πΔβΔΔ🐙©ΣβββλαΔΣπ";
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
    public async Task SetupSlashCommand() {
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

    [SlashCommand("inv", "view your inventory")]
    public async Task InventorySlashCommand(IUser? member = null) => await RespondAsync(embed: Inventory.GetEmbed(
        Context.Guild.Id,
        member ?? Context.User,
        member == null
    ));

    [SlashCommand("info", "get info about bot")]
    public async Task InfoSlashCommand() => await RespondAsync(embed: new EmbedBuilder() {
        Title = "ctqa bto",
        Description = $@"[support server](https://discord.gg/QnXad4qY4U) | [source code](https://github.com/tema5002/ctqa-bto)

i dont really know what to say here
run /setup to make ctqas spawn in channel
if they randomly stopped spawning try running /setup again

thanks to:
- **{Program.client.GetUser(986132157967761408).FullName()}** for syating ctqa image and making ctqa icons"
    }.Build());
}