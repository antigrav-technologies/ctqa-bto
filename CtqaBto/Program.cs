using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using static CtqaBto.Utils;
using static CtqaBto.Ctqas;
using static CtqaBto.Achievements;
using static CtqaBto.Teapot;
using static CtqaBto.Leaderboards;
using static CtqaBto.Configs;
using кансоль = System.Console;
[assembly: AssemblyVersion("1.0.*")]

namespace CtqaBto;

internal class Program {
    private static readonly string Token = File.ReadAllText("TOKEN.txt");
    public static readonly DiscordSocketClient Client = new(new DiscordSocketConfig() {
        GatewayIntents = GatewayIntents.All,
        UseInteractionSnowflakeDate = false
    });
    private readonly InteractionService _interactionService = new(Client.Rest);

    public static Task Main() => new Program().MainAsync();
    // bot setup is done here
    private async Task MainAsync() {
        await _interactionService.AddModuleAsync<CommandModule>(null);
        Client.Log += Log;
        Client.Ready += Ready;
        Client.SlashCommandExecuted += SlashCommandExecuted;
        Client.SelectMenuExecuted += Client_SelectMenuExecuted;
        Client.MessageReceived += Client_MessageReceived;
        Client.ButtonExecuted += InteractionExecuted;
        _interactionService.Log += Log;
        await Client.LoginAsync(TokenType.Bot, Token);
        await Client.StartAsync();
        await Task.Delay(-1);
    }

    private async Task<Task> Client_SelectMenuExecuted(SocketMessageComponent cmd) {
        await _interactionService.ExecuteCommandAsync(new InteractionContext(Client, cmd, cmd.Channel), null);
        return Task.CompletedTask;
    }

    private async Task<Task> SlashCommandExecuted(SocketSlashCommand cmd) {
        await _interactionService.ExecuteCommandAsync(new InteractionContext(Client, cmd, cmd.Channel), null);
        return Task.CompletedTask;
    }

    private static async Task<Task> InteractionExecuted(SocketMessageComponent component) {
        ulong guildId = (ulong)component.GuildId!;
        string h = component.Data.CustomId;
        string[] t = h.Split(";");
        if (t[0] == "UPDATELB") {
            await component.UpdateAsync(m => {
                m.Embed = GetLeaderboardsEmbed(component.Guild(), (LeaderboardsType)int.Parse(t[1]));
                m.Components = GetLeaderboardsComponents((LeaderboardsType)int.Parse(t[1]));
            });
        }
        else if (t[0] == "SENDACHS") {
            if (t[1] == component.User.Id.ToString()) await component.RespondAsync(
                embed: GetAchEmbed(guildId, component.User.Id, AchievementCategory.CtqaHunt),
                components: GetAchComponents(AchievementCategory.CtqaHunt),
                ephemeral: true
            );
            else await component.RespondAsync("nouuuuu 😛😛😛😛😛😛 thats 🫂🫂🫂🫂 not 🔕🔕🔕🔕 yours <:insane:1136262312366440582><:insane:1136262312366440582>😼<:insane:1136262312366440582><:insane:1136262312366440582><:insane:1136262312366440582><:typing:1133071627370897580><:typing:1133071627370897580><:typing:1133071627370897580>", ephemeral: true);
        }
        else if (t[0] == "UPDATEACHS") await component.UpdateAsync(m => {
            m.Embed = GetAchEmbed(guildId, component.User.Id, (AchievementCategory)int.Parse(t[1]));
            m.Components = GetAchComponents((AchievementCategory)int.Parse(t[1]));
        });
        else if (t[0] == "CLEARTEAPOT") {
            var brewer = GetBrewer(guildId);
            brewer.Clear();
            await component.UpdateAsync(m => {
                m.Embed = brewer.Info(component.Guild());
                m.Components = brewer.InfoComponents();
            });
        }
        /*
        elif t[0] == "PINGONREPLY":
            id_to_update = int(t[1])
            if id_to_update != ctx.author.id:
                amount = give_cat(ctx.guild.id, ctx.author.id, 'Fine', -1)
                await ctx.send(f"you now have {amount} Fine ctqas for using not yours butotn 🍞", ephemeral=True)
            else:
                config = get_user_config(ctx.author.id)
                config["ping_on_catch"] = not config.get("ping_on_catch", True)
                save_user_config(config, ctx.author.id)
                await ctx.response.edit_message(
                    embed=disnake.Embed(title="your config 🍹🍹🍹🍹🍹🍹🍹🍹",
                                        description=f"```json\n{config}\n```"),
                    components=config_components(config, ctx.author.id)
                )*/
        else await component.RespondAsync($"o cholera 🔄 czy to Freddy Fazbear 🧸 har har har har har har har har har har har har 😛\ncomponent.Data.CustomId if it will be any useful>: `{component.Data.CustomId}`", ephemeral: true);
        return Task.CompletedTask;
    }

    private async Task<Task> Ready() {
        кансоль.WriteLine($"@{Client.CurrentUser} is now ready");
        await _interactionService.RegisterCommandsGloballyAsync();
        Data.StartTime = DateTime.Now;
        await SpawnCtqaLoopAsync();
        return Task.CompletedTask;
    }

    private async Task<Task> Client_MessageReceived(SocketMessage message) {
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
        if (message.MentionedUsers.Any(x => x.Id == Client.CurrentUser.Id)) {
            await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.PingBot);
        }
        if (msg == "полимерная глина в шкиле 🦈 и тысяча рублей за 48 сообщений в теме на солнце ☀️ бесплатно и смерть 💀 и не только у 😎") {
            await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Unknown);
        }
#pragma warning disable CA1862 // Используйте перегрузки метода "StringComparison" для сравнения строк без учета регистра
        if (sayToCatch == msg.ToUpper()) { // ТЫ ТУПОЙ БЛЯ
            if (sayToCatch == "ctqa") {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.CTQA);
            }
            else {
                await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.NOTCTQA);
            }
        }

        if (spawnMessageData != null && msgl == "cat") {
            await message.AddReactionAsync(GetEmoji("pointlaugh"));
        }
        if (msg.Equals(sayToCatch, StringComparison.CurrentCultureIgnoreCase)) {
            if (spawnMessageData != null && ServerConfig.IsWhitelistedStatic(message.GuildId(), message.Author)) {
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
                    amount = inv.IncrementCtqa(type);
                }
                try {
                    await ctqaMessage.DeleteAsync();
                    await message.DeleteAsync();
                }
                catch (Exception ex) {
                    await ((IMessageChannel)message).SendMessageAsync($"failed to delete ctqa spawn or \"ctqa\" message\n```\n{ex.Message}\n```");
                }
                string emoji = type.Emoji();
                await message.Channel.SendMessageAsync($"{message.Author.Mention} caught a {emoji} **{type.Name()} Ctqa** in **{FormatTime(time)}**!\n" +
                                                       $"You now have **{amount} {type.Name()} Ctqas** in your inventory");
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
            await message.ReplyFileAsync(GetImage("socialcredit.png"));
            await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.PleaseDoTheCtqa);
        }

        if (msgl == "please do not the ctqa") {
            await message.ReplyAsync($"ok then\n{message.Author.Mention} lost one fine ctqa!!!!11");
            using var inv = Inventory.Load(message.GuildId(), message.Author.Id);
            inv.DecrementCtqa(CtqaType.Fine);
            await inv.GiveAchAsync(message.Channel, message.Author, AchievementId.PleaseDoNotTheCtqa);
        }
                
        if (msgl.Contains(":syating_ctqa:") && msgl.Contains("🛐")) {
            await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.Worship);
        }
            
        if ("ctqa!lol_i_have_dmed_ctqa_and_got_an_ach" == msg) {
            await Inventory.GiveAchAsyncStatic(message.Channel, message.GuildId(), message.Author, AchievementId.DmBot);
        }
        string[] args = message.Content.Split();
        if (args[0] == "ctqa!whitelist") {
            if (args.Length < 2) {
                await message.ReplyAsync("no id specified");
            }
            else if (!message.Author.SkillIssued()) {
                ulong id = ulong.Parse(args[1]);
                if (ServerConfig.UpdateWhitelistStatic(message.GuildId(), id)) await message.ReplyAsync($"**{GetName(id)}** was whitelisted");
                else await message.ReplyAsync($"**{GetName(id)}** was removed from whitelist");
            }
        }
        if (args[0] == "ctqa!custom") {
            if (args.Length < 2) {
                await message.ReplyAsync("no user specified");
            }
            else if (!ulong.TryParse(args[1], out _)) {
                await message.ReplyAsync("user id must be ulong");
            }
            else if (args.Length < 3) {
                await message.ReplyAsync("no type specified");
            }
            else if (!message.Author.SkillIssued()) {
                if (Enum.TryParse(args[2], out CtqaType type)) {
                    UserConfig.SetCustomCtqaStatic(ulong.Parse(args[1]), type);
                    await message.ReplyAsync("Success!!1113" + string.Concat(Enumerable.Range(0, 20).Select(_ => (char)RandInt(45, 65))));
                }
                else {
                    await message.ReplyAsync($"Can't parse {args[2]} to CtqaType enum");
                }
            }
        }
        if (args[0] == "ctqa!tractor") {
            await message.AddReactionAsync(Emoji.Parse("🚜"));
        }
        if (message.Content.StartsWith("ctqa!news") && Data.TrustedPeople.Contains(message.Author.Id)) {
            foreach (Tuple<ulong, ulong> tuple in new List<Tuple<ulong, ulong>>() { new(1287684990041063445, 1291064242040078538) }.Concat(GetCtqasChannels())) {
                await ((IMessageChannel)Client.GetChannel(tuple.Item2)).SendMessageAsync(message.Content[10..]);
            }
        }
        return Task.CompletedTask;
    }

    private static Task Log(LogMessage msg) {
        кансоль.WriteLine(msg);
        return Task.CompletedTask;
    }
}
internal class CommandModule : InteractionModuleBase {
    public required InteractionService Service { get; set; }

    [SlashCommand("ping", "get ping")]
    public async Task Ping() {
        int h = ((DiscordSocketClient)Context.Client).Latency;
        await RespondAsync(h switch {
            >= 10000 => $"ctqa  iahbr an d<:syating_ctqa:1178288745435385896>lsy o{h} fmsae",
            >= 8000 => $"ci a<:syating_ctqa:1178288745435385896>has bratqmdelay o n{h}  sf",
            >= 5000 => $"cn a <:syating_ctqa:1178288745435385896>at br isadel yaofq{h}ms h",
            >= 3000 => $"htqa cysrb fan delai oa {h}ms <:syating_ctqa:1178288745435385896>",
            >= 2000 => $"cdqa{h}hastbrain  e  ylof  msa<:syating_ctqa:1178288745435385896>",
            >= 1500 => $"ct a<:syating_ctqa:1178288745435385896>ahsqbrain delay of {h} s m",
            >= 1000 => $"ctsbohas arain delay  f {h}mq <:syating_ctqa:1178288745435385896>",
            >= 500 => $"ctqa<:syating_ctqa:1178288745435385896>has brain delay of {h}ms",
            >= 300 => $"ctqa hasnbrai  delay of {h}ms <:syating_ctqa:1178288745435385896>",
            _ => $"ctqa has bran delayt of {h}ms <:syating_ctqa:1178288745435385896>"
        });
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
            await RespondAsync($"there is already a ctqa spawn loop in {channel.GetUrl()}");
            return;
        }
        channels.Add(tuple);
        await RespondAsync($"**#{Context.Channel}** was added to ctqa spawn list ✅");
        SetCtqasChannels(channels);
    }

    [SlashCommand("inv", "view your inventory")]
    public async Task InventorySlashCommand(IUser? member = null) => await RespondAsync(embed: Inventory.GetInvEmbed(
        Context.Guild.Id,
        member ?? Context.User,
        member == null
    ));

    [SlashCommand("achs", "see your achievements")]
    public async Task AchsSlashCommand() => await RespondAsync(
        embed: new EmbedBuilder() { Title = "Your achievements:", Description = GetAchsCountStatic(Context.Guild.Id, Context.User.Id) }.Build(),
        components: MakeComponents([new Button("View achievements", $"SENDACHS;{Context.User.Id}")])
    );

    [SlashCommand("lb", "see leaderboards for this server")]
    public async Task LeaderboardsSlashCommand() => await RespondAsync(
        embed: GetLeaderboardsEmbed(Context.Guild, LeaderboardsType.Ctqas),
        components: GetLeaderboardsComponents(LeaderboardsType.Ctqas)
    );

    [SlashCommand("nerdmode", "🤓🤓🤓🤓🤓🤓")]
    public async Task NerdmodeSlashCommand(IUser user) {
        if (user.SkillIssued()) await RespondAsync("i dont think you are allowed to use this", ephemeral: true);
        else if (ServerConfig.UpdateBlacklistStatic(Context.Guild.Id, user.Id)) await RespondAsync($"**{user.FullName()}** was nerdmoded, Fs in chat");
        else await RespondAsync($"**{user.FullName()}** was removed from nerdmode");
    }

    [SlashCommand("info", "get info about bot")]
    public async Task InfoSlashCommand() => await RespondAsync(embed: new EmbedBuilder() {
        Title = $"ctqa bto",
        Description = $@"[support server](https://discord.gg/pC5u5pGwPJ) | [source code](https://github.com/antigrav-technologies/ctqa-bto)

A cat bot clone
Spawns ctqas, and probably actually does that
Run /setup to make spawn loop
If they randomly stopped spawning try running /setup again

thanks to:
- **{Program.Client.GetUser(986132157967761408).FullName()}** for syating ctqa image and making ctqa icons
- **{Program.Client.GetUser(802846743049404426).FullName()}** for hosting ctqa bto",
        Footer = new() { Text = GetVersion() }
    }.Build()
    );

    [SlashCommand("ctqas", "get list of ctqa spawn chances")]
    public async Task CtqasSlashCommand() => await RespondAsync(
        $"ctqa chances: ```\n" + string.Join('\n', TypeDict.Select(kvp => $"{kvp.Key,-15}{(float)kvp.Value / TotalWeight * 100:F4}%")) +
        "```\ncustom ctqas:```\n" + string.Join(", ", CustomTypes) + "\n```"
    );
    [SlashCommand("coupon", "use coupon")]
    public async Task CouponSlashCommand(string coupon) {
        if (ServerConfig.TryUseCouponStatic(Context.Guild.Id, coupon, out Coupon? couponNullable)) {
            Coupon coupon1 = (Coupon)couponNullable!;
            Inventory.RecieveCoupon(Context.Guild.Id, Context.User.Id, coupon1);
            await RespondAsync($"you used coupon for {coupon1.Amount} {coupon1.Name} ctqas!!!1");
        }
        else await RespondAsync("unknown coupon", ephemeral: true);
    }

    [SlashCommand("gift", "give ctqas pls")]
    public async Task GiftSlashCommand(IUser user, [Autocomplete(typeof(CtqasAutocomplete))]string type, long amount = 1) {
        if (user.Id == Context.User.Id) {
            await RespondAsync("uhhhhh", ephemeral: true);
            return;
        }
        if (amount < 1) {
            await RespondAsync("nooouuuuu 😛😛😛😛", ephemeral: true);
        }
        if (TryGetType(type, out var ctqaTypeNullable)) {
            CtqaType ctqaType = (CtqaType)ctqaTypeNullable!;
            using (var inv = Inventory.Load(Context.Guild.Id, Context.User.Id)) {
                if (!inv.HasAmount(ctqaType, amount)) {
                    await RespondAsync($"you dont have that many {type} ctqas\n(you have only {inv[ctqaType]} and wanted to donate {amount})");
                    inv.DisposeIt = false;
                    return;
                }
                inv.DecrementCtqa(ctqaType);
                await inv.GiveAchAsync(Context.Channel, Context.User, AchievementId.Donator);

                using var reciever = Inventory.Load(Context.Guild.Id, user.Id);
                Inventory.GiftCtqas(inv, reciever, ctqaType, amount);
                await reciever.GiveAchAsync(Context.Channel, user, AchievementId.AntiDonator);
            }
            await RespondAsync($"{Context.User.Mention} gave {user.Mention} {amount} {type} ctqas!!!!!!!");
        }
        else {
            await RespondAsync($"ctqa type {type} doesn't exist");
        }
    }
    /*
    [SlashCommand("teapot", "get info about teapot")]
    public async Task TeapotSlashCommand() {
        var brewer = GetBrewer(Context.Guild.Id);
        await RespondAsync(embed: brewer.Info(Context.Guild), components: brewer.InfoComponents());
    }
    
    [SlashCommand("brew", "Brew a coffee")]
    public async Task BrewSlashCommand(string coffee_type = "ъ", int milk = 0, int sugar = 0) {
        HttpStatusCode code = GetBrewer(Context.Guild.Id).TryToBrewCoffee(Context.User.Id, coffee_type, milk, sugar);
        await RespondAsync(code.Description());
        using var inv = Inventory.Load(Context.Guild.Id, Context.User.Id);
        inv.DisposeIt = false;
        if (code == HttpStatusCode.ImATeapot) {
            await inv.GiveAchAsync(Context.Channel, Context.User, AchievementId.IAmATeapot);
            inv.DisposeIt = true;
        }
    }*/

    [SlashCommand("holy", "HOLY CTQA")]
    public async Task HolySlashCommand() {
        await RespondWithFileAsync(GetImage("holy_ctqa.jpg"));
        await Inventory.GiveAchAsyncStatic(Context.Channel, Context.Guild.Id, Context.User, AchievementId.Holy);
    }
}

public class CtqasAutocomplete : AutocompleteHandler {
#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services) => AutocompletionResult.FromSuccess(CtqaTypes.Select(x => new AutocompleteResult(x.Name(), x.Name())).Take(25));
}
