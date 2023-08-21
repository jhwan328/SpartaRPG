using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SpartaRPG.Classes;

namespace SpartaRPG.Managers
{
    internal class DataManager
    {
        public GameManager GameManager;

        public Character Player { get; private set; }
        public List<Item> Inventory { get; private set; }
        public List<Item> Shop { get; private set; }
        public List<Dungeon> Dungeons { get; private set; }


        private Item[] _items = new Item[50];

        private string _path = "data.json";



        public void SaveData()
        {
            int[] inventoryIds = new int[Inventory.Count];

            JObject configData = new JObject(
                new JProperty("Name", Player.Name),
                new JProperty("Job", Player.Job),
                new JProperty("Level", Player.Level),
                new JProperty("Exp", Player.Exp),
                new JProperty("Atk", Player.Atk),
                new JProperty("Def", Player.Def),
                new JProperty("MaxHp", Player.MaxHp),
                new JProperty("CurrentHp", Player.CurrentHp),
                new JProperty("Gold", Player.Gold)
                );

            for (int i = 0; i < Inventory.Count; i++)
                inventoryIds[i] = Inventory[i].Id;
            configData.Add(new JProperty("inventory", JArray.FromObject(inventoryIds)));


            if (Player.Equipments[(int)Item.Parts.WEAPON] != null)
                configData.Add(new JProperty("Weapon", Player.Equipments[(int)Item.Parts.WEAPON].Id));
            if (Player.Equipments[(int)Item.Parts.HELMET] != null)
                configData.Add(new JProperty("Helmet", Player.Equipments[(int)Item.Parts.HELMET].Id));
            if (Player.Equipments[(int)Item.Parts.CHESTPLATE] != null)
                configData.Add(new JProperty("ChestPlate", Player.Equipments[(int)Item.Parts.CHESTPLATE].Id));
            if (Player.Equipments[(int)Item.Parts.LEGGINGS] != null)
                configData.Add(new JProperty("Leggings", Player.Equipments[(int)Item.Parts.LEGGINGS].Id));
            if (Player.Equipments[(int)Item.Parts.BOOTS] != null)
                configData.Add(new JProperty("Boots", Player.Equipments[(int)Item.Parts.BOOTS].Id));

            File.WriteAllText(_path, configData.ToString());
        }

        public bool LoadData()
        {
            if (File.Exists(_path)) return false;

            dynamic data = JsonConvert.DeserializeObject(File.ReadAllText(_path));

            Player = new Character(
                data["Name"].ToString(),
                data["Job"].ToString(),
                (int)data["Level"],
                (int)data["Atk"],
                (int)data["Def"],
                (int)data["MaxHp"],
                (int)data["Gold"]
                );

            Player.Exp = (int)data["Exp"];

            Inventory.Clear();
            foreach (var id in data["inventory"])
            {
                Inventory.Add(_items[(int)id]); _items[(int)id].IsSold = true;
            }

            if (data["Weapon"] != null)
            {
                Player.Equipments[(int)Item.Parts.WEAPON] = _items[(int)data["Weapon"]];
                _items[(int)data["Weapon"]].IsEquipped = true;
            }
            if (data["Helmet"] != null)
            {
                Player.Equipments[(int)Item.Parts.HELMET] = _items[(int)data["Helmet"]];
                _items[(int)data["Helmet"]].IsEquipped = true;
            }
            if (data["ChestPlate"] != null)
            {
                Player.Equipments[(int)Item.Parts.CHESTPLATE] = _items[(int)data["ChestPlate"]];
                _items[(int)data["ChestPlate"]].IsEquipped = true;
            }
            if (data["Leggings"] != null)
            {
                Player.Equipments[(int)Item.Parts.LEGGINGS] = _items[(int)data["Leggings"]];
                _items[(int)data["Leggings"]].IsEquipped = true;
            }
            if (data["Boots"] != null)
            {
                Player.Equipments[(int)Item.Parts.BOOTS] = _items[(int)data["Boots"]];
                _items[(int)data["Boots"]].IsEquipped = true;
            }

            Player.ChangeHP((int)data["CurrentHp"] - Player.MaxHp);

            return true;
        }

        public void Wear(Item item)
        {
            Player.Equipments[(int)item.Part] = item;
            item.IsEquipped = true;
        }

        public void Unwear(Item.Parts part)
        {
            Player.Equipments[(int)part].IsEquipped = false;
            Player.Equipments[(int)part] = null;
        }

        public void BuyItem(Item item)
        {
            Player.Gold -= item.Price;
            Inventory.Add(item);
            item.IsSold = true;
            GameManager.Instance.UIManager.PrintGold();
            GameManager.Instance.UIManager.PrintShopItems(true);
        }

        public void SellItem(Item item)
        {
            Player.Gold += (int)(item.Price * 0.85f);
            Inventory.Remove(item);
            item.IsSold = false;
            GameManager.Instance.UIManager.PrintGold();
            GameManager.Instance.UIManager.PrintInventoryItems(true);
        }
        public void GameDataSetting()
        {
            // 캐릭터 정보 세팅
            Character warrior = new Character("기현빈", "전사", 1, 10, 5, 100, 1500);
            Character archer = new Character("기현빈", "궁수", 1, 15, 3, 80, 1500);
            Character wizard = new Character("기현빈", "마법사", 1, 13, 4, 90, 1500);

            Player = warrior;

            // 아이템 정보 세팅
            _items[0] = new Item("낡은 검", 0, Item.Parts.WEAPON, 0, 2, 600, "쉽게 볼 수 있는 낡은 검 입니다.");
            _items[1] = new Item("청동 도끼", 1, Item.Parts.WEAPON, 0, 5, 1500, "어디선가 사용됐던 것 같은 도끼입니다.");
            _items[2] = new Item("스파르타의 창", 2, Item.Parts.WEAPON, 0, 7, 3200, "스파르타의 전사들이 사용했다는 전설의 창입니다.");
            //items[3] = new Item("광선검", 3, Item.Parts.WEAPON, 0, 15, 2200, "실명할 정도의 빛으로 내뿜는 강력한 검입니다.");

            _items[4] = new Item("수련자 두건", 4, Item.Parts.HELMET, 0, 5, 1000, "수련에 도움을 주는 두건입니다.");
            _items[5] = new Item("무쇠 투구", 5, Item.Parts.HELMET, 0, 9, 2200, "무쇠로 만들어져 튼튼한 투구입니다.");
            _items[6] = new Item("스파르타의 투구", 6, Item.Parts.HELMET, 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 투구입니다.");
            //items[7] = new Item("낡은 ", 7, Item.Parts.HELMET, 0, 25, 10000, "누군가 입었던 것 같은 쫄쫄이입니다.");

            _items[8] = new Item("수련자 갑옷", 8, Item.Parts.CHESTPLATE, 0, 5, 1000, "수련에 도움을 주는 갑옷입니다.");
            _items[9] = new Item("무쇠 갑옷", 9, Item.Parts.CHESTPLATE, 0, 9, 2200, "무쇠로 만들어져 튼튼한 갑옷입니다.");
            _items[10] = new Item("스파르타의 갑옷", 10, Item.Parts.CHESTPLATE, 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.");
            _items[11] = new Item("빨간 망토", 11, Item.Parts.CHESTPLATE, 0, 35, 30000, "누군가가 둘렀다던 빨간 망토입니다.");

            // 인벤토리 세팅
            Inventory = new List<Item>();

            // 상점 세팅
            Shop = new List<Item>();
            foreach (var item in _items)
            {
                if (item != null)
                    Shop.Add(item);
            }

            // 던전 세팅
            Dungeons = new List<Dungeon>();
            Dungeons.Add(new Dungeon(Player, "쉬운 던전", 5, 1000));
            Dungeons.Add(new Dungeon(Player, "일반 던전", 11, 1700));
            Dungeons.Add(new Dungeon(Player, "어려운 던전", 17, 2500));


            // 자동 로드, 데이터가 없으면 기본 아이템 지급
            if (!LoadData())
            {
                Inventory.Add(_items[0]); _items[0].IsSold = true;
                Inventory.Add(_items[9]); _items[9].IsSold = true;
            }
        }
    }
}
