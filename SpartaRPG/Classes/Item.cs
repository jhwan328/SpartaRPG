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

        public Item(string name, int id, Parts part, int level, int stat, int price, string description, bool isEquipped = false)
        {
            Name = name;
            Id = id;
            Part = part;
            Stat = stat;
            Description = description;
            Price = price;
            Level = level;
            IsEquipped = isEquipped;
        }

        public void PrintInfo(bool showPrice, int num = 0, float sale = 1)
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

            if(showPrice) Console.WriteLine($"| {((int)(Price * sale)).ToString().PadLeft(8, ' ')} G");
            else Console.Write($"| {Description}\n");
        }
    }
}
