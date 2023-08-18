internal class Program
{
    private static Character _player;
    private static List<Item> _inventory;
    private static List<Item> _shop;
    private static List<Dungeon> _dungeons;
    private static Item[] _items = new Item[50];

    static void Main(string[] args)
    {
        GameDataSetting();
        while(_player.CurrentHp > 0) { DisplayGameIntro(); }
        DisplayGameOuttro();
    }

    static void GameDataSetting()
    {
        Console.SetWindowSize(120, 30);
        // 캐릭터 정보 세팅
        _player = new Character("기현빈", "전사", 1, 10, 5, 100, 1500);

        //WEAPON,
        //HELMET,
        //CHESTPLATE,
        //LEGGINGS,
        //BOOTS,

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
        _inventory = new List<Item>();
        _inventory.Add(_items[0]); _items[0].IsSold = true;
        _inventory.Add(_items[9]); _items[9].IsSold = true;
        _inventory.Add(_items[10]); _items[10].IsSold = true;

        // 상점 세팅
        _shop = new List<Item>();
        foreach (var item in _items)
        {
            if (item != null)
                _shop.Add(item);
        }

        // 던전 세팅
        _dungeons = new List<Dungeon>();
        _dungeons.Add(new Dungeon(_player, "쉬운 던전", 5, 1000));
        _dungeons.Add(new Dungeon(_player, "일반 던전", 11, 1700));
        _dungeons.Add(new Dungeon(_player, "어려운 던전", 17, 2500));

    }

    #region 씬 전환

    static void DisplayGameIntro()
    {
        Console.Clear();

        PrintTitle("마을");
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전 입장");

        int input = CheckValidInput(1, 4);
        switch (input)
        {
            case 1:
                DisplayMyInfo();
                break;
            case 2:
                DisplayInventory();
                break;
            case 3:
                DisplayShop();
                break;
            case 4:
                DisplayDungeonIntro();
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        PrintTitle("상태 보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv.{_player.Level}");
        Console.WriteLine($"{_player.Name} ({_player.Job})");

        PrintAtk();
        PrintDef();
        PrintHp();

        Console.WriteLine($"Gold: {_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                // DisplayGameIntro();
                break;
        }
    }

    static void DisplayInventory()
    {
        Console.Clear();

        PrintTitle("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _inventory)
        {
            item.PrintInfoAtInventory();
        }
        Console.WriteLine();
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("2. 아이템 정렬");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 2);
        switch (input)
        {
            case 0:
                // DisplayGameIntro();
                break;
            case 1:
                DisplayEquipment();
                break;
            case 2:
                DisplaySorting();
                break;
        }
    }

    static void DisplayEquipment()
    {
        Console.Clear();

        PrintTitle("인벤토리 - 장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventory[i].PrintInfoAtInventory(i+1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, _inventory.Count);
        if (input == 0)
            DisplayInventory();
        else
        {
            // 선택한 장비
            var selectedItem = _inventory[input - 1];
            //가 장착 중이라면
            if (selectedItem.IsEquipped)
            {
                Unwear(selectedItem.Part);
            }
            //가 장착 중이 아니라면
            else {
                // 해당 파트에 이미 착용 중인 장비가 있다면
                if (_player.Equipment[(int)selectedItem.Part] != null)
                    Unwear(selectedItem.Part);

                Wear(selectedItem);
            }
            DisplayEquipment();
        }
    }

    static void DisplaySorting()
    {
        Console.Clear();

        PrintTitle("인벤토리 - 아이템 정렬");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventory[i].PrintInfoAtInventory(i + 1);
        }
        Console.WriteLine();
        Console.WriteLine("1. 이름");
        Console.WriteLine("2. 부위");
        Console.WriteLine("3. 능력치");
        Console.WriteLine("4. 가격");
        Console.WriteLine("5. 장착");
        Console.WriteLine("6. 강화");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 6);
        switch(input)
        {
            case 0:
                // DisplayGameIntro();
                break;
            case 1:
                _inventory = _inventory.OrderBy(item => item.Name).ToList();
                DisplaySorting(); break;
            case 2:
                _inventory = _inventory.OrderBy(item => item.Part).ToList();
                DisplaySorting(); break;
            case 3:
                _inventory = _inventory.OrderByDescending(item => item.Stat).ToList();
                DisplaySorting(); break;
            case 4:
                _inventory = _inventory.OrderBy(item => item.Price).ToList();
                DisplaySorting(); break;
            case 5:
                _inventory = _inventory.OrderByDescending(item => item.IsEquipped).ToList();
                DisplaySorting(); break;
            case 6:
                _inventory = _inventory.OrderByDescending(item => item.Level).ToList();
                DisplaySorting(); break;
        }
    }

    static void DisplayShop()
    {
        Console.Clear();

        PrintTitle("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        foreach (var item in _shop)
        {
            item.PrintInfoAtShop();
        }
        Console.WriteLine();
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 2);
        switch (input)
        {
            case 0:
                // DisplayGameIntro();
                break;
            case 1:
                DisplayBuyItem();
                break;
            case 2:
                DisplaySellItem();
                break;
        }
    }

    static void DisplayBuyItem()
    {
        Console.Clear();

        PrintTitle("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < _shop.Count; i++)
        {
            _shop[i].PrintInfoAtShop(i+1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        InputLoopForShop(0, _shop.Count);
        DisplayShop();
    }

    static void DisplaySellItem()
    {
        Console.Clear();

        PrintTitle("상점 - 아이템 판매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventory[i].PrintInfoAtShop(i + 1, true);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        InputLoopForShop(0, _shop.Count, true);
        DisplayShop();
    }

    static void DisplayDungeonIntro()
    {
        Console.Clear();

        PrintTitle("던전 입장");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[플레이어 능력치]");
        PrintAtk();
        PrintDef();
        PrintHp();
        Console.WriteLine();
        for (int i = 0; i < _dungeons.Count; i++)
        {
            _dungeons[i].PrintInfo(i + 1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, _dungeons.Count);
        if (input == 0)
        {
            //DisplayGameIntro();
        }
        else
        {
            DisplayDungeonOuttro(input - 1);
        }
    }

    static void DisplayDungeonOuttro(int stage)
    {
        Console.Clear();

        var iHp = _player.CurrentHp;
        var iGold = _player.Gold;

        bool dungeonClear = _dungeons[stage].ExploreDungeon(PrintAtk(false), PrintDef(false));

        if (dungeonClear)
        {
            PrintTitle("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{_dungeons[stage].Name}을 클리어 하였습니다.");
        }
        else
        {
            PrintTitle("던전 실패");
            Console.WriteLine($"{_dungeons[stage].Name}에서 도망쳤습니다.");
        }
        Console.WriteLine();
        Console.WriteLine("[탐험 결과]");
        Console.WriteLine($"체력 {iHp} -> {_player.CurrentHp}");
        if(dungeonClear) Console.WriteLine($"Gold {iGold} -> {_player.Gold}");

        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        if (input == 0)
        {
            //DisplayGameIntro();
        }
    }

    static void DisplayGameOuttro()
    {
        Console.Clear();

        PrintTitle("게임 오버");
        Console.WriteLine("사망했습니다.");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        CheckValidInput(0, 0);
    }

    #endregion

    #region 편의성 함수
    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void InputLoopForShop(int exit, int max, bool sellMode = false)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret == exit) break;
                else if (ret > exit && ret <= max)
                {
                    // (구매 모드)
                    if (!sellMode)
                    {
                        // 선택한 장비
                        var selectedItem = _shop[ret - 1];
                        //가 이미 구매됐다면
                        if (selectedItem.IsSold)
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        //가 구매 가능하다면
                        else
                        {
                            // 돈이 충분하다면
                            if (_player.Gold >= selectedItem.Price)
                            {
                                Console.WriteLine("구매를 완료했습니다.");
                                BuyItem(selectedItem);
                            }
                            // 돈이 충분치 않다면
                            else
                                Console.WriteLine("Gold가 부족합니다.");
                        }
                    }
                    // (판매 모드)
                    else
                    {
                        // 선택한 장비
                        var selectedItem = _inventory[ret - 1];

                        // 가 장착 중이라면
                        if (selectedItem.IsEquipped)
                        {
                            Console.WriteLine("장착 중인 장비는 판매할 수 없습니다.");
                            Console.WriteLine("해제하고 판매하시겠습니까? (예: 0 / 아니오: 1)");
                            if (CheckValidInput(0, 1) == 0)
                            {
                                Unwear(selectedItem.Part);
                                Console.WriteLine("판매를 완료했습니다.");
                                SellItem(selectedItem);
                            }
                        }
                        // 가 장착 중이 아니라면
                        else
                        {
                            Console.WriteLine("판매를 완료했습니다.");
                            SellItem(selectedItem);
                        }
                    }
                }
                else Console.WriteLine("잘못된 입력입니다.");
            }
            else Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void Wear(Item item)
    {
        _player.Equipment[(int)item.Part] = item;
        item.IsEquipped = true;
    }

    static void Unwear(Item.Parts part)
    {
        _player.Equipment[(int)part].IsEquipped = false;
        _player.Equipment[(int)part] = null;
    }

    static void BuyItem(Item item)
    {
        _player.Gold -= item.Price;
        _inventory.Add(item);
        item.IsSold = true;
        RefreshGoldAndList(false);
    }

    static void SellItem(Item item)
    {
        _player.Gold += (int)(item.Price * 0.85f);
        _inventory.Remove(item);
        item.IsSold = false;
        RefreshGoldAndList(true);
    }

    static void RefreshGoldAndList(bool sellMode)
    {
        var currentCursor = Console.GetCursorPosition();
        Console.SetCursorPosition(0, 4);
        Console.Write("                                   \r");
        Console.Write($"{_player.Gold} G");
        Console.SetCursorPosition(0, 7);

        if(!sellMode)
        {
            for (int i = 0; i < _shop.Count; i++)
            {
                ClearLine();
                _shop[i].PrintInfoAtShop(i + 1, sellMode);
            }
        }
        else
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                ClearLine();
                _inventory[i].PrintInfoAtShop(i + 1, sellMode);
            }
            ClearLine();
        }

        Console.SetCursorPosition(currentCursor.Left, currentCursor.Top);
    }

    static void ClearLine()
    {
        for(int i = 0; i < Console.WindowWidth; i++)
        {
            Console.Write(" ");
        }
        Console.Write("\r");
    }

    static void PrintTitle(string title, ConsoleColor color = ConsoleColor.DarkRed)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"[{title}]");
        Console.ForegroundColor = currentColor;
    }

    static int PrintAtk(bool print = true)
    {
        int atk = _player.Atk;
        int atkBonus = 0;

        // 장비 효과 계산
        if (_player.Equipment[(int)Item.Parts.WEAPON] != null)
        {
            atkBonus = _player.Equipment[(int)Item.Parts.WEAPON].Stat;
        }

        if(print)
        {
            Console.Write($"공격력: {atk}");
            if (atkBonus != 0) Console.Write($" (+{atkBonus})");
            Console.WriteLine();
        }
        

        return atk + atkBonus;
    }

    static int PrintDef(bool print = true)
    {
        int def = _player.Def;
        int defBonus = 0;


        // 장비 효과 계산
        if (_player.Equipment[(int)Item.Parts.CHESTPLATE] != null)
            defBonus += _player.Equipment[(int)Item.Parts.CHESTPLATE].Stat;

        if (_player.Equipment[(int)Item.Parts.LEGGINGS] != null)
            defBonus += _player.Equipment[(int)Item.Parts.LEGGINGS].Stat;

        if(print)
        {
            Console.Write($"방어력: {def}");
            if (defBonus != 0) Console.WriteLine($" (+{defBonus})");
            Console.WriteLine();
        }
        
        return def + defBonus;
    }

    static int PrintHp(bool print = true)
    {
        int hp = _player.MaxHp;
        int hpBonus = 0;

        // 장비 효과 계산
        if (_player.Equipment[(int)Item.Parts.HELMET] != null)
            hpBonus += _player.Equipment[(int)Item.Parts.HELMET].Stat;

        if (_player.Equipment[(int)Item.Parts.BOOTS] != null)
            hpBonus += _player.Equipment[(int)Item.Parts.BOOTS].Stat;

        if(print)
        {
            Console.Write($"체력: {_player.CurrentHp} / {hp}");
            if (hpBonus != 0) Console.WriteLine($" (+{hpBonus})");
            Console.WriteLine();
        }

        return hp + hpBonus;
    }

    #endregion
}
public class Character
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int MaxHp { get; }
    public int CurrentHp { get; set; }
    public int Gold { get; set; }
    public Item[]? Equipment { get; set; }

    public Character(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = hp;
        CurrentHp = hp;
        Gold = gold;
        Equipment = new Item[System.Enum.GetValues(typeof(Item.Parts)).Length];
    }

    public void ChangeHP(int hp)
    {
        var totalHp = MaxHp;

        var helmet = Equipment[(int)(Item.Parts.HELMET)];
        var boots = Equipment[(int)(Item.Parts.BOOTS)];

        if (helmet != null)
            totalHp += helmet.Stat;
        if (boots != null)
            totalHp += boots.Stat;

        CurrentHp += hp;

        if(totalHp < CurrentHp)
        {
            CurrentHp = totalHp;
        }

        if(CurrentHp < 0)
        {
            CurrentHp = 0;
        }
    }
}
public class Item
{
    public enum Parts
    {
        WEAPON,
        HELMET,
        CHESTPLATE,
        LEGGINGS,
        BOOTS
    }
    public string Name { get; }
    public int Id { get; }
    public Parts Part { get; }
    public int Stat { get; }
    public string Description { get; }
    public int Price { get; }
    public int Level { get; set; }
    public bool IsEquipped { get; set; }
    public bool IsSold { get; set; }

    public Item(string name, int id, Parts part, int level, int stat, int price, string description, bool isEquipped = false, bool isSold = false)
    {
        Name = name;
        Id = id;
        Part = part;
        Stat = stat;
        Description = description;
        Price = price;
        Level = level;
        IsEquipped = isEquipped;
        IsSold = isSold;
    }

    public void PrintInfo(int num = 0)
    {
        string equip = (IsEquipped) ? "[E]" : "";
        string printNum = (num == 0) ? "" : $"{num} ";

        string statByPart = "";
        switch(Part)
        {
            case Parts.WEAPON:
                statByPart = "공격력";
                break;
            case Parts.HELMET:
                statByPart = "체력";
                break;
            case Parts.CHESTPLATE:
                statByPart = "방어력";
                break;
            case Parts.LEGGINGS:
                statByPart = "방어력";
                break;
            case Parts.BOOTS:
                statByPart = "체력";
                break;
        }

        Console.Write($"- {printNum}{equip}{Name}");
        Console.SetCursorPosition(25, Console.GetCursorPosition().Top);
        Console.Write($"| {statByPart} + {Stat}");
        Console.SetCursorPosition(40, Console.GetCursorPosition().Top);
        Console.Write($"| {Description}");
        Console.SetCursorPosition(100, Console.GetCursorPosition().Top);
    }
    public void PrintInfoAtInventory(int num = 0)
    {
        PrintInfo(num);
        Console.WriteLine();
    }
    public void PrintInfoAtShop(int num = 0, bool sellMode = false)
    {
        PrintInfo(num);
        if (!sellMode)
        {
            if (IsSold)
                Console.WriteLine("| 구매 완료");
            else
                Console.WriteLine($"| {Price}G");
        }
        else
            Console.WriteLine($"| {(int)(Price * 0.85f)}G");
    }
}
public class Dungeon
{
    public string Name { get; }
    public int Condition { get; }
    public List<int> Reward { get; } // Reward[0]은 골드, Reward[1]부터는 아이템 id

    Action<int> PlayerHPHandler;
    Action<int> PlayerGoldHandler;

    public Dungeon(Character player, string name, int condition, int gold)
    {
        PlayerHPHandler = player.ChangeHP;
        PlayerGoldHandler = (int rewardGold) => { player.Gold += rewardGold; };
        Reward = new List<int>();

        Name = name;
        Condition = condition;
        Reward.Add(gold);
    }

    public void AddReward(int id)
    {
        Reward.Add(id);
    }

    public bool ExploreDungeon(int atk, int def)
    {
        Random rnd = new Random();

        int damage = rnd.Next(20, 35 + 1);
        damage -= def - Condition;

        if (damage < 0) damage = 0;

        if (def < Condition && rnd.Next(0, 100) < 40)
        {
            // 던전 실패
            PlayerHPHandler(-(int)(damage * 0.5f));

            return false;
        }
        else
        {
            // 던전 클리어
            int rewardGold = (int)(Reward[0] * rnd.Next(100 + atk, 100 + atk * 2 + 1) * 0.01f); // reward의 1.x배

            PlayerHPHandler(-damage);
            PlayerGoldHandler(rewardGold);

            return true;
        }
    }

    public void PrintInfo(int num = 0)
    {
        string printNum = (num == 0) ? "" : $"{num}. ";
        Console.Write($"{printNum}{Name}");
        Console.SetCursorPosition(25, Console.GetCursorPosition().Top);
        Console.Write($"| 방어력 {Condition} 이상 권장");
        Console.WriteLine();
    }
}