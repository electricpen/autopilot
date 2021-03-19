using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WK.Libraries.HotkeyListenerNS;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;

namespace Autopilot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Timer timer;
        private int refreshInterval = 500;
        private GameState gameState = new GameState();
        private AI ai = new AI();
        private HotkeyListener startListener = new HotkeyListener();
        private InputSimulator simulator = new InputSimulator();
        private bool inCombat = false;
        public MainWindow()
        {
            InitializeComponent();
            InitializeKeybinds();
            
            // Keybinds keybinds = new Keybinds();
        }

        public void InitializeKeybinds()
        {
            Hotkey startCombat = new Hotkey(Keys.Control, Keys.Q);
            startListener.Add(startCombat);
            startListener.HotkeyPressed += ToggleCombatMode;
        }

        private void ToggleCombatMode(object sender, HotkeyEventArgs e)
        {
            if(inCombat)
            {
                ExitCombatLoop();
            } else
            {
                InitializeGameLoop();
            }
            inCombat = !inCombat;
        }

        private void InitializeGameLoop()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Scan); 
            timer.Interval = refreshInterval;
            timer.Enabled = true;
        }

        private void ExitCombatLoop()
        {
            timer.Stop();
            ai.cleric.Follow();
        }

        private void Scan(object source, EventArgs e)
        {
            gameState = new GameState();
            ai.cleric.CheckHealth(gameState.PartyMembers, gameState.ExtendedTargets, ai);
            UpdateForm();
        }
        
        private void UpdateForm()
        {
            player1Health.Value = gameState.PartyMembers[0].health;
            player2Health.Value = gameState.PartyMembers[1].health;
            player3Health.Value = gameState.PartyMembers[2].health;
            player4Health.Value = gameState.PartyMembers[3].health;
            player5Health.Value = gameState.PartyMembers[4].health;
            player6Health.Value = gameState.PartyMembers[5].health;
            player1Mana.Value = gameState.PartyMembers[0].mana;
            player2Mana.Value = gameState.PartyMembers[1].mana;
            player3Mana.Value = gameState.PartyMembers[2].mana;
            player4Mana.Value = gameState.PartyMembers[3].mana;
            player5Mana.Value = gameState.PartyMembers[4].mana;
            player6Mana.Value = gameState.PartyMembers[5].mana;
            player1Pet.Value = gameState.PartyMembers[0].petHealth;
            player2Pet.Value = gameState.PartyMembers[1].petHealth;
            player3Pet.Value = gameState.PartyMembers[2].petHealth;
            player4Pet.Value = gameState.PartyMembers[3].petHealth;
            player5Pet.Value = gameState.PartyMembers[4].petHealth;
            player6Pet.Value = gameState.PartyMembers[5].petHealth;
            player1HealthChange.Text = "TTD: " + ai.partyInfo[0].estimatedTimeTilDeath.ToString() + " sec";
            player2HealthChange.Text = "TTD: " + ai.partyInfo[1].estimatedTimeTilDeath.ToString() + " sec";
            player3HealthChange.Text = "TTD: " + ai.partyInfo[2].estimatedTimeTilDeath.ToString() + " sec";
            player4HealthChange.Text = "TTD: " + ai.partyInfo[3].estimatedTimeTilDeath.ToString() + " sec";
            player5HealthChange.Text = "TTD: " + ai.partyInfo[4].estimatedTimeTilDeath.ToString() + " sec";
            player6HealthChange.Text = "TTD: " + ai.partyInfo[5].estimatedTimeTilDeath.ToString() + " sec";
            player1Casting.Text = gameState.PartyMembers[0].casting ? "CASTING" : "";
        }

        private void SetRole(string role, int position)
        {
            ai.roles[role] = position;
        }
        private void player1Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 0);
        }
        private void player2Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 1);
        }
        private void player3Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 2);
        }
        private void player4Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 3);
        }
        private void player5Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 4);
        }
        private void player6Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] roles = e.AddedItems[0].ToString().Split(' ');
            SetRole(roles[roles.Length - 1], 5);
        }

        private void Popup(string text)
        {
            System.Windows.Forms.MessageBox.Show(text);
        }
    }
}
