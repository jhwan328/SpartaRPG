namespace SpartaRPG.Classes
{
    internal class Item
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
            switch (Part)
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
            Console.Write('\n');
        }

        public void PrintInfoAtShop(int num = 0, bool sellMode = false)
        {
            PrintInfo(num);
            if (!sellMode)
            {
                if (IsSold) Console.WriteLine("| 구매 완료");
                else Console.WriteLine($"| {Price}G");
            }
            else Console.WriteLine($"| {(int)(Price * 0.85f)}G");
        }
    }
}
