using static CtqaBto.Utils;

namespace CtqaBto;

internal static class Data {
    public static Random random = new();
    public static DateTime StartTime = DateTime.MinValue;
    public static readonly string InventoriesData = GetFolderPath(["ctqas"]);
    public static readonly string CtqaChannelsPath = GetFilePath(["ctqa channels.antigrav"], "null");
    public static readonly string CtqasPath = GetFilePath(["ctqas.antigrav"], "null");
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
        "download rech2020 games for FREE",
        "CtqaLink",
        "29A:AA79//@A>@4-->.4>",
        "🌶️ копуста 🥐 🥧 🥧 перчик 🤯 🤯 перчик 🥧 перчик 🌶️ 🥐 🥐 🤯 🥐 пирчик копуста 🤯 киписти перчик киписти киписти копуста копуста копуста 🥐 копуста перчик 🌶️ копуста 🥧 пирчик перчик копуста копуста киписти перчик киписти копуста перчик 🥐 🥧 перчик пирчик перчик 🌶️ 🤯 🥧 🤯 🤯 🥧 киписти копуста киписти 🤯 пирчик 🥧 🤯 копуста перчик 🌶️ копуста киписти копуста 🤯 пирчик 🥐 пирчик перчик перчик пирчик пирчик перчик 🥧 🌶️ 🌶️ 🥧 🥧 копуста 🌶️ пирчик пирчик 🥧 киписти 🥧 киписти 🌶️ 🌶️ 🥐 киписти 🌶️ пирчик 🤯 🌶️ киписти пирчик 🥐 киписти 🌶️",
        "untitled goose game 😔🎯",
        "icosAHERONOOOOOOOOOOOOOOOO.ICOSAHEDROOOOOOOOOOOOOOOOD.S..ICIOCDHAOIDNEFP2O43[32[3PVMFDOVPFDMVNNM",
        "why did mister breasts go to new zealand? because of electric caterpillars! get it? because i am kreisi",
        "Me when i see the kreisi bruglar making oickles,: 😁😁😁📥🍔🍔😑🦣🦣😆😭🕷️🪞🥒🥐🥒🥒📚🦑",
        "balls to the store 🏪😁balls to the store 🏪😁balls to the store 🏪😁balls to the store 🏪😁balls to the store 🏪😁balls to the store 🏪😁balls to the store 🏪😁",
        "kreisi burglar alarm clock ⌚  ⏰",
        "*kreisi music plays*",
        "#TeamPicardias for life",
        "how to pull a proglet out of a hat",
        "OKG FREE BOBUX REAL??!???🐂😎😎😎😎😎😝😝😝😝😝😱😱😱😱",
        "https://media.discordapp.net/attachments/1042064947867287646/1173531283725504512/attachment.png",
        "ъ",
        "context 😨😂🤣😦😮🙀less",
        "```cs\nthrow new ArgumentException(\"sniffshit hump 🐪🐫\");\n```",
        "transgender carbohydrates",
        "А КТО ТОКИЕ ФИКСЕКЕ 🤔🤔🤔 БАЛШОЙ БОЛШОЙ СЕКРЕТ ㊙️",
        "By the way did you know that i use the GNU/Linux distribution for x86-64 processor architecture known as Arch Linux?",
        "я не знаю как здорово и огород и огород и огород 😼 не могу подключить и смерть в раю и огород и не только от вас 🐋🤩🐋",
        "https://media.discordapp.net/attachments/810858829767639081/1078004234055847946/chrome_4euY54brJE.png",
        "helo my name is abomtin and i chacked CloroxBle.. wait what",
        "https://media.discordapp.net/attachments/1205124620227846186/1236360856246419476/remix-ff6d164f-3c98-4a1c-b231-c794fae51259.png",
        "https://media.discordapp.net/attachments/1205124620227846186/1224361212142161931/Octogrow.gif",
        "😾🐵🐅👀🧶🎱🀄🧪🗿📐🍘🍑🌵💢🕳️🅰️❌❗☢️☣️⚠️♻",
        "Знакомьтесь с клавиатурой Gboard! Здесь будет сохраняться текст, который вы копируете.",
        "обсидиван 🛋️ тайм ⌛",
        "asexual but does for the rtx 4090 graphics for the internet 🛜😁😁",
        "foo52 техношаман 🧙",
        "я робот долбаёб",
        "пшеничная каша в голове 😞 и не только в понедельник с дедушкой морозом 🎅",
        "сниф щит 🛡️ джампскейр 😱",
        "https://media.discordapp.net/attachments/1042064947867287646/1199047070116495470/image.png",
        "https://media.discordapp.net/attachments/1042064947867287646/1198242355392811088/image.png",
        "https://github.com/tema5002/ammeter/commit/1eeeb3ed1839fd41fbb5cbd2f219466b6932245f",
        "https://media.discordapp.net/attachments/1042064947867287646/1198242415685947513/image.png",
        "https://media.discordapp.net/attachments/1042064947867287646/1198258848738316370/image.png",
        "📴♦️📵📵📵📵📵📵📵effafiigiiaiigigigiaapgoggiapggpaogogigoggogoggG?G  :DDDD",
        "inase:insae::sanine:insan::isnane:insnae:insanias:infainfainfainfa:isane:<:insane:1136262312366440582>",
        "https://media.discordapp.net/attachments/1042064947867287646/1198274980320911440/image.png",
        "©️©️cocoopyright",
        "https://media.discordapp.net/attachments/1042064947867287646/1198276452689391717/image.png",
        "⬅️<:typing:1133071627370897580>🛡️🧱⚔️<:typing:1133071627370897580>➡️↪️❌♻️",
        "https://media.discordapp.net/attachments/1100477068787077140/1292935882613067857/Screenshot_20241007-224506.png",
        "sanity and my mind is full of the most informative and equilateral triangle basement is a wuggy games 🎯💯😎😄🎧😊⭐🪥",
        "ctqa inverted triangle 📐▶️ Attach a debugger to make a server using the most informative video 📷📸😃😉😁",
        "gboard is a wuggy games 🎯💯😎😄🎧😊⭐🪥🍹🥘🎉🥮🍿🍪🍭🍮🍯🍡‼️✔️",
        "УГЛЕВОДОРОДЫ ВПЕРЁД ♿♿♿♿♿♿♿♿",
        "БАТАРЕЙКИ БАТАРЕЙКИ\nБАТАРЕЙКИ БАТАРЕЙКИ\nБАТАРЕЙКИ БАТАРЕЙКИ\nБАТАРЕЙКИ БАТАРЕЙКИ",
        "KURWA 🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱🇵🇱",
        "kurzgesagt bird 🐔🕊️🐔🐔🕊️🕊️🕊️🕊️🐦😎"
        "асексуальный с арбузным холодильником 🤠"
        "REPOST THIS MESSAGE IF YOU HATE RIVER OPTIC CABLE 🚠🚡🚡📄💬😠😾😡😞😭🙄😑😐",
        "Brought You Here By The Letter G",
        "polyhedra geometry dash lua discord bot abotmin russia programming",
        "my name is john discord and i invented discord 🦣",
        "майонез_dl forever"
    ];
    public static readonly string Datamine = "ctqa!ΔπβΔ©🐙αλ1Σhh1π1π©🐙Σ1π©βπΔΔ1βππhαββπλβππ🐙ΔhhαΔΔΣ1π🐙βλhαπβ©βββ1πΣβ🐙πΔβΣΔ🐙©αλαh🐙hΣβπh©ΣΔΔ🐙πλΣλλ11λhα🐙Δh©β©©πΔ©ΣβhΔλ🐙πΔβΔΔ🐙©ΣβββλαΔΣπ";
}
