using SpartaRPG.Classes;

namespace SpartaRPG.Managers
{
    internal class UIManager
    {   
        public GameManager GameManager;

        private Item.Parts? _category;

        private int _itemsTopPostion = 7;

        private int _categoryTopPostion = 3;

        private int _goldLeftPostion = 91;

        private int _goldTopPostion = 3;

        public UIManager()
        {
            Console.SetWindowSize(120, 30);
        }

        public void ClearLine()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.Write("\r");
        }

        public void PrintTitle(string title, ConsoleColor color = ConsoleColor.DarkRed)
        {
            Console.Clear();

            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{title}]");
            Console.ForegroundColor = currentColor;
        }

        public int PrintAtk(bool print = true)
        {
            var player = GameManager.Instance.DataManager.Player;

            int atk = player.Atk;
            int atkBonus = 0;

            // 장비 효과 계산
            if (player.Equipments[(int)Item.Parts.WEAPON] != null)
            {
                atkBonus = player.Equipments[(int)Item.Parts.WEAPON].Stat;
            }

            if (print)
            {
                Console.Write($"공격력: {atk}");
                if (atkBonus != 0) Console.Write($" (+{atkBonus})");
                Console.WriteLine();
            }


            return atk + atkBonus;
        }

        public int PrintDef(bool print = true)
        {
            var player = GameManager.Instance.DataManager.Player;

            int def = player.Def;
            int defBonus = 0;


            // 장비 효과 계산
            if (player.Equipments[(int)Item.Parts.CHESTPLATE] != null)
                defBonus += player.Equipments[(int)Item.Parts.CHESTPLATE].Stat;

            if (player.Equipments[(int)Item.Parts.LEGGINGS] != null)
                defBonus += player.Equipments[(int)Item.Parts.LEGGINGS].Stat;

            if (print)
            {
                Console.Write($"방어력: {def}");
                if (defBonus != 0) Console.Write($" (+{defBonus})");
                Console.WriteLine();
            }

            return def + defBonus;
        }

        public int PrintHp(bool print = true)
        {
            var player = GameManager.Instance.DataManager.Player;

            int hp = player.MaxHp;
            int hpBonus = 0;

            // 장비 효과 계산
            if (player.Equipments[(int)Item.Parts.HELMET] != null)
                hpBonus += player.Equipments[(int)Item.Parts.HELMET].Stat;

            if (player.Equipments[(int)Item.Parts.BOOTS] != null)
                hpBonus += player.Equipments[(int)Item.Parts.BOOTS].Stat;

            if (print)
            {
                Console.Write($"체력: {player.CurrentHp} / {hp}");
                if (hpBonus != 0) Console.Write($" (+{hpBonus})");
                Console.WriteLine();
            }

            return hp + hpBonus;
        }

        public void PrintItemCategories(bool forRefresh = false)
        {
            var currentCursor = Console.GetCursorPosition();

            Console.SetCursorPosition(0, _categoryTopPostion);

            Console.WriteLine("┌───────┐───────┐───────┐───────┐───────┐───────┐");
            Console.WriteLine("│  전체 │  무기 │  투구 │  갑옷 │  바지 │  신발 │");
            switch (_category)
            {
                case null:
                    Console.Write("┘       └─────────────────────────────────────────────");
                    break;
                case Item.Parts.WEAPON:
                    Console.Write("────────┘       └─────────────────────────────────────");
                    break;
                case Item.Parts.HELMET:
                    Console.Write("────────────────┘       └─────────────────────────────");
                    break;
                case Item.Parts.CHESTPLATE:
                    Console.Write("────────────────────────┘       └─────────────────────");
                    break;
                case Item.Parts.LEGGINGS:
                    Console.Write("────────────────────────────────┘       └─────────────");
                    break;
                case Item.Parts.BOOTS:
                    Console.Write("────────────────────────────────────────┘       └─────");
                    break;
            }
            Console.Write("".PadRight(Console.WindowWidth - 54, '─'));
            Console.Write("\n\n");

            if (forRefresh) Console.SetCursorPosition(currentCursor.Left, currentCursor.Top);
        }

        public void PrintGold()
        {
            var currentCursor = Console.GetCursorPosition();

            Console.SetCursorPosition(_goldLeftPostion, _goldTopPostion);
            Console.Write("┌─────────────────────────┐");
            Console.SetCursorPosition(_goldLeftPostion, _goldTopPostion + 1);
            Console.Write($"│ 소지금│ {GameManager.Instance.DataManager.Player.Gold.ToString().PadLeft(14, ' ')} G│");
            Console.SetCursorPosition(_goldLeftPostion, _goldTopPostion + 2);
            Console.Write("┴─────────────────────────┴");

            Console.SetCursorPosition(currentCursor.Left, currentCursor.Top);
        }

        public void PrintDescription(string description)
        {
            Console.Write($"{description}\n\n");
        }

        public void PrintOption(List<string>? option)
        {
            if (option == null) return;
            foreach(var opt in option) Console.WriteLine(opt.ToString());
        }

        public void PrintShopItems(bool printNum)
        {
            var shop = GameManager.Instance.DataManager.Shop;
            var currentCursor = Console.GetCursorPosition();

            Console.SetCursorPosition(0, _itemsTopPostion);

            for (int i = 0; i < shop.Count; i++)
            {
                var item = shop[i];

                if (item.Part == _category || _category == null)
                {
                    ClearLine();
                    if (printNum) item.PrintInfoAtShop(i + 1);
                    else item.PrintInfoAtShop();
                }
            }

            Console.SetCursorPosition(currentCursor.Left, currentCursor.Top);
        }

        public void PrintInventoryItems(bool printNum)
        {
            var inventory = GameManager.Instance.DataManager.Inventory;
            var currentCursor = Console.GetCursorPosition();

            Console.SetCursorPosition(0, _itemsTopPostion);

            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];

                if(item.Part == _category || _category == null)
                {
                    ClearLine();
                    if (printNum) item.PrintInfoAtInventory(i + 1);
                    else item.PrintInfoAtInventory();
                }
                
            }
            ClearLine();

            Console.SetCursorPosition(currentCursor.Left, currentCursor.Top);
        }

        public void PrintExit(int exitNum = 0)
        {
            Console.Write($"{exitNum}. 나가기");
        }

    }
}
