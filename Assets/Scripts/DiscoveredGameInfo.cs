using System;

namespace AssemblyCSharp
{
	public class DiscoveredGameInfo {
		public string hostAddress;
		public string hostPort;
		public int timeStamp; // in milleseconds given by (int)(System.DateTime.Now.Ticks / 10000)
		// the following are rendered fields in the lobby ui
		public string gameName;
		public bool passwordProtected;
		public int numPlayers;
		public int playerLimit;

		public DiscoveredGameInfo (string host, string port, int timeStamp,
			string gameName, bool passwordProtected, int numPlayers, int playerLimit) {
			this.hostAddress = host;
			this.hostPort = port;
			this.timeStamp = timeStamp;
			this.gameName = gameName;
			this.passwordProtected = passwordProtected;
			this.numPlayers = numPlayers;
			this.playerLimit = playerLimit;
		}
	}
}