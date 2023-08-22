using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SpartaRPG.Classes;
using static SpartaRPG.Managers.SceneManager;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace SpartaRPG.Managers
{
    internal class DataManager
    {
        public GameManager GameManager;

        public Character Player { get; private set; }
        public List<Item> Inventory { get; private set; }
        public List<Item> Shop { get; private set; }
        public List<Item> SortedItems { get; set; }
        public List<Dungeon> Dungeons { get; private set; }


        private Item[] _items = new Item[50];
        private string? _id;

        private string _savePath = @"../../../Save";

        public DataManager()
        {
            SortedItems = new List<Item>();
            Inventory = new List<Item>();
            Shop = new List<Item>();
            Dungeons = new List<Dungeon>();
        }


        public void SaveData()
        {
            if (Player == null) return;

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

            using (FileStream fs = File.Open(_savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(configData.ToString());
                }
            }
        }

        public void LoadData()
        {
            string jsonContent;

            using (FileStream fs = File.Open(_savePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    jsonContent = reader.ReadToEnd();
                }
            }

            dynamic data = JsonConvert.DeserializeObject(jsonContent);

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
                Inventory.Add(_items[(int)id]);
                Shop.Remove(_items[(int)id]);
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
            Shop.Remove(item);
            GameManager.Instance.UIManager.PrintGold();
            GameManager.Instance.UIManager.PrintItems();
        }

        public void SellItem(Item item)
        {
            Player.Gold += (int)(item.Price * 0.85f);
            Inventory.Remove(item);
            Shop.Add(item);
            GameManager.Instance.UIManager.PrintGold();
            GameManager.Instance.UIManager.PrintItems();
        }

        public void GameDataSetting()
        {
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

            // 상점 세팅
            foreach (var item in _items)
            {
                if (item != null)
                    Shop.Add(item);
            }

            // 던전 세팅
            Dungeons.Add(new Dungeon(Player, "쉬운 던전", 5, 1000));
            Dungeons.Add(new Dungeon(Player, "일반 던전", 11, 1700));
            Dungeons.Add(new Dungeon(Player, "어려운 던전", 17, 2500));
        }

        public void SortItems(List<Item> itemList)
        {
            SortedItems.Clear();

            foreach (Item item in itemList)
            {
                if (item.Part == GameManager.Instance.UIManager.Category
                    || GameManager.Instance.UIManager.Category == null)
                {
                    SortedItems.Add(item);
                }
            }
        }

        public void SortInventory(int num)
        {
            switch (num)
            {
                case 1:
                    Inventory = Inventory.OrderBy(item => item.Name).ToList();
                    break;
                case 2:
                    Inventory = Inventory.OrderByDescending(item => item.Stat).ToList();
                    break;
                case 3:
                    Inventory = Inventory.OrderBy(item => item.Price).ToList();
                    break;
                case 4:
                    Inventory = Inventory.OrderByDescending(item => item.IsEquipped).ToList();
                    break;
                case 5:
                    Inventory = Inventory.OrderByDescending(item => item.Level).ToList();
                    break;
            }
        }

        public int GetAtkBonus(bool print = true)
        {
            int atkBonus = 0;

            if (Player.Equipments[(int)Item.Parts.WEAPON] != null)
            {
                atkBonus = Player.Equipments[(int)Item.Parts.WEAPON].Stat;
            }

            return atkBonus;
        }

        public int GetDefBonus(bool print = true)
        {
            int defBonus = 0;

            if (Player.Equipments[(int)Item.Parts.CHESTPLATE] != null)
                defBonus += Player.Equipments[(int)Item.Parts.CHESTPLATE].Stat;

            if (Player.Equipments[(int)Item.Parts.LEGGINGS] != null)
                defBonus += Player.Equipments[(int)Item.Parts.LEGGINGS].Stat;

            return defBonus;
        }

        public int GetHpBonus(bool print = true)
        {
            int hpBonus = 0;

            if (Player.Equipments[(int)Item.Parts.HELMET] != null)
                hpBonus += Player.Equipments[(int)Item.Parts.HELMET].Stat;

            if (Player.Equipments[(int)Item.Parts.BOOTS] != null)
                hpBonus += Player.Equipments[(int)Item.Parts.BOOTS].Stat;

            return hpBonus;
        }

        public void ExploreDungeon(int stage)
        {
            Random rnd = new Random();
            Dungeon dungeon = Dungeons[stage - 1];
            UIManager ui = GameManager.Instance.UIManager;
            bool clear = false;

            var iHp = Player.CurrentHp;
            var iGold = Player.Gold;

            if (Player.Def + GetDefBonus() < dungeon.Condition && rnd.Next(0, 100) < 40)
            {
                clear = false;

                int damage = rnd.Next(20, 36) - (Player.Def + GetDefBonus() - dungeon.Condition);
                if (damage < 0) damage = 0;
                Player.ChangeHP(-(int)(damage / 2));
            }
            else
            {
                clear = true;
                int damage = rnd.Next(20, 36) - (Player.Def + GetDefBonus() - dungeon.Condition);
                if (damage < 0) damage = 0;
                Player.ChangeHP(-damage);

                int rewardGold = (int)(dungeon.Reward[0]
                    * (100 + rnd.Next(Player.Atk + GetAtkBonus(), 2 * Player.Atk + GetAtkBonus() + 1)) / 100);
                Player.Gold += rewardGold;
            }

            ui.ClearLog();
            if (clear) ui.AddLog($"{dungeon.Name} 클리어");
            else ui.AddLog($"{dungeon.Name} 도전 실패");

            ui.AddLog($"체력 {iHp} -> {Player.CurrentHp}");
            if (clear)
            {
                ui.AddLog($"소지금 {iGold} -> {Player.Gold}");
                ui.AddLog("");

                if (Player.Level <= ++Player.Exp)
                {
                    Player.Exp -= Player.Level;

                    ui.AddLog("레벨이 올랐습니다.");
                    ui.AddLog($"레벨 {Player.Level} -> {++Player.Level}");
                    if (Player.Level % 2 == 1)
                        ui.AddLog($"공격력 {Player.Atk} -> {++Player.Atk}");
                    ui.AddLog($"방어력 {Player.Def} -> {++Player.Def}");
                }
            }
        }

        public void RestPlayer()
        {
            UIManager ui = GameManager.Instance.UIManager;

            if(Player.Gold >= 500)
            {
                ui.AddLog(".");
                Thread.Sleep(500);
                ui.AddLog(".");
                Thread.Sleep(500);
                ui.AddLog(".");

                Player.Gold -= 500;
                Player.ChangeHP(100);

                ui.AddLog("휴식을 완료했습니다.");
            }
            else
            {
                ui.AddLog("소지금이 부족합니다.");
            }

        } 

        public void CreateId()
        {
            UIManager ui = GameManager.Instance.UIManager;

            while(true)
            {
                ui.SetCursorPositionForOption();
                _id = Console.ReadLine();
                if (_id != null)
                {
                    string savePath = Path.Combine(_savePath, $"{_id}.json");

                    if (File.Exists(savePath))
                    {
                        ui.AddLog("존재하는 ID입니다.");
                        ui.AddLog("ID 생성에 실패했습니다.");
                        return;
                    }
                    else
                    {
                        _savePath = savePath;
                        ui.AddLog("ID 생성에 성공했습니다.");
                        ui.AddLog("닉네임을 입력하세요.");

                        while (true)
                        {
                            ui.SetCursorPositionForOption();
                            string? name = Console.ReadLine();
                            if (name != null)
                            {
                                Player = new Character(name, "전사", 1, 10, 5, 100, 1500);
                                GetBasicItem();
                                SaveData();
                                GameManager.Instance.SceneManager.Scene = Scenes.TOWN;
                                return;
                            }
                            ui.AddLog("잘못된 입력입니다.");
                        }
                    }
                }
                ui.AddLog("잘못된 입력입니다.");
            }
        }

        public void LoginId()
        {
            UIManager ui = GameManager.Instance.UIManager;

            while (true)
            {
                ui.SetCursorPositionForOption();
                _id = Console.ReadLine();
                if (_id != null)
                {
                    string savePath = Path.Combine(_savePath, $"{_id}.json");

                    if (File.Exists(savePath))
                    {
                        _savePath = savePath;
                        GameManager.Instance.SceneManager.Scene = Scenes.TOWN;
                        LoadData();
                        ui.AddLog("로그인에 성공했습니다");
                        return;
                    }
                    else
                    {
                        ui.AddLog("존재하지 않는 ID입니다.");
                        ui.AddLog("로그인에 실패했습니다");
                        return;
                    }
                }
                ui.AddLog("잘못된 입력입니다.");
            }
        }

        private void GetBasicItem()
        {
            Inventory.Add(_items[0]);
            Shop.Remove(_items[0]);
            Inventory.Add(_items[9]);
            Shop.Remove(_items[9]);
        }
    }
}
