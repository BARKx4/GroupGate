using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeatherLoader.ModList;

namespace GroupGate
{
	public class GroupGateInfo : IModInfo
	{
		public GroupGateInfo()
		{

		}

		public string GetModName()
		{
			return "BARKx4-GroupGate";
		}

		public string GetModVersion()
		{
			return "1.0.0";
		}

		public string GetPrettyModName()
		{
			return "Group Gate";
		}

		public string GetPrettyModVersion()
		{
			return "Version 1.0";
		}

		public bool CanAcceptModlessClients()
		{
			return true;
		}

		public bool CanConnectToModlessServers()
		{
			return true;
		}

		public string GetCreditString()
		{
			return "By BARK BARK BARK BARK";
		}
	}
}