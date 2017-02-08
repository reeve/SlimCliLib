using System;
using System.Collections;

namespace Com.AdamReeve.Slim.SlimCliLib
{
	public class Server
	{
	    private SlimCli client;
	    
	    private string serverVersion;

		internal Server(SlimCli client) {
	        this.client = client;
	        this.serverVersion = getServerVersion();
		}

	    public string ServerVersion {
	        get {
	            return serverVersion;
	        }
	    }	    
	    
	    public bool Scanning {
	        get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.RESCAN, null, new string[]{"?"}));
    	        return "1".Equals(result.ResponseParams[0]);	            
	        }
	    }

	    public int AlbumCount {
	        get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.TOTALALBUMS, null, new string[]{"?"}));
    	        return int.Parse(result.ResponseParams[0]);	            	            
	        }
	    }
	    
	    public int ArtistCount {
	        get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.TOTALARTISTS, null, new string[]{"?"}));
    	        return int.Parse(result.ResponseParams[0]);	            	            
	        }
	    }

	    public int SongCount {
	        get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.TOTALSONGS, null, new string[]{"?"}));
    	        return int.Parse(result.ResponseParams[0]);	            	            
	        }
	    }

	    public int GenreCount {
	        get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.TOTALGENRES, null, new string[]{"?"}));
    	        return int.Parse(result.ResponseParams[0]);	            	            
	        }
	    }	    
	    
	    private string getServerVersion() {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.VERSION, null, new string[]{"?"}));
    	        return result.ResponseParams[0];	            	        
	    }	    
	    
		public Player[] getPlayers() {
	        ExtendedResponse result = client.makeRequest(new ExtendedCommand(ExtendedCommandString.PLAYERS));

	        if (result.Responses.Count == 0) {
	            throw new InvalidResponseException("There don't appear to be any players connected", result);
	        }
	        
	        Player[] players = new Player[result.Responses.Count];
	        int i = 0;
	        foreach(Hashtable map in result.Responses) {
	            Player player = new Player(client, map);
	            players[i++] = player;
	        }
	        
    	    return players;
	    }
	    
	    public void rescan(bool playlistsOnly) {
	        string[] param = null;
	        if (playlistsOnly) {
	            param = new string[]{"playlists"};
	        }	        
	        
	        BasicCommand command = new BasicCommand(CommandString.RESCAN, null, param);        
            BasicResponse result = client.makeRequest(command);
	    }
	    
	    public void wipecache() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.WIPECACHE, null));
	    }
	}
}
