using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Autopilot
{
    class PlayerInfo
    {
        public string role { get; set; }
        public List<int> health { get; set; }
        public int estimatedTimeTilDeath { get; set; }
        public Dictionary<string, int> buffTimers = new Dictionary<string, int>()
        {
            {"hpBuff", 0},
            {"acBuff", 0 },
            {"symbol", 0 }
        };

        public PlayerInfo()
        {
            role = "none";
            health = new List<int>();
            estimatedTimeTilDeath = 100;
        }

        public int GetLastHit()
        {
            return health[health.Count - 2] - health[health.Count - 1];
        }
    }
}