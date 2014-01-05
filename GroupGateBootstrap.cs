using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeatherLoader;
using Facepunch;
using uLink;
using UnityEngine;

namespace GroupGate
{
	[Bootstrap]
	public class GroupGateBootstrap : uLink.MonoBehaviour
	{
		String configDir;
		String lockGroup = "none";
		int minVacDays = 180;
		String userProfilePage;
		NetUser connectingPlayer;

		public void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
		{
			connectingPlayer = (NetUser)player.localData;
			String playerSteamURL = "http://steamcommunity.com/profiles/" + connectingPlayer.userID.ToString();

			WebClient wc = new WebClient ();
			wc.DownloadStringCompleted += (sender, e) => 
			{
				userProfilePage = e.Result;

				if (lockGroup != "none")
				{
					if (userProfilePage.IndexOf("http://steamcommunity.com/groups/" + lockGroup) == -1) 
					{
						connectingPlayer.Kick(NetError.Facepunch_Kick_Ban, true);
					}
				}

				Match vacMatch = Regex.Match(userProfilePage, @"^([0-9]{1,5}) day\(s\) since last ban$");
				if (vacMatch.Success)
				{
					int daysSinceBan = Convert.ToInt32(vacMatch.Groups[1].Value);
					if (daysSinceBan < minVacDays)
					{
						connectingPlayer.Kick(NetError.Facepunch_Kick_Ban, true);
					}
				}
			};

			wc.DownloadStringAsync(new Uri(playerSteamURL));
		}

		public void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}

		public void Start()
		{
			configDir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config"));

			int counter = 0;
			string line;

			System.IO.StreamReader file = new System.IO.StreamReader("configDir" + "\\groupgate.cfg");
			while((line = file.ReadLine()) != null)
			{
				string[] confPair = line.Split(' ');

				if (confPair [0] == "lockgroup")
					lockGroup = confPair[1];
				else if (confPair [0] == "minvacdays")
					minVacDays = Convert.ToInt32(confPair[1]);

				counter++;
			}

			file.Close();
		}
	}
}

