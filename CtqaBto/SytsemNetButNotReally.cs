using Discord;
using static CtqaBto.Configs;
using static CtqaBto.Utils;

namespace CtqaBto;

public static class Teapot {
    public enum HttpStatusCode {
        OK = 200,
        Accepted = 202,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        UnsupportedMediaType = 415,
        ImATeapot = 418,
        TooManyRequests = 429,
        InternalServerError = 500,
    }
    private static readonly Dictionary<HttpStatusCode, string> HttpInfo = new() {
        { HttpStatusCode.OK, "200 OK -- The coffee has been brewed successfully."},
        { HttpStatusCode.Accepted, "202 Accepted: -- The coffee brewing request has been accepted, please wait."},
        { HttpStatusCode.BadRequest, "400 Bad Request -- The request is malformed or contains errors, such as an invalid milk or sugar amount."},
        { HttpStatusCode.Forbidden, "403 Request forbidden -- The user is forbidden from brewing coffee, see Icosahedron Industries™️ policy."},
        { HttpStatusCode.NotFound, "404 Not Found -- The coffee maker, coffee beans or anything else couldn't be found"},
        { HttpStatusCode.UnsupportedMediaType, "415 Unsupported Media Type -- The coffee beans must be in D64 format"},
        { HttpStatusCode.ImATeapot, "418 I'm a Teapot -- Server refuses to brew coffee because it is a teapot."},
        { HttpStatusCode.TooManyRequests, "429 Too Many Requests -- The user has sent too many requests in a given amount of time (\"rate limiting\")"},
        { HttpStatusCode.InternalServerError, "500 Internal Server Error -- The server bricked itself"},
    };
    public static string Description(this HttpStatusCode code) => HttpInfo.TryGetValue(code, out string? description) ? description : "Unknown error happened";
    public class CoffeeBrewer {
        public readonly ulong GuildId;
        private const float MAX_MILK = 1f;
        private const float MAX_SUGAR = 1f;
        private bool Bricked;
        private float Capacity;
        private float Loaded = 0f;
        private float WaterAvailable;
        private float MilkAvailable;
        private float SugarAvailable;
        private Dictionary<ulong, List<DateTime>> TeapotUses = [];
        public DateTime StartedAt = DateTime.MinValue;
        public HttpStatusCode? LastStatus = null;

        public CoffeeBrewer(ulong guildId) {
            GuildId = guildId;
            Reset();
        }

        public void Clear() => Loaded = 0f;

        public void Reset() {
            Bricked = false;
            Capacity = 4.0f;
            WaterAvailable = 20.0f;
            MilkAvailable = 5.0f;
            SugarAvailable = 5.0f;
            TeapotUses = [];
            StartedAt = DateTime.Now;
        }

        public Embed Info(IGuild guild) => new EmbedBuilder() {
            Title = $"{guild.Name} teapot info: ",
            Description = $"Bricked: {(Bricked ? "yes" : "no")}\n" +
                          $"Capacity: {Capacity} cups\n" +
                          $"Loaded: {Loaded}\n" +
                          $"Water Available: {WaterAvailable}\n" +
                          $"Milk Available: {MilkAvailable}\n" +
                          $"\n" +
                          $"Last status: {LastStatus}"
        }.Build();

        public MessageComponent InfoComponents() => MakeComponents([
            new Button("Clear", $"CLEARTEAPOT;{GuildId}")
        ]);

        public HttpStatusCode TryToBrewCoffee(ulong userId, string seed, float milk, float sugar) {
            if (Math.Abs((StartedAt - DateTime.Now).TotalHours) > 1) Reset();
            if (!Bricked) Bricked = Data.Random.Next(0, 200) == 0;
            if (Bricked) return HttpStatusCode.InternalServerError;
            if (ServerConfig.IsNotAllowToUseTeapotStatic(GuildId, userId)) return HttpStatusCode.Forbidden;
            if (RandIntFromString(seed, 0, 4) == 0) return HttpStatusCode.UnsupportedMediaType;
            if (milk > MAX_MILK || sugar > MAX_SUGAR) return HttpStatusCode.BadRequest;
            if (milk > MilkAvailable || sugar > SugarAvailable) return HttpStatusCode.NotFound;
            if (Loaded + 1f + milk + sugar > Capacity) return HttpStatusCode.BadRequest;

            if (TeapotUses.TryGetValue(userId, out var datetimes)) {
                if (datetimes.Where(x => Math.Abs((DateTime.Now - x).TotalMinutes) < 5).Count() >= 5) return HttpStatusCode.TooManyRequests;
                TeapotUses[userId].Add(DateTime.Now);
            }
            else TeapotUses[userId] = [DateTime.Now];
            if (RandIntFromString(seed, 0, RandIntFromSeed((int)((GuildId + userId)/2), 180, 240)
                                           - RandIntFromString(seed, 0, 75)
                                           - (int)(milk * 20)
                                           - (int)(sugar * 20)
               ) == 0) return HttpStatusCode.ImATeapot;
            Loaded += 1f + milk + sugar;
            if (Data.Random.Next(0, 2) == 0) return HttpStatusCode.OK;
            return HttpStatusCode.Accepted;
        }
    }

    private static readonly List<CoffeeBrewer> Brewers = [];

    public static CoffeeBrewer GetBrewer(ulong guildId) {
        foreach (CoffeeBrewer brewer in Brewers) {
            if (brewer.GuildId == guildId) {
                return brewer;
            }
        }
        Brewers.Add(new CoffeeBrewer(guildId));
        return Brewers.Last();
    }
}