using System.Collections.Generic;


namespace The_Rustwood_Outlaw
{

    public class Level
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public float SpawnRate { get; set; }
        public int BossCount { get; set; }
        public List<string> MapLines { get; set; }
    }
}