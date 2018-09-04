﻿using Assistant;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;

namespace RazorEnhanced
{
	public class Misc
	{
		// Bool per blocco packet in attesa di menu vecchi e vecchio gump response
		internal static bool BlockMenu = false;
		internal static bool BlockGump = false;

		//General
		public static void Pause(int mseconds)
		{
			System.Threading.Thread.Sleep(mseconds);
		}

		public static void Resync()
		{
			Assistant.ClientCommunication.SendToServer(new ResyncReq());
		}

		public static double DistanceSqrt(Point3D a, Point3D b)
		{
			double distance = Math.Sqrt(((a.X - b.X) ^ 2) + (a.Y - b.Y) ^ 2);
			return distance;
		}

		// Sysmessage
		public static void SendMessage(int num)
		{
			SendMessage(num.ToString(), true);
		}
		
		public static void SendMessage(object obj)
		{
			SendMessage(obj.ToString(), true);
		}

		public static void SendMessage(uint num)
		{
			SendMessage(num.ToString(), true);
		}

		public static void SendMessage(bool msg)
		{
			SendMessage(msg.ToString(), true);
		}

		public static void SendMessage(double msg)
		{
			SendMessage(msg.ToString(), true);
		}


		public static void SendMessage(int num, int color)
		{
			SendMessage(num.ToString(), color, true);
		}

		public static void SendMessage(object obj, int color)
		{
			SendMessage(obj.ToString(), color, true);
		}

		public static void SendMessage(uint num, int color)
		{
			SendMessage(num.ToString(), color, true);
		}

		public static void SendMessage(bool msg, int color)
		{
			SendMessage(msg.ToString(), color, true);
		}

		public static void SendMessage(double msg, int color)
		{
			SendMessage(msg.ToString(), color, true);
		}

		public static void SendMessage(string msg, bool wait = true)
		{
			SendMessage(msg, 945, wait);
		}

		internal static void SendMessage(string msg, int color, bool wait) //Main function of sendmessage
		{
			if (Assistant.World.Player != null)
			{
				if (wait)
					ClientCommunication.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
				else
					ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
			}
		}

		public static void Beep()
		{
			SystemSounds.Beep.Play();
		}

		// Login and logout
		public static void Disconnect()
		{
			Assistant.ClientCommunication.SendToClient(new Disconnect());
		}

		// Context Menu
		public static void WaitForContext(Mobile mob, int delay) // Delay in MS
		{
			WaitForContext(mob.Serial, delay);
		}

		public static void WaitForContext(Item i, int delay) // Delay in MS
		{
			WaitForContext(i.Serial, delay);
		}

		public static void WaitForContext(int ser, int delay) // Delay in MS
		{
			ClientCommunication.SendToServerWait(new ContextMenuRequest(ser));
			int subdelay = delay;
			while (World.Player.HasContext != true && World.Player.ContextID != ser && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
		}


		public static void ContextReply(int serial, int idx)
		{
			ClientCommunication.SendToServerWait(new ContextMenuResponse(serial, (ushort)idx));
			World.Player.HasContext = false;
			World.Player.ContextID = 0;
		}

		public static void ContextReply(Mobile mob, int idx)
		{
			ContextReply(mob.Serial, idx);
		}

		public static void ContextReply(Item item, int idx)
		{
			ContextReply(item.Serial, idx);
		}

		public static void ContextReply(Mobile mob, string menuname)
		{
			ContextReply(mob.Serial, menuname);
		}

		public static void ContextReply(Item item, string menuname)
		{
			ContextReply(item.Serial, menuname);
		}

		public static void ContextReply(int serial, string menuname)
		{
			int idx = -1;
			UOEntity e = World.FindItem(serial);
			if (e == null)
				e = World.FindMobile(serial);

			if (e != null)
			{
				foreach (KeyValuePair<ushort, int> menu in e.ContextMenu)
				{
					if (Language.GetCliloc(menu.Value).ToLower() == menuname.ToLower())
					{
						idx = menu.Key;
						break;
					}
				}
				if (idx >= 0)
				{
					ClientCommunication.SendToServerWait(new ContextMenuResponse(serial, (ushort)idx));
					World.Player.HasContext = false;
					World.Player.ContextID = 0;
				}
				else
					Scripts.SendMessageScriptError("Script Error: ContextReply: Menu entry " + menuname + " not exist");
			}
			else
				Scripts.SendMessageScriptError("Script Error: ContextReply: Mobile or item not exit");

		}

		// Prompt Message Stuff
		public static void ResetPrompt()
		{
			World.Player.HasPrompt = false;
		}

		public static bool HasPrompt()
		{
			return World.Player.HasPrompt;
		}

		public static void WaitForPrompt(int delay) // Delay in MS
		{
			int subdelay = delay;
			while (!World.Player.HasPrompt && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
		}

		public static void CancelPrompt()
		{
			ClientCommunication.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0, Language.CliLocName, String.Empty));
			World.Player.HasPrompt = false;
		}

		public static void ResponsePrompt(string text)
		{
			ClientCommunication.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1, Language.CliLocName, text));
			World.Player.HasPrompt = false;
		}

		public static void NoOperation()
		{
			return;
		}

		// Shared Script data
		public static object ReadSharedValue(string name)
		{
			object data = 0;
			if (RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.ContainsKey(name))
				RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.TryGetValue(name, out data);
			return data;
		}

		public static void SetSharedValue(string name, object value)
		{
			RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.AddOrUpdate(name, value, (key, oldValue) => value);
		}
		public static void RemoveSharedValue(string name)
		{
			RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.TryRemove(name, out object data);
		}

		public static bool CheckSharedValue(string name)
		{
			if (RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.ContainsKey(name))
				return true;
			else
				return false;
		}

		// Comandi Script per Menu Old

		public static bool HasMenu() 
		{
			return World.Player.HasMenu;
		}

		public static void CloseMenu()
		{
			if(World.Player.HasMenu)
			{
				ClientCommunication.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, 0, 0, 0));
				World.Player.MenuEntry.Clear();
				World.Player.HasMenu = false;
			}
		}

		public static bool MenuContain(string submenu)
		{
			foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
			{
				if (menuentry.ModelText.Contains(submenu))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetMenuTitle()
		{
			if (World.Player.HasMenu)
			{
				return World.Player.MenuQuestionText;
			}
			return String.Empty;
		}

		public static void WaitForMenu(int delay) // Delay in MS
		{
			BlockMenu = true;
            int subdelay = delay;
			while (!World.Player.HasMenu && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
			BlockMenu = false;
		}

		public static void MenuResponse(string submenu) // Delay in MS
		{
			int i = 1;
			foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
			{
				if (menuentry.ModelText.Contains(submenu))
				{
					ClientCommunication.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, (ushort)i, menuentry.ModelID, menuentry.ModelColor));
					World.Player.MenuEntry.Clear();
                    World.Player.HasMenu = false;
					return;
				}
				i++;
			}
			ClientCommunication.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, 0, 0, 0));
			World.Player.MenuEntry.Clear();
			World.Player.HasMenu = false;
			Scripts.SendMessageScriptError("MenuResponse Error: No menu name found");
		}

		// Comandi Query String

		public static bool HasQueryString()
		{
			return World.Player.HasQueryString;
		}

        public static void WaitForQueryString(int delay) // Delay in MS
		{
			BlockGump = true;
			int subdelay = delay;
			while (!World.Player.HasQueryString && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
			BlockGump = false;
        }

		public static void QueryStringResponse(bool okcancel, string response) // Delay in MS
		{
			ClientCommunication.SendToServerWait(new StringQueryResponse(World.Player.QueryStringID, World.Player.QueryStringType, World.Player.QueryStringIndex, okcancel, response));
			World.Player.HasQueryString = false;
		}

		// Script function
		public static void ScriptRun(string scriptfile)
		{
			Scripts.EnhancedScript script = Scripts.Search(scriptfile);
			if (script != null)
			{
				script.Run = true;
			}
			else
				Scripts.SendMessageScriptError("ScriptStatus: Script not exist");
		}

		public static void ScriptStop(string scriptfile)
		{
			Scripts.EnhancedScript script = Scripts.Search(scriptfile);
			if (script != null)
			{
				script.Run = false;
			}
			else
				Scripts.SendMessageScriptError("ScriptStatus: Script not exist");
		}

		public static bool ScriptStatus(string scriptfile)
		{
			Scripts.EnhancedScript script = Scripts.Search(scriptfile);
			if (script != null)
			{
				return script.Run;
			}
			else
			{
				Scripts.SendMessageScriptError("ScriptStatus: Script not exist");
				return false;
			}
		}

		// Pet Rename
		public static void PetRename(int serial, string name)
		{
			ClientCommunication.SendToServerWait(new RenameRequest((uint)serial, name));
		}

		public static void PetRename(RazorEnhanced.Mobile mob, string name)
		{
			ClientCommunication.SendToServerWait(new RenameRequest((uint)mob.Serial, name));
		}

		// Lock stealth run
		public static void NoRunStealthToggle(bool enable)
		{
				Engine.MainWindow.ChkNoRunStealth.Invoke(new Action(() => Engine.MainWindow.ChkNoRunStealth.Checked = enable));
		}

		public static bool NoRunStealthStatus()
		{
			return Engine.MainWindow.ChkNoRunStealth.Checked;
		}
		internal static bool RotateMove(Direction d)
		{
			if ((World.Player.Direction & Assistant.Direction.Mask) != d)
			{
				Player.Run(d.ToString());
			}
			return Player.Run(d.ToString());
		}

		public static IntPtr _handleToUo = IntPtr.Zero;
		public static void testpath(int x, int y, int z) 
		{
			var pathFindEnded = false;
			
			new Thread(() =>
			{
				var timeout = 1000;
				DateTime end = DateTime.Now.AddMilliseconds(timeout);
				while (!pathFindEnded && DateTime.Now < end)
					Thread.Sleep(10);

				if (!pathFindEnded)
				{
					if (_handleToUo == IntPtr.Zero)
						_handleToUo = DLLImport.Win.OpenProcess(2035711, false, ClientCommunication.ClientProcess.Id);

					DLLImport.Win.WriteProcessMemory(_handleToUo, 0x400600 - 2, BitConverter.GetBytes((short)0), 2, out var dummy);
				}
			}).Start();

			UoWarper.UODLLHandleClass.Pathfind(x, y, z);
			pathFindEnded = true;
		
		}


	}
}