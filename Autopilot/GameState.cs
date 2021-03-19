using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using Autopilot;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for Class1
/// </summary>
public class GameState
{
    private static Bitmap gameScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
    private Graphics snapshot = Graphics.FromImage(gameScreen);
    private Dictionary<string, Color> barColors = new Dictionary<string, Color>();
    private Dictionary<string, int> playerBarCoords = new Dictionary<string, int>() {
        // for current 1920x1080 windowed UI settings
        // X coords are labeled, non-labeled are Y coords
        {"groupLeftX", 838},
        {"groupRightX", 954},
        {"groupHealth", 820},
        {"groupMana", 826 },
        {"groupPet", 835  },
        {"verticalOffset", 41},
        {"selfLeftX", 704 },
        {"selfRightX", 803},
        {"selfHealth", 826},
        {"selfPet", 828   },
        {"selfMana", 836  },
        {"selfStam", 846  },
        {"extendedLeftX", 996 },
        {"extendedRightX", 1129 },
        {"extended", 822 } // NOTE: further extended bars use the same vertical offset as the group bars
    };

    private int enchanter;
    public Player[] PartyMembers { get; set; }
    public Player[] ExtendedTargets { get; set; }
    public bool Sitting { get; set; }
    public GameState()
    {
        CaptureScreen();
        PartyMembers = new Player[6];
        SetGameState();
    }


    // INIT METHODS

    private void CaptureScreen()
    {
        snapshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
    }

    private void SetGameState()
    {
        InitBarColors();
        SetOwnState();
        SetGroupState();
        SetExtendedState();
    }

    private void InitBarColors()
    {
        barColors["groupHealthColor"] = gameScreen.GetPixel(playerBarCoords["groupLeftX"], playerBarCoords["groupHealth"]);
        barColors["groupManaColor"] = gameScreen.GetPixel(playerBarCoords["groupLeftX"], playerBarCoords["groupMana"]);
        barColors["groupPetColor"] = gameScreen.GetPixel(playerBarCoords["groupLeftX"], playerBarCoords["groupPet"]);
        barColors["selfHealthColor"] = gameScreen.GetPixel(playerBarCoords["selfLeftX"], playerBarCoords["selfHealth"]);
        barColors["selfManaColor"] = gameScreen.GetPixel(playerBarCoords["selfLeftX"], playerBarCoords["selfMana"]);
        barColors["selfPetColor"] = gameScreen.GetPixel(playerBarCoords["selfLeftX"], playerBarCoords["selfPet"]);
        barColors["extendedHealthColor"] = gameScreen.GetPixel(playerBarCoords["extendedLeftX"], playerBarCoords["extended"]);
    }

    private void SetOwnState()
    {
        Bar healthBar = new Bar(playerBarCoords["selfLeftX"], playerBarCoords["selfRightX"], playerBarCoords["selfHealth"], barColors["selfHealthColor"]);
        Bar manaBar = new Bar(playerBarCoords["selfLeftX"], playerBarCoords["selfRightX"], playerBarCoords["selfMana"], barColors["selfManaColor"]);
        Bar petBar = new Bar(playerBarCoords["selfLeftX"], playerBarCoords["selfRightX"], playerBarCoords["selfPet"], barColors["selfPetColor"]);
        PartyMembers[0] = new Player(healthBar.GetPercent(gameScreen), manaBar.GetPercent(gameScreen), petBar.GetPercent(gameScreen));
        Color sitTextColor = gameScreen.GetPixel(740, 994);
        PartyMembers[0].sitting = sitTextColor.Equals(Color.FromArgb(255, 255, 255));
    }

    private void SetGroupState()
    {
        // TODO: capture extended target bars
        // TODO: fix pet bar capture - currently hardcoding pet health to be extended target #1
        Bar healthBar, manaBar, petBar;
        for (int playerNum = 0; playerNum < 5; playerNum++)
        {
            healthBar = new Bar(playerBarCoords["groupLeftX"], playerBarCoords["groupRightX"], playerBarCoords["groupHealth"] + (playerBarCoords["verticalOffset"] * playerNum), barColors["groupHealthColor"]);
            manaBar = new Bar(playerBarCoords["groupLeftX"], playerBarCoords["groupRightX"], playerBarCoords["groupMana"] + (playerBarCoords["verticalOffset"] * playerNum), barColors["groupManaColor"]);
            petBar = new Bar(playerBarCoords["groupLeftX"], playerBarCoords["groupRightX"], playerBarCoords["groupPet"] + (playerBarCoords["verticalOffset"] * playerNum), barColors["groupPetColor"]);
            PartyMembers[playerNum + 1] = new Player(healthBar.GetPercent(gameScreen), manaBar.GetPercent(gameScreen), petBar.GetPercent(gameScreen));
        }
    }

    private void SetExtendedState()
    {
        // TODO: add functionality for multiple extended target tracking
        Bar extendedBar = new Bar(playerBarCoords["extendedLeftX"], playerBarCoords["extendedRightX"], playerBarCoords["extended"], barColors["extendedHealthColor"]);
        ExtendedTargets = new Player[1];
        ExtendedTargets[0] = new Player(extendedBar.GetPercent(gameScreen), 0, 0);
    }
        
    // TEST AND DEVELOPMENT METHODS
    private void SaveScreen()
    {
        gameScreen.Save("test.jpg", ImageFormat.Jpeg);
    }

    public void testHealthColors()
    {
        string[] colorRange = new string[116];
        //for (int i = playerBarCoords["groupLeftX"]; i < playerBarCoords["groupRightX"] + 1; i++)
        //{
        //    Color healthColor = gameScreen.GetPixel(i, playerBarCoords["groupPet"]);
        //    colorRange[i - playerBarCoords["groupLeftX"]] = "R:" + healthColor.R.ToString() + ",G:" + healthColor.G.ToString() + ",B:" + healthColor.B.ToString();
        //}
        Color textPixel = gameScreen.GetPixel(835, 765);
        Color textPixel2 = gameScreen.GetPixel(930, 729);
        gameScreen.Save("test.jpg", ImageFormat.Jpeg);
        Console.WriteLine(colorRange);
    }

    // DECONSTRUCTOR
    ~GameState() => snapshot.Dispose();
}