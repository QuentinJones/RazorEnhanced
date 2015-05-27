﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace RazorEnhanced
{
    public class BandageHeal
	{
        private static bool m_AutoMode;
        internal static bool AutoMode
        {
            get { return m_AutoMode; }
            set { m_AutoMode = value; }
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.SelectedIndex = Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count - 1));
            if (Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear()));
        }
        internal static string TargetType
        {
            get
            {
                return (string)Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text = value));
            }
        }

        internal static int TargetSerial
        {
            get
            {
                int serial = 0;
                try
                {
                    serial = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealtargetLabel.Text, 16);
                }
                catch
                { }
                return serial;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetLabel.Text = "0x" + value.ToString("X8")));
            }
        }
        internal static bool CustomCheckBox 
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked = value));
            }
        }

        internal static int CustomID
        {
            get
            {
                int ID = 0;
                try
                {
                   ID = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text, 16);
                }
                catch
                { }
                return ID;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text = "0x" + value.ToString("X4")));
            }
        }
        internal static int CustomColor
        {
            get
            {
                int color = 0;
                try
                {
                    color = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text, 16);
                }
                catch
                { }
                return color;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text = "0x" + value.ToString("X4")));
            }
        }
        internal static int CustomDelay
        {
            get
            {
                int delay = 1000;
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text, out delay)));
                return delay;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text = value.ToString()));
            }
        }

        internal static bool DexFormula
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked = value));
            }
        }

        internal static int HpLimit
        {
            get
            {
                int hplimit = 100;
                Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealhpTextBox.Text, out hplimit)));
                return hplimit;
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhpTextBox.Text = value.ToString()));
            }
        }

        internal static bool PoisonBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked = value));
            }
        }
        internal static bool MortalBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked = value));
            }
        }
        internal static bool HiddenBlock
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked = value));
            }
        }

        internal static bool ShowCountdown
        {
            get
            {
                return (bool)Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked));
            }

            set
            {
                Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked = value));
            }
        }

        internal static void LoadSettings()
        {
            bool BandageHealcountdownCheckBox = false;
            string BandageHealtargetComboBox = "Self";
            int BandageHealtargetLabel = 0;
            bool BandageHealcustomCheckBox = false;
            int BandageHealcustomIDTextBox = 0;
            int BandageHealcustomcolorTextBox = 0;
            bool BandageHealdexformulaCheckBox = false;
            int BandageHealdelayTextBox = 0;
            int BandageHealhpTextBox = 0;
            bool BandageHealpoisonCheckBox = false;
            bool BandageHealmortalCheckBox = false;
            bool BandageHealhiddedCheckBox = false;

            RazorEnhanced.Settings.General.AssistantBandageHealLoadAll(out BandageHealcountdownCheckBox, out BandageHealtargetComboBox, out BandageHealtargetLabel, out BandageHealcustomCheckBox, out BandageHealcustomIDTextBox, out BandageHealcustomcolorTextBox, out BandageHealdexformulaCheckBox, out BandageHealdelayTextBox, out BandageHealhpTextBox, out BandageHealpoisonCheckBox, out BandageHealmortalCheckBox, out BandageHealhiddedCheckBox);
           
            Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Self");
            Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Target");

            ShowCountdown = BandageHealcountdownCheckBox;
            HiddenBlock = BandageHealhiddedCheckBox;
            MortalBlock = BandageHealmortalCheckBox;
            PoisonBlock = BandageHealpoisonCheckBox;
            HpLimit = BandageHealhpTextBox;
            CustomDelay = BandageHealdelayTextBox;
            DexFormula = BandageHealdexformulaCheckBox;
            if (DexFormula)
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = false;
            else
                Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = true;

            CustomColor = BandageHealcustomcolorTextBox;
            CustomID = BandageHealcustomIDTextBox;
            CustomCheckBox = BandageHealcustomCheckBox;
            if (CustomCheckBox)
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = true;
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = true;
            }
            else
            {
                Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = false;
                Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = false;
            }

            TargetSerial = BandageHealtargetLabel;
            TargetType = BandageHealtargetComboBox;
            if (TargetType == "Target")
            {
                Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = true;
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = true;
            }
            else
            {
                Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = false;
                Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = false;
            }
        }

        // Core

        internal static int EngineRun()
        {
            if (World.Player.IsGhost)
            {
                Thread.Sleep(2000);
                return -1;
            }

            if ((int)(World.Player.Hits * 100 / (World.Player.HitsMax == 0 ? (ushort)1 : World.Player.HitsMax)) < HpLimit)       // Check HP se bendare o meno.
            {
                if (HiddenBlock)
                {
                    if (!World.Player.Visible)  // Esce se attivo blocco hidded 
                        return 0;
                }

                if (PoisonBlock)
                {
                    if (World.Player.Poisoned) // Esce se attivo blocco poison
                        return 0;
                }

                if (MortalBlock)                // Esce se attivo blocco mortal 
                {
                    if (Player.BuffsExist("Mortal Strike"))
                        return 0;
                }

                int serialbende = FindBandage();
                if (serialbende != 0)        // Cerca le bende
                {
                    Assistant.ClientCommunication.SendToServer(new DoubleClick((Assistant.Serial)serialbende));
                    AddLog("Using bandage!");
                    Target.WaitForTarget(1000);

                    if (BandageHeal.TargetType != "Self")
                    {
                        Target.TargetExecute(TargetSerial);
                        AddLog("Targetting: 0x" + TargetSerial.ToString("X8"));
                    }
                    else
                    {
                        Target.TargetExecute(World.Player.Serial);
                        AddLog("Targetting: 0x" + World.Player.Serial.Value.ToString("X8"));
                    }

                    
                    if (DexFormula)         
                    {
                        double delay = (11 - (Player.Dex - (Player.Dex % 10)) / 20) * 1000;         // Calcolo delay in MS
                        if (ShowCountdown)          // Se deve mostrare il cooldown
                        {
                            int second = 0;

                            var delays = delay.ToString(CultureInfo.InvariantCulture).Split('.');
                            int first = int.Parse(delays[0]);
                            if (delays.Count() > 1)
                                second = int.Parse(delays[1]);

                            while (first > 0)
                            {
                                Player.HeadMessage(10, (first/1000).ToString());
                                AddLog("Delay counting....");
                                first = first -1000;
                                Thread.Sleep(1000);
                            }
                            Thread.Sleep(second + 300);           // Pausa dei decimali rimasti
                        }
                        else
                        {
                            Thread.Sleep((Int32)delay + 300);
                        }
                    }
                    else                // Se ho un delay custom
                    {
                        double delay = CustomDelay;
                        if (ShowCountdown)          // Se deve mostrare il cooldown
                        {

                            double subdelay = delay / 1000;

                            int second = 0;

                            var delays = subdelay.ToString(CultureInfo.InvariantCulture).Split('.');
                            int first = int.Parse(delays[0]);
                            if (delays.Count() > 1)
                                second = int.Parse(delays[1]);

                            while (first > 0)
                            {
                                Player.HeadMessage(10, (first / 1000).ToString());
                                AddLog("Delay counting....");
                                first --;
                                Thread.Sleep(1000);
                            }
                            Thread.Sleep(second+300);           // Pausa dei decimali rimasti
                        }
                        else
                        {
                            Thread.Sleep((Int32)delay + 300);
                        }               
                    }
                }
                else        // Fine bende
                {
                    Player.HeadMessage(10, "Bandage not found");
                    AddLog("Bandage not found");
                    Thread.Sleep(5000);
                }
            }
            return 0;
        }

        internal static int FindBandage()
        {
            if (CustomCheckBox)         // Se cerco bende custom
            {
                foreach (Assistant.Item iteminzaino in Assistant.World.Player.Backpack.Contains)
                {
                    if (iteminzaino.ItemID == CustomID && iteminzaino.Hue == CustomColor)
                    {
                        if (iteminzaino.Amount < 11)
                        {
                            Player.HeadMessage(10, "Warning: Low bandage: " + iteminzaino.Amount + " left");
                            AddLog("Warning: Low bandage: " + iteminzaino.Amount + " left");
                        }
                        return iteminzaino.Serial;
                    }
                }
            }
            else
            {
                foreach (Assistant.Item iteminzaino in Assistant.World.Player.Backpack.Contains)
                {
                    if (iteminzaino.ItemID == 0x0E21)
                    {
                        if (iteminzaino.Amount < 11)
                        {
                            Player.HeadMessage(10, "Warning: Low bandage: " + iteminzaino.Amount + " left");
                            AddLog("Warning: Low bandage: " + iteminzaino.Amount + " left");
                        }
                        return iteminzaino.Serial;
                    }
                }
            }
            return 0;
        }

        internal static void Engine()
        {
            int exit = Int32.MinValue;
            exit = EngineRun();
        }

        // Funzioni da script
        public static void Start()
        {
            if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
                Misc.SendMessage("Script Error: BandageHeal.Start: Bandage Heal already running");
            else
                Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = true));
        }

        public static void Stop()
        {
            if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == false)
                Misc.SendMessage("Script Error: BandageHeal.Stop: Bandage Heal already sleeping");
            else
                Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false));
        }

        public static bool Status()
        {
            return Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked;
        }
	}
}
