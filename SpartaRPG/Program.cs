using SpartaRPG.Managers;

internal class Program
{
    private static GameManager gameManager = new GameManager();

    static void Main(string[] args)
    {
        GameManager.Instance.DataManager.GameDataSetting();

        while (true)
        {
            GameManager.Instance.SceneManager.LoadScene();
        }
    }
}

    /*#region 씬 전환

    static void DisplayGameIntro()
    {   
        int input = CheckValidInput(1, 6);
        switch (input)
        {
            case 1:
                _scene.SceneName = Scene.Name.STATUS;
                break;
            case 2:
                _scene.SceneName = Scene.Name.INVENTORY_MAIN;
                break;
            case 3:
                _scene.SceneName = Scene.Name.SHOP_MAIN;
                break;
            case 4:
                _scene.SceneName = Scene.Name.DUNGEON;
                break;
            case 5:
                _scene.SceneName = Scene.Name.RESTPLACE;
                break;
            case 6:
                SaveData(_path);
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        PrintTitle("상태 보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv.{_player.Level} (다음 레벨까지: {_player.Level - _player.Exp})");
        Console.WriteLine($"{_player.Name} ({_player.Job})");

        PrintAtk();
        PrintDef();
        PrintHp();

        Console.WriteLine($"소지금: {_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                _scene.SceneName = Scene.Name.GAME_INTRO;
                break;
        }
    }

    static void DisplayEquipment()
    {
        Console.Clear();

        PrintTitle("인벤토리 - 장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        PrintItemCategories();
        Console.WriteLine();
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i].Part == _category || _category == null)
                _inventory[i].PrintInfoAtInventory(i + 1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, _inventory.Count);
        if (input == 0)
            _scene.SceneName = Scene.Name.INVENTORY_MAIN;
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
                if (_player.Equipments[(int)selectedItem.Part] != null)
                    Unwear(selectedItem.Part);

                Wear(selectedItem);
            }
        }
    }

    static void DisplaySorting()
    {
        Console.Clear();

        PrintTitle("인벤토리 - 아이템 정렬");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        PrintItemCategories();
        Console.WriteLine();
        foreach (var item in _inventory)
        {
            if (item.Part == _category || _category == null)
                item.PrintInfoAtInventory();
        }
        Console.WriteLine();
        Console.WriteLine("1. 이름");
        Console.WriteLine("2. 능력치");
        Console.WriteLine("3. 가격");
        Console.WriteLine("4. 장착");
        Console.WriteLine("5. 강화");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 6);
        switch(input)
        {
            case 0:
                _scene.SceneName = Scene.Name.INVENTORY_MAIN;
                break;
            case 1:
                _inventory = _inventory.OrderBy(item => item.Name).ToList();
                DisplaySorting(); break;
            case 2:
                _inventory = _inventory.OrderByDescending(item => item.Stat).ToList();
                DisplaySorting(); break;
            case 3:
                _inventory = _inventory.OrderBy(item => item.Price).ToList();
                DisplaySorting(); break;
            case 4:
                _inventory = _inventory.OrderByDescending(item => item.IsEquipped).ToList();
                DisplaySorting(); break;
            case 5:
                _inventory = _inventory.OrderByDescending(item => item.Level).ToList();
                DisplaySorting(); break;
        }
    }



    static void DisplayBuyItem()
    {
        Console.Clear();

        PrintTitle("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다");
        Console.WriteLine();
        PrintItemCategories();
        Console.WriteLine();

        PrintGold();

        for (int i = 0; i < _shop.Count; i++)
        {
            _shop[i].PrintInfoAtShop(i+1);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        InputLoopForShop(false);
        _scene.SceneName = Scene.Name.SHOP_MAIN;
    }

    static void DisplaySellItem()
    {
        Console.Clear();

        PrintTitle("상점 - 아이템 판매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        PrintItemCategories();
        Console.WriteLine();

        PrintGold();
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventory[i].PrintInfoAtShop(i + 1, true);
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        InputLoopForShop(true);
        _scene.SceneName = Scene.Name.SHOP_MAIN;
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
            _scene.SceneName = Scene.Name.GAME_INTRO;
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

            if (_player.Level == ++_player.Exp)
            {
                _player.Exp = 0;

                Console.WriteLine($"레벨이 올랐습니다.");
                Console.WriteLine();

                Console.WriteLine($"레벨 {_player.Level} -> {++_player.Level}");
                if (_player.Level % 2 == 1)
                    Console.WriteLine($"공격력 {_player.Atk} -> {++_player.Atk}");
                Console.WriteLine($"방어력 {_player.Def} -> {++_player.Def}");

            }
        }
        else
        {
            PrintTitle("던전 실패");
            Console.WriteLine($"{_dungeons[stage].Name}에서 도망쳤습니다.");
        }
        Console.WriteLine();
        Console.WriteLine("[탐험 결과]");
        Console.WriteLine($"체력 {iHp} -> {_player.CurrentHp}");
        if(dungeonClear) Console.WriteLine($"소지금 {iGold} -> {_player.Gold}");

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
       

        int input = CheckValidInput(0, 0);
        if (input == 0)
        {
            _scene.SceneName = Scene.Name.GAME_INTRO;
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

    static void DisplayRestPlace()
    {
        Console.Clear();

        PrintTitle("휴식하기");
        Console.WriteLine("500 G를 내면 체력을 회복할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[소지금]");
        Console.WriteLine($"{_player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기");

        int input;
        do
        {
            input = CheckValidInput(0, 1);

            if (input == 1)
            {
                if (_player.Gold >= 500)
                {
                    _player.ChangeHP(100);
                    _player.Gold -= 500;
                    Console.WriteLine("휴식을 완료했습니다.");
                    PrintGold();
                }
                else
                {
                    Console.WriteLine("소지금이 부족합니다.");
                }
            }
        } while (input != 0);
        _scene.SceneName = Scene.Name.GAME_INTRO;
    }
}
    */

