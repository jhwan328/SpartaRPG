namespace SpartaRPG.Classes
{
    internal class Dungeon
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
            Console.Write($"| 방어력 {Condition}이상 권장");
            Console.WriteLine();
        }
    }
}
