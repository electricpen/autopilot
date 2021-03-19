using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;

namespace Autopilot
{
    class Cleric
    {
        private Dictionary<string, int> manaNeededFor = new Dictionary<string, int>()
        {
            {"completeHeal", 25},
            {"groupHeal", 25 },
            {"slowHeal", 10 },
            {"fastHeal", 5 },
            {"hot", 15 },
            {"buffs", 75 }
        };
        private Dictionary<string, int> castTimeFor = new Dictionary<string, int>()
        {
            {"completeHeal", 11},
            {"groupHeal", 5 },
            {"slowHeal", 2 },
            {"fastHeal", 2 },
            {"hot", 5 },
            {"buffs", 15 }
        };
        private InputSimulator simulator = new InputSimulator();
        private AI ai;
        private Player self;
        private PlayerInfo selfInfo;
        private bool casting = false;
        private int castingTarget = -1;
        private System.Windows.Forms.Timer castTimer;
        private int lowHealthCount = 0;
        private Dictionary<string, int> lowestHealth = new Dictionary<string, int>()
        {
            {"Index", -1 },
            {"Health", 100 }
        };
        private Dictionary<string, double> gettingHitNow = new Dictionary<string, double>()
        {
            {"Index", -1 },
            {"DamageTaken", 0 }
        };
        public Cleric(AI ai)
        {
            this.ai = ai;
        }

        public void CheckHealth(Player[] players, Player[] extendedTargets, AI ai)
        {
            self = players[0];
            selfInfo = ai.partyInfo[0];
            CountWounded(players);
            if(!casting)
            {
                if(self.mana > 5)
                {

                    if (players[(ai.roles["Enchanter"])].health < 55) {
                        Heal(ai.roles["Enchanter"], players[(ai.roles["Enchanter"])]);
                    } //else if (lowHealthCount > 2 && self.mana > manaNeededFor["groupHeal"]) {
                       //groupHeal();
                    //}  
                    else if (self.health < 60)
                    {
                        Heal(0, players[0]);
                    } else if (extendedTargets[0].health < 30 && extendedTargets[0].health > 0) {
                        TargetPlayer(ai.roles["Enchanter"] + 6);
                        completeHeal();
                       // Heal(ai.roles["Enchanter"] + 6, players[(ai.roles["Enchanter"])]);
                    }
                    else if (players[(ai.roles["Tank"])].health < 35) // switch to about 33 when first switching to CH
                    {
                        // Heal(ai.roles["Tank"], players[(ai.roles["Tank"])]);
                        TargetPlayer(ai.roles["Tank"]);
                        completeHeal();
                    } else
                    if (lowestHealth["Health"] < 60)
                    {
                        //if(lowestHealth["Index"] != ai.roles["Tank"])
                        //{
                            Heal(lowestHealth["Index"], players[(lowestHealth["Index"])]);
                        //}
                    }
                }
                if (ai.GetRecentDamageTaken(0) < 1)
                {
                    if(self.mana < 99)
                    {
                        Sit();
                    }
                } else if (selfInfo.estimatedTimeTilDeath < 5)
                {
                    divineAura();
                    Sit();
                }
            } else
            {
                if(castingTarget > 5)
                {
                    if(players[castingTarget - 6].petHealth > 60)
                    {
                        stopCasting();
                    }
                } else if(players[castingTarget].health > 60)
                {
                    stopCasting();
                }
            }
        }

        private void CountWounded(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                ai.TallyPartyDamage(i, players[i]);
                if (players[i].IsAlive())
                {
                    lowHealthCount = players[i].health < 60 ? lowHealthCount + 1 : lowHealthCount;
                    if (players[i].health < lowestHealth["Health"] && players[i].health > 0)
                    {
                        if (ai.roles["Tank"] != i)
                        {
                            lowestHealth["Health"] = players[i].health;
                            lowestHealth["Index"] = i;
                        }
                    }
                    int damageTaken = ai.GetRecentDamageTaken(i);
                    if (damageTaken > gettingHitNow["DamageTaken"])
                    {
                        gettingHitNow["Index"] = i;
                        gettingHitNow["DamageTaken"] = damageTaken;
                    }
                }
            }
        }
        private void startCastTimer(int castTime)
        {
            casting = true;
            castTimer = new System.Windows.Forms.Timer();
            castTimer.Tick += new EventHandler(stopCastTimer);
            castTimer.Interval = castTime;
            castTimer.Enabled = true;
        }

        private void stopCastTimer(object source, EventArgs e)
        {
            casting = false;
            castingTarget = -1;
            castTimer.Stop();
        }
        private void TargetPlayer(int position)
        {
            simulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
            switch (position)
            {
                case 0:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                    break;
                case 1:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    break;
                case 2:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    break;
                case 3:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                    break;
                case 4:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                    break;
                case 5:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F6);
                    break;
                case 6:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                    break;
                case 7:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    break;
                case 8:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    break;
                case 9:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                    break;
                case 10:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                    break;
                case 11:
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F6);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.F6);
                    break;
                default:
                    break;
            }
        }

        private void Heal(int targetIndex, Player targetPlayer)
        {
            // TODO: Add role logic to the heal type decision
            PlayerInfo targetInfo = new PlayerInfo();
            targetInfo = targetIndex > 5 ? ai.partyInfo[targetIndex - 6] : ai.partyInfo[targetIndex];
            int targetHealth = targetIndex < 6 ? targetInfo.health[targetInfo.health.Count - 1] : targetPlayer.petHealth;
            TargetPlayer(targetIndex);
            castingTarget = targetIndex;


            if (targetInfo.role != "Shaman")
            {
                if(targetHealth > 15)
                {
                    slowHeal();
                } else
                {
                    fastHeal();
                }
            } else if (targetInfo.role == "Tank")
            {
                if (targetHealth < 15)
                {
                    fastHeal();
                }
                else
                {
                    // slowHeal();
                    completeHeal();
                }
            } else if (targetInfo.role == "Enchanter" && targetIndex > 5)
            {
                slowHeal();
                // completeHeal();
            } else if (targetHealth < 20)
            {
                fastHeal();
            } else
            {
                slowHeal();
            }

            if(lowestHealth["Index"] == targetIndex)
            {
                lowestHealth["Health"] = 100;
            }
        }
        private void stopCasting()
        {
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
            casting = false;
            castingTarget = -1;
        }
        private void fastHeal()
        {
            if (self.mana > manaNeededFor["fastHeal"])
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_1);
                startCastTimer(castTimeFor["fastHeal"]);
            }
        }

        private void slowHeal()
        {
            if (self.mana > manaNeededFor["slowHeal"])
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_2);
                startCastTimer(castTimeFor["slowHeal"]);
            }
        }

        private void completeHeal()
        {
            if (self.mana > manaNeededFor["completeHeal"])
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_3);
                startCastTimer(castTimeFor["completeHeal"]);
            }
        }

        private void groupHeal()
        {
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_5);
            startCastTimer(castTimeFor["groupHeal"]);
        }
        private void divineAura()
        {
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_8);
        }

        public void Follow()
        {
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_2);
        }

        public void Sit()
        {
            if(!self.sitting && !casting)
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_9);
            }
        }
    }
}
