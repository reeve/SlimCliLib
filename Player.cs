using System;
using System.Collections;

namespace Com.AdamReeve.Slim.SlimCliLib
{
	/// <summary>
	/// Description of Player.
	/// </summary>

    public class Player {
	    private SlimCli client;
	    
        private int index;
        private string id;
        private string ip;	        
        private string name;	        
        private string model;	        
        private string displayType;
        private bool connected;	        
        
        internal const string FIELD_INDEX = "playerindex";
        internal const string FIELD_ID = "playerid";
        internal const string FIELD_IP = "ip";
        internal const string FIELD_NAME = "name";
        internal const string FIELD_MODEL = "model";
        internal const string FIELD_DISPLAYTYPE = "displaytype";
        internal const string FIELD_CONNECTED = "connected";        
        
        public enum PlayerState {
            PLAY,
            PAUSE,
            STOP
        }
        
        internal Player(SlimCli client, Hashtable map) {
            this.client = client;
            index       = int.Parse((string)map[FIELD_INDEX]);
            id          = (string)map[FIELD_ID];
            ip          = (string)map[FIELD_IP];
            name        = (string)map[FIELD_NAME];
            model       = (string)map[FIELD_MODEL];
            displayType = (string)map[FIELD_DISPLAYTYPE];
            connected   = "1".Equals((string)map[FIELD_CONNECTED]);
        }
        
        #region Accessors
        internal Player(string id, string name) {
            this.id = id;
            this.name = name;
        }

        public int Index {
            get {
                return index;
            }
        }
        
        public string Id {
            get {
                return id;
            }
        }
        
        public string Ip {
            get {
                return ip;
            }
        }
        
        public string Name {
            get {
                return name;
            }
        }
        
        public string Model {
            get {
                return model;
            }
        }
        
        public string DisplayType {
            get {
                return displayType;
            }
        }
        
        public bool Connected {
            get {
                //TODO this should be dynamic
                return connected;
            }
        }
        
        public bool Power {
            get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.POWER, this, new string[]{"?"}));
    	        return "1".Equals(result.ResponseParams[0]);
            }
            set {
    	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.POWER, this, new string[]{value ? "1" : "0"}));
            }
        }
        
        public float Volume {
            get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.VOLUME, this, new string[]{"?"}));
    	        return float.Parse(result.ResponseParams[0]);
            }
            set {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.VOLUME, this, new string[]{value.ToString()}));
            }
        }
        
        public PlayerState State {
            get {
                BasicResponse result = client.makeRequest(new BasicCommand(CommandString.MODE, this, new string[]{"?"}));
    	        return (PlayerState)Enum.Parse(typeof(PlayerState), result.ResponseParams[0].ToUpper());
            }
        }
        #endregion        

        #region Control Methods
        public void play() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.PLAY, this, null));
	    }
	    
	    public void pause() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.PAUSE, this, null));
	    }

	    public void stop() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.STOP, this, null));
	    }
	    
	    public void nextTrack() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.PLAYLISTINDEX, this, new string[]{"+1"}));
	    }
	
	    public void prevTrack() {
	        BasicResponse result = client.makeRequest(new BasicCommand(CommandString.PLAYLISTINDEX, this, new string[]{"-1"}));
	    }
        
        public void volUp() {
            BasicResponse result = client.makeRequest(new BasicCommand(CommandString.VOLUME, this, new string[]{"+2.5"}));
        }

        public void volDown() {
            BasicResponse result = client.makeRequest(new BasicCommand(CommandString.VOLUME, this, new string[]{"-2.5"}));
        }
        #endregion
        
        #region Playlist Accessors
        public Track CurrentTrack {
            get {
                Hashtable taggedParams = new Hashtable();
                taggedParams.Add("tags", "gacbhldiqtymkovrfzjnu");
                ExtendedResponse result = client.makeRequest(new ExtendedCommand(ExtendedCommandString.STATUS, this, taggedParams, ExtendedCommandStartParam.DASH, 1));
                if (result.Responses.Count > 0) {
                    return new Track(client, (Hashtable)result.Responses[0]);
                } else {
                    return null;
                }
            }
        }
        
        #endregion
        
        #region Utils
		public override string ToString() {
			return name;
		}
				
        public string dump() {
            return String.Format("[Player: index = {0}, id = {1}, ip = {2}, name = {3}, model = {4}, displayType = {5}, connected = {6}]",
                                 index,
                                 id,
                                 ip,
                                 name,
                                 model,
                                 displayType,
                                 connected);
        }
        #endregion
        
    }
}
