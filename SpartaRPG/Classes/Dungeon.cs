namespace SpartaRPG.Classes
{
    internal class Dungeon
    {
        public string Name { get; }
        public int Condition { get; }
        public List<int> Reward { get; } // Reward[0]은 골드, Reward[1]부터는 아이템 id


        public Dungeon(Character player, string name, int condition, int gold)
        {
            Reward = new List<int>();

            Name = name;
            Condition = condition;
            Reward.Add(gold);
        }

        public void AddReward(int id)
        {
            Reward.Add(id);
        }
    }
}
