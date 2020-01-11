﻿using System.Collections.Generic;

namespace PKHeX.Core
{
    public class SaveBlockAccessorSWSH : ISaveBlockAccessor<SCBlock>, ISaveBlock8Main
    {
        public IReadOnlyList<SCBlock> BlockInfo { get; }
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


        public Dictionary<uint, object> BlockDict;

        public SaveBlockAccessorSWSH(SAV8SWSH sav)
        {
            BlockInfo = sav.AllBlocks;
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

            BlockDict = new Dictionary<uint, object>
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
            };
        }

        /* To dump key list of current format, use the following in the immediate window, and update Meta8
        var blocks = BlockInfo.Where(z => z.Data.Length != 0).Select(z => new KeyValuePair<uint, int>(z.Key, z.Data.Length)).Select(z => $"{z.Key:X8}, {z.Value:X5},");
        System.IO.File.WriteAllLines("blank.txt", blocks.ToArray());
        */
        private const uint KBox = 0x0d66012c; // Box Data
        private const uint KMysteryGift = 0x112d5141; // Mystery Gift Data
        private const uint KItem = 0x1177c2c4; // Items
        private const uint KCoordinates = 0x16aaa7fa; // Coordinates?
        private const uint KBoxLayout = 0x19722c89; // Box Names
        private const uint KMisc = 0x1b882b09; // Money
        private const uint KParty = 0x2985fe5d; // Party Data
        private const uint KDaycare = 0x2d6fba6a; // Daycare slots (2 daycares)
        private const uint KRecord = 0x37da95a3;
        private const uint KZukan = 0x4716c404; // PokeDex
        private const uint KTrainerCard = 0x874da6fa; // Trainer Card
        private const uint KPlayTime = 0x8cbbfd90; // Time Played
        private const uint KRaidSpawnList = 0x9033eb7b; // Nest current values (hash, seed, meta)
        private const uint KRepel = 0x9ec079da;
        private const uint KFused = 0xc0de5c5f; // Fused PKM (*3)
        private const uint KFashionUnlock = 0xd224f9ac; // Fashion unlock bool array (owned for (each apparel type) * 0x80, then another array for "new")
        private const uint KMyStatus = 0xf25c070e; // Trainer Details

        // Rather than storing a dictionary of keys, we can abuse the fact that the SCBlock[] is stored in order of ascending block key.
        // Binary Search doesn't require extra memory like a Dictionary would; also, we only need to find a few blocks.
        public SCBlock GetBlock(uint key) => BinarySearch(BlockInfo, key);

        private static SCBlock BinarySearch(IReadOnlyList<SCBlock> arr, uint key)
        {
            int min = 0;
            int max = arr.Count - 1;
            do
            {
                int mid = (min + max) / 2;
                var entry = arr[mid];
                var ek = entry.Key;
                if (key == ek)
                    return entry;

                if (key < ek)
                    max = mid - 1;
                else
                    min = mid + 1;
            } while (min <= max);
            throw new KeyNotFoundException(nameof(key));
        }
    }
}