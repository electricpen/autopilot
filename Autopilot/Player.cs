using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autopilot
{   
    public class Player
    {
        public int health { get; set; }
        public int mana { get; set; }
        public int petHealth { get; set; }
        public bool sitting { get; set; }
        public bool casting { get; set; }

        public Player(int hp, int mp, int pet)
        {
            health = hp;
            mana = mp;
            petHealth = pet;
        }

        public Player()
        {
            health = 100;
            mana = 100;
            petHealth = 0;
        }

        public bool IsAlive()
        {
            return health > 0;
        }
    }
}