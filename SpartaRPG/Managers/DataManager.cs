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

            if (item.Part == Item.Parts.HELMET || item.Part == Item.Parts.BOOTS)
            {
                Player.ChangeHP(item.Stat);
            }
        }

        public void Unwear(Item.Parts part)
        {
            if (part == Item.Parts.HELMET || part == Item.Parts.BOOTS)
            {
                int hp;
                if (Player.CurrentHp <= Player.Equipments[(int)part].Stat)
                    hp = Player.CurrentHp - 1;
                else
                    hp = Player.Equipments[(int)part].Stat;
                
                Player.ChangeHP(-hp);
            }
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
            _items[0] = new Item("나뭇가지", 0, Item.Parts.WEAPON, 0, 1, 300, "금방이라도 부러질 듯한 나뭇가지입니다.");
            _items[1] = new Item("낡은 검", 1, Item.Parts.WEAPON, 0, 4, 1000, "너무 무뎌져 종이조차 자르지 못하는 검입니다.");
            _items[2] = new Item("청동 도끼", 2, Item.Parts.WEAPON, 0, 9, 2200, "청동으로 만들어진 무거운 도끼입니다.");
            _items[3] = new Item("은빛 창", 3, Item.Parts.WEAPON, 0, 16, 4400, "은을 매우 뾰족하게 깎아만든 창입니다.");
            _items[4] = new Item("주궁", 4, Item.Parts.WEAPON, 0, 25, 8800, "여왕 거미 \'마더\'의 거미줄로 만든 활입니다.");
            _items[5] = new Item("곰 장갑", 5, Item.Parts.WEAPON, 0, 36, 17600, "흉폭한 곰 \'비틀즈\'의 날카로운 발톱이 달린 장갑입니다.");
            _items[6] = new Item("지룡의 단검", 6, Item.Parts.WEAPON, 0, 49, 35200, "지룡 \'메테오\'의 어금니를 벼려 만든 단검입니다.");
            _items[7] = new Item("수룡장", 7, Item.Parts.WEAPON, 0, 64, 70400, "수룡 \'네시\'의 두개골로 만든 지팡이입니다.");
            _items[8] = new Item("백월", 8, Item.Parts.WEAPON, 0, 81, 140800, "달빛이 사라졌던 어느 밤, 이 차크람이 세상을 비췄습니다.");
            _items[9] = new Item("초월한 창", 9, Item.Parts.WEAPON, 0, 100, 281600, "이 창을 쥐면 전장을 휘어잡을 용맹함이 생겨납니다.");

            _items[10] = new Item("야구 모자", 10, Item.Parts.HELMET, 0, 3, 500, "머리에 꼭 맞는 모자입니다.");
            _items[11] = new Item("수련자 두건", 11, Item.Parts.HELMET, 0, 9, 1800, "수련에 도움을 주는 두건입니다.");
            _items[12] = new Item("청동 투구", 12, Item.Parts.HELMET, 0, 21, 3900, "청동으로 만든 투구입니다.");
            _items[13] = new Item("강철 투구", 13, Item.Parts.HELMET, 0, 35, 8300, "강철로 만든 투구입니다.");
            _items[14] = new Item("사슴뿔 투구", 14, Item.Parts.HELMET, 0, 50, 16500, "성스러운 사슴 \'데이지\'의 뿔로 장식한 투구입니다.");
            _items[15] = new Item("공작깃 투구", 15, Item.Parts.HELMET, 0, 69, 35200, "총명한 공작새 \'무지개\'의 깃털로 장식한 투구입니다.");
            _items[16] = new Item("지룡의 투구", 16, Item.Parts.HELMET, 0, 96, 74700, "지룡 \'메테오\'의 비늘로 덮은 투구입니다.");
            _items[17] = new Item("수룡의 투구", 17, Item.Parts.HELMET, 0, 137, 166000, "수룡 \'네시\'의 지느러미로 덮은 투구입니다");
            _items[18] = new Item("한이 서린 왕관", 18, Item.Parts.HELMET, 0, 184, 350500, "무게를 견딘다면 주인이 될 수 있습니다.");
            _items[19] = new Item("초월한 투구 ", 19, Item.Parts.HELMET, 0, 250, 723600, "이 투구를 쓰면 쓰러지지 않을 강인함 생겨납니다.");

            _items[20] = new Item("티셔츠", 20, Item.Parts.CHESTPLATE, 0, 1, 500, "얇은 면 티셔츠입니다.");
            _items[21] = new Item("수련자 상의", 21, Item.Parts.CHESTPLATE, 0, 3, 1000, "수련에 도움을 주는 옷입니다.");
            _items[22] = new Item("청동 갑옷", 22, Item.Parts.CHESTPLATE, 0, 7, 3500, "청동으로 만든 갑옷입니다.");
            _items[23] = new Item("강철 갑옷", 23, Item.Parts.CHESTPLATE, 0, 15, 14000, "강철로 만든 갑옷입니다.");
            _items[24] = new Item("악어 갑옷", 24, Item.Parts.CHESTPLATE, 0, 22, 40500, "늪의 군주 \'샤로\'의 가죽으로 만든 갑옷입니다.");
            _items[25] = new Item("곰가죽 갑옷", 25, Item.Parts.CHESTPLATE, 0, 31, 107000, "흉폭한 곰 \'비틀즈\'의 가죽을 받친 갑옷입니다.");
            _items[26] = new Item("지룡의 갑주", 26, Item.Parts.CHESTPLATE, 0, 40, 361000, "지룡 \'메테오\'의 날갯가죽으로 만든 갑옷입니다.");
            _items[27] = new Item("수룡 갑주", 27, Item.Parts.CHESTPLATE, 0, 52, 902000, "수룡 \'네시\'의 비늘로 덮은 갑옷입니다.");
            _items[28] = new Item("적월", 28, Item.Parts.CHESTPLATE, 0, 68, 2136000, "붉게 물든 땅에 외로이 피어있던 몽우리를 서린 갑옷입니다.");
            _items[29] = new Item("빨간 망토", 29, Item.Parts.CHESTPLATE, 0, 80, 7320000, "이 망토만 있다면 갑옷은 필요하지 않을 것 같습니다.");

            _items[30] = new Item("청바지", 30, Item.Parts.LEGGINGS, 0, 3, 1000, "멋을 위해 군데군데 찢긴 청바지입니다.");
            _items[31] = new Item("수련자 하의", 31, Item.Parts.LEGGINGS, 0, 9, 2200, "수련에 도움을 주는 바지입니다.");
            _items[32] = new Item("청동 바지", 32, Item.Parts.LEGGINGS, 0, 15, 7000, "청동으로 만든 바지입니다.");
            _items[33] = new Item("강철 바지", 33, Item.Parts.LEGGINGS, 0, 23, 28000, "강철로 만든 바지입니다.");
            _items[34] = new Item("뱀가죽 바지", 34, Item.Parts.LEGGINGS, 0, 32, 91000, "교활한 뱀 \'스니키\'의 가죽으로 만든 레깅스입니다.");
            _items[35] = new Item("곰가죽 바지", 35, Item.Parts.LEGGINGS, 0, 42, 214000, "흉폭한 곰 \'비틀즈\'의 가죽으로 만든 바지입니다.");
            _items[36] = new Item("지룡의 바지", 36, Item.Parts.LEGGINGS, 0, 55, 722000, "지룡 \'메테오\'의 지느러미로 덮은 바지입니다.");
            _items[37] = new Item("수룡의 문양", 37, Item.Parts.LEGGINGS, 0, 68, 1804000, "수룡 \'네시\'의 표식이 박힌 바지입니다.");
            _items[38] = new Item("흑월", 38, Item.Parts.LEGGINGS, 0, 83, 4272000, "달빛이 사라졌던 어느 밤, 그 그림자에 물들었습니다.");
            _items[39] = new Item("빨간 반바지", 39, Item.Parts.LEGGINGS, 0, 100, 14640000, "반바지에서조차 그의 예절과 겸손이 느껴집니다.");

            _items[40] = new Item("운동화", 40, Item.Parts.BOOTS, 0, 5, 1000, "밑창이 다 닳아버린 운동화입니다.");
            _items[41] = new Item("수련자 단화", 41, Item.Parts.BOOTS, 0, 12, 3600, "수련에 도움을 주는 단화입니다.");
            _items[42] = new Item("청동 부츠", 42, Item.Parts.BOOTS, 0, 36, 7800, "청동으로 만든 부츠입니다.");
            _items[43] = new Item("강철 부츠", 43, Item.Parts.BOOTS, 0, 68, 16600, "강철로 만든 부츠입니다.");
            _items[44] = new Item("늑대 부츠", 44, Item.Parts.BOOTS, 0, 112, 33000, "굶주린 늑대 \'울\'의 발바닥을 밑창에 붙인 부츠입니다.");
            _items[45] = new Item("곰가죽 장화", 45, Item.Parts.BOOTS, 0, 172, 70400, "흉폭한 곰 \'비틀즈\'의 가죽으로 만든 장화입니다.");
            _items[46] = new Item("지룡의 각반", 46, Item.Parts.BOOTS, 0, 255, 149400, "지룡 \'메테오\'의 뿔을 감아 만든 각반입니다.");
            _items[47] = new Item("수룡각", 47, Item.Parts.BOOTS, 0, 360, 332000, "수룡 \'네시\'의 보주로 만든 각반입니다.");
            _items[48] = new Item("청월", 48, Item.Parts.BOOTS, 0, 496, 701000, "달은 물에 잠겨서도 은은한 빛을 만들었습니다.");
            _items[49] = new Item("살구색 양말", 49, Item.Parts.BOOTS, 0, 650, 1447200, "맨발은 위험합니다.");

            // 상점 세팅
            foreach (var item in _items)
            {
                if (item != null)
                    Shop.Add(item);
            }

            // 던전 세팅
            Dungeons.Add(new Dungeon(Player, "쉬운 던전", 5, 1000));
            Dungeons.Add(new Dungeon(Player, "일반 던전", 17, 2500));
            Dungeons.Add(new Dungeon(Player, "어려운 던전", 52, 8000));
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

                Player.Exp += stage;

                if (Player.Level <= Player.Exp)
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
            Inventory.Add(_items[0]); Shop.Remove(_items[0]);
            Inventory.Add(_items[10]); Shop.Remove(_items[10]);
            Inventory.Add(_items[20]); Shop.Remove(_items[20]);
            Inventory.Add(_items[30]); Shop.Remove(_items[30]);
            Inventory.Add(_items[40]); Shop.Remove(_items[40]);
        }
    }
}
