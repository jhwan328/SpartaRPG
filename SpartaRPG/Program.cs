using static System.Reflection.Metadata.BlobBuilder;
using System.Net.NetworkInformation;

internal class Program
{
    private static Character player;
    private static List<Item> inventory;
    private static Item[] items = new Item[50];


    static void Main(string[] args)
    {
        GameDataSetting();
        DisplayGameIntro();
    }

    static void GameDataSetting()
    {
        // 캐릭터 정보 세팅
        player = new Character("기현빈", "전사", 1, 10, 5, 100, 1500);

        //WEAPON,
        //HELMET,
        //CHESTPLATE,
        //LEGGINGS,
        //BOOTS,

        // 아이템 정보 세팅
        items[0] = new Item("낡은 검", 0, Item.Parts.WEAPON, 0, 2, 600, "쉽게 볼 수 있는 낡은 검 입니다.");
        items[1] = new Item("청동 도끼", 1, Item.Parts.WEAPON, 0, 5, 1500, "어디선가 사용됐던 것 같은 도끼입니다.");
        items[2] = new Item("스파르타의 창", 2, Item.Parts.WEAPON, 0, 7, 3200, "스파르타의 전사들이 사용했다는 전설의 창입니다.");
        //items[3] = new Item("광선검", 3, Item.Parts.WEAPON, 0, 15, 2200, "실명할 정도의 빛으로 내뿜는 강력한 검입니다.");

        items[4] = new Item("수련자 두건", 4, Item.Parts.HELMET, 0, 5, 1000, "수련에 도움을 주는 두건입니다.");
        items[5] = new Item("무쇠 투구", 5, Item.Parts.HELMET, 0, 9, 2200, "무쇠로 만들어져 튼튼한 투구입니다.");
        items[6] = new Item("스파르타의 투구", 6, Item.Parts.HELMET, 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 투구입니다.");
        //items[7] = new Item("낡은 ", 7, Item.Parts.HELMET, 0, 25, 10000, "누군가 입었던 것 같은 쫄쫄이입니다.");

        items[8] = new Item("수련자 갑옷", 8, Item.Parts.CHESTPLATE, 0, 5, 1000, "수련에 도움을 주는 갑옷입니다.");
        items[9] = new Item("무쇠 갑옷", 9, Item.Parts.CHESTPLATE, 0, 9, 2200, "무쇠로 만들어져 튼튼한 갑옷입니다.");
        items[10] = new Item("스파르타의 갑옷", 10, Item.Parts.CHESTPLATE, 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.");
        items[11] = new Item("빨간 망토", 11, Item.Parts.CHESTPLATE, 0, 35, 30000, "누군가가 둘렀다던 빨간 망토입니다.");

        // 인벤토리 세팅
        inventory = new List<Item>();
        inventory.Add(items[0]);
        inventory.Add(items[9]);
    }

    static void DisplayGameIntro()
    {
        Console.Clear();

        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");

        int input = CheckValidInput(1, 2);
        switch (input)
        {
            case 1:
                DisplayMyInfo();
                break;

            case 2:
                DisplayInventory();
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv.{player.Level}");
        Console.WriteLine($"{player.Name} ({player.Job})");

        Console.Write($"공격력: {player.Atk}");
        // 장비 효과 계산
        if (player.Equipment[(int)Item.Parts.WEAPON] != null)
            Console.Write($" (+{player.Equipment[(int)Item.Parts.WEAPON].Stat})");
        Console.WriteLine();

        Console.Write($"방어력: {player.Def}");
        // 장비 효과 계산
        if (player.Equipment[(int)Item.Parts.CHESTPLATE] != null || player.Equipment[(int)Item.Parts.LEGGINGS] != null)
        {
            int defBonus = 0;
            if (player.Equipment[(int)Item.Parts.CHESTPLATE] != null)
                defBonus += player.Equipment[(int)Item.Parts.CHESTPLATE].Stat;

            if (player.Equipment[(int)Item.Parts.LEGGINGS] != null)
                defBonus += player.Equipment[(int)Item.Parts.LEGGINGS].Stat;

            Console.Write($" (+{defBonus})");
        }
        Console.WriteLine();

        Console.Write($"체력: {player.Hp}");
        // 장비 효과 계산
        if (player.Equipment[(int)Item.Parts.HELMET] != null || player.Equipment[(int)Item.Parts.BOOTS] != null)
        {
            int hpBonus = 0;
            if (player.Equipment[(int)Item.Parts.HELMET] != null)
                hpBonus += player.Equipment[(int)Item.Parts.HELMET].Stat;

            if (player.Equipment[(int)Item.Parts.BOOTS] != null)
                hpBonus += player.Equipment[(int)Item.Parts.BOOTS].Stat;

            Console.Write($" (+{hpBonus})");
        }
        Console.WriteLine();

        Console.WriteLine($"Gold: {player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;
        }
    }

    static void DisplayInventory()
    {
        Console.Clear();

        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        foreach (var item in inventory)
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
                DisplayGameIntro();
                break;
            case 1:
                DisplayEquipment();
                break;
            case 2:
                DisplayInventorySorting();
                break;
        }
    }

    static void DisplayEquipment()
    {
        Console.Clear();

        Console.WriteLine("인벤토리 - 장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintInfoAtInventory(i+1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, inventory.Count);
        if (input == 0)
            DisplayInventory();
        else
        {
            // 선택한 장비
            var selectedItem = inventory[input - 1];
            //가 장착 중이라면
            if (selectedItem.IsEquipped)
            {
                // 장착 해제
                player.Equipment[(int)selectedItem.Part] = null;
                selectedItem.IsEquipped = false;
            }
            //가 장착 중이 아니라면
            else {
                // 선택한 장비가 해당하는 파트에 이미 장착 중인 장비
                var equipmentOfSelectedPart = player.Equipment[(int)selectedItem.Part];
                // 가 있다면
                if (equipmentOfSelectedPart != null)
                {
                    // 장착 중이던 장비를 해제하고
                    player.Equipment[(int)selectedItem.Part] = null;
                    equipmentOfSelectedPart.IsEquipped = false;

                    // 선택한 장비를 해당 파트에 장착
                    player.Equipment[(int)selectedItem.Part] = selectedItem;
                    selectedItem.IsEquipped = true;
                }
                // 가 없다면
                else
                {
                    // 선택한 장비를 해당 파트에 장착
                    player.Equipment[(int)selectedItem.Part] = selectedItem;
                    selectedItem.IsEquipped = true;
                }
            }
            DisplayEquipment();
        }
    }

    static void DisplayInventorySorting()
    {
        Console.Clear();

        Console.WriteLine("인벤토리 - 아이템 정렬");
        Console.WriteLine("인벤토리를 정렬할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintInfoAtInventory(i + 1);
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
                DisplayGameIntro(); break;
            case 1:
                inventory = inventory.OrderBy(item => item.Name).ToList();
                DisplayInventorySorting(); break;
            case 2:
                inventory = inventory.OrderBy(item => item.Part).ToList();
                DisplayInventorySorting(); break;
            case 3:
                inventory = inventory.OrderByDescending(item => item.Stat).ToList();
                DisplayInventorySorting(); break;
            case 4:
                inventory = inventory.OrderBy(item => item.Price).ToList();
                DisplayInventorySorting(); break;
            case 5:
                inventory = inventory.OrderByDescending(item => item.IsEquipped).ToList();
                DisplayInventorySorting(); break;
            case 6:
                inventory = inventory.OrderByDescending(item => item.Level).ToList();
                DisplayInventorySorting(); break;
        }
    }

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
}


public class Character
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; }
    public int Gold { get; }
    public Item[] Equipment { get; set; }

    public Character(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
        Equipment = new Item[System.Enum.GetValues(typeof(Item.Parts)).Length];
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
        
        string firstInfo = $"- {printNum}{equip}{Name}".PadRight(15, ' ');
        string secondInfo = $"| {statByPart} + {Stat}".PadRight(15, ' ');
        string thirdInfo = $"| {Description}".PadRight(15, ' ');

        Console.Write($"{firstInfo}\t{secondInfo}\t{thirdInfo}\t");
    }
    public void PrintInfoAtInventory(int num = 0)
    {
        PrintInfo(num);
        Console.WriteLine();
    }
    public void PrintInfoAtShop()
    {
        PrintInfo();
        if (IsSold)
            Console.WriteLine("| 구매 완료");
        else
            Console.WriteLine($"| {Price}G");
    }
}