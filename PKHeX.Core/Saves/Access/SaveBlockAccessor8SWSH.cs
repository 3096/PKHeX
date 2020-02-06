﻿using System.Collections.Generic;
using System.Linq;

namespace PKHeX.Core
{
    public class SaveBlockAccessor8SWSH : SCBlockAccessor, ISaveBlock8Main
    {
        public Dictionary<uint, SCBlock> KeyBlockMap { get; }
        public override IReadOnlyList<SCBlock> BlockInfo { get; }

        public Box8 BoxInfo { get; }
        public Party8 PartyInfo { get; }
        public MyItem Items { get; }
        public MyStatus8 MyStatus { get; }
        public Misc8 Misc { get; }
        public Zukan8 Zukan { get; }
        public BoxLayout8 BoxLayout { get; }
        public PlayTime8 Played { get; }
        public Fused8 Fused { get; }
        public Daycare8 Daycare { get; }
        public Record8 Records { get; }
        public TrainerCard8 TrainerCard{ get; }
        public FashionUnlock8 Fashion { get; }
        public RaidSpawnList8 Raid { get; }
        public TitleScreen8 TitleScreen { get; }
        public TeamIndexes8 TeamIndexes { get; }

        public Dictionary<uint, object> KeyAccessorMap;

        public SaveBlockAccessor8SWSH(SAV8SWSH sav)
        {
            BlockInfo = sav.AllBlocks;
            KeyBlockMap = sav.AllBlocks.ToDictionary(z => z.Key);
            BoxInfo = new Box8(sav, GetBlock(KBox));
            PartyInfo = new Party8(sav, GetBlock(KParty));
            Items = new MyItem8(sav, GetBlock(KItem));
            Zukan = new Zukan8(sav, GetBlock(KZukan));
            MyStatus = new MyStatus8(sav, GetBlock(KMyStatus));
            Misc = new Misc8(sav, GetBlock(KMisc));
            BoxLayout = new BoxLayout8(sav, GetBlock(KBoxLayout));
            TrainerCard = new TrainerCard8(sav, GetBlock(KTrainerCard));
            Played = new PlayTime8(sav, GetBlock(KPlayTime));
            Fused = new Fused8(sav, GetBlock(KFused));
            Daycare = new Daycare8(sav, GetBlock(KDaycare));
            Records = new Record8(sav, GetBlock(KRecord), Core.Records.MaxType_SWSH);
            Fashion = new FashionUnlock8(sav, GetBlock(KFashionUnlock));
            Raid = new RaidSpawnList8(sav, GetBlock(KRaidSpawnList));
            TitleScreen = new TitleScreen8(sav, GetBlock(KTitleScreenTeam));
            TeamIndexes = new TeamIndexes8(sav, GetBlock(KTeamIndexes));

            KeyAccessorMap = new Dictionary<uint, object>
            {
                {KBox, BoxInfo},
                //{KMysteryGift,},
                {KItem, Items},
                //{KCoordinates,},
                {KBoxLayout, BoxLayout},
                {KMisc, Misc},
                {KParty, PartyInfo},
                {KDaycare, Daycare},
                {KRecord, Records},
                {KZukan, Zukan},
                {KTrainerCard, TrainerCard},
                {KPlayTime, Played},
                {KRaidSpawnList, Raid},
                //{KRepel,},
                {KFused, Fused},
                {KFashionUnlock, Fashion},
                {KMyStatus, MyStatus},
                {KTitleScreenTeam, TitleScreen},
                {KTeamIndexes, TeamIndexes},
            };
        }

        /* To dump key list of current format, use the following in the immediate window, and update Meta8
        var blocks = BlockInfo.Where(z => z.Data.Length != 0).Select(z => new KeyValuePair<uint, int>(z.Key, z.Data.Length)).Select(z => $"{z.Key:X8}, {z.Value:X5},");
        System.IO.File.WriteAllLines("blank.txt", blocks.ToArray());
        */

        // Arrays (Blocks)
        private const uint KTeamNames = 0x1920C1E4; // Team 1, 2...6 ((10 + terminator)*6 char16 strings)
        private const uint KBoxLayout = 0x19722c89; // Box Names

        // Objects (Blocks)
        private const uint KBox = 0x0d66012c; // Box Data
        private const uint KMysteryGift = 0x112d5141; // Mystery Gift Data
        private const uint KItem = 0x1177c2c4; // Items
        private const uint KCoordinates = 0x16aaa7fa; // Coordinates?
        private const uint KMisc = 0x1b882b09; // Money
        private const uint KParty = 0x2985fe5d; // Party Data
        private const uint KDaycare = 0x2d6fba6a; // Daycare slots (2 daycares)
        private const uint KTeamIndexes = 0x33F39467; // Team Indexes for competition
        private const uint KRecord = 0x37da95a3;
        private const uint KZukan = 0x4716c404; // PokeDex
        private const uint KCurryDex = 0x6EB72940; // Curry Dex
        private const uint KTrainerCard = 0x874da6fa; // Trainer Card
        private const uint KPlayTime = 0x8cbbfd90; // Time Played
        private const uint KRaidSpawnList = 0x9033eb7b; // Nest current values (hash, seed, meta)
        private const uint KFused = 0xc0de5c5f; // Fused PKM (*3)
        private const uint KFashionUnlock = 0xd224f9ac; // Fashion unlock bool array (owned for (each apparel type) * 0x80, then another array for "new")
        private const uint KTitleScreenTeam = 0xE9BE28BF; // Title Screen Team details
        public const uint KEnteredHallOfFame = 0xE2F6E456; // U64 Unix Timestamp
        private const uint KMyStatus = 0xf25c070e; // Trainer Details

        // Raid DLC Flatbuffer Storage Objects (Blocks)
        private const uint KDropRewards = 0x680EEB85; // drop_rewards
        private const uint KDaiEncount = 0xAD3920F5; // dai_encount
        private const uint KNormalEncount = 0xAD9DFA6A; // normal_encount
        private const uint KBonusRewards = 0xEFCAE04E; // bonus_rewards

        // Values
        public const uint KGameLanguage = 0x0BFDEBA1; // U32 Game Language
        public const uint KRepel = 0x9ec079da; // U16 Repel Steps remaining
        public const uint KRotoRally = 0x38548020; // U32 Roto Rally Score (99,999 cap)
        public const uint KBattleTowerSinglesVictory = 0x436CAF2B; // U32 Singles victories (9,999,999 cap)
        public const uint KBattleTowerDoublesVictory = 0x0D477836; // U32 Doubles victories (9,999,999 cap)
        public const uint KBattleTowerSinglesStreak = 0x6226F5AD; // U16 Singles Streak (300 cap)
        public const uint KBattleTowerDoublesStreak = 0x5F74FCEE; // U16 Doubles Streak (300 cap)
        public const uint KStarterChoice = 0x3677602D; // U32 Grookey=0, Scorbunny=1, Sobble=2
        public const uint KDiggingDuoStreakSkill = 0xA0F49CFB; // U32
        public const uint KDiggingDuoStreakStamina = 0x066F38F5; // U32

        public SCBlock GetBlock(uint key) => KeyBlockMap[key];
    }
}
