using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Autopilot
{
    class AI
    {
        public PlayerInfo[] partyInfo { get; set; }
        public Dictionary<string, int> roles { get; set; }

        private int timeFrame = 12;
        // Keep track of health over time (stretch goal)
        // Modules for different classes (stretch goal)
        // Look at player health and use class and health-based logic

        // Use the AI module for your class here
        public Cleric cleric { get; set; }

        public AI()
        {
            roles = new Dictionary<string, int>() {
                {"Enchanter", 0 },
                {"Tank", 0 },
                {"Healer", 0 },
                {"DPS", 0 },
                {"Squishy", 0 },
                {"Shaman", 0 }
            };
            partyInfo = new PlayerInfo[6];
            for (int i = 0; i < 6; i++)
            {
                partyInfo[i] = new PlayerInfo();
                partyInfo[i].health.Add(100);
                partyInfo[i].health.Add(100);
            }
            cleric = new Cleric(this);
        }

        public void TallyPartyDamage(int position, Player player)
        {
            partyInfo[position].health.Add(player.health);
            if(partyInfo[position].health.Count > timeFrame)
            {
                partyInfo[position].health.RemoveAt(0);
            }
        }

        public int GetRecentDamageTaken(int position)
        {
            int length = partyInfo[position].health.Count;
            int health = partyInfo[position].health[length - 1];
            int total = 0;
            int damageTaken;
            for (int i = 1; i < length; i++)
            {
                damageTaken = partyInfo[position].health[i - 1] - partyInfo[position].health[i];
                total += damageTaken < 1 ? 0 : damageTaken;
            }
            partyInfo[position].estimatedTimeTilDeath = total == 0 ? 100 : (health / total) * (timeFrame);
            return health == 0 ? 0 : total;
        }
    }
}
