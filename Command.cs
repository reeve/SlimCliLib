using System;
using System.Collections;
using System.Text;
using System.Web;

namespace Com.AdamReeve.Slim.SlimCliLib
{
    public enum CommandString {
        PLAY,
        PAUSE,
        STOP,
        PLAYERCOUNT,
        PLAYLISTINDEX,
        POWER,
        MODE,
        VOLUME,
        DEBUG,
        PREF,
        VERSION,
        SLEEP,
        SYNC,
        SIGNALSTRENGTH,
        MUTE,
        BASS,
        TREBLE,
        PITCH,
        RATE,
        DISPLAY,
        LINESPERSCREEN,
        DISPLAYNOW,
        PLAYERPREF,
        RESCAN,
        WIPECACHE,
        TOTALARTISTS,
        TOTALALBUMS,
        TOTALSONGS,
        TOTALGENRES
    }
    
    public enum ExtendedCommandString {
        PLAYERS, 
        GENRES,
        ARTISTS, 
        ALBUMS, 
        TITLES, 
        PLAYLISTS,
        PLAYLISTTRACKS,
        SONGINFO,
        STATUS,
        PLAYERPREF
    }
        
    public class ExtendedCommandStartParam {
        private int regularVal;
        private bool dashVal;
        
        public ExtendedCommandStartParam(int i) {
            regularVal = i;
            dashVal = false;
        }

        private ExtendedCommandStartParam() {
            dashVal = true;
        }
        
    	public override string ToString() {
            return dashVal ? "-" : regularVal.ToString();
    	}
        
        public static readonly ExtendedCommandStartParam DASH = new ExtendedCommandStartParam();
        public static readonly ExtendedCommandStartParam ZERO = new ExtendedCommandStartParam(0);
        
    }
    
    public interface Command {

        string CommStr {
            get;
        }
        
        string Base {
            get;
        }
    }
    
    public abstract class AbstractCommand : Command {

        protected static string[] CommandStringValues = new String[] {
            "play",
            "pause",
            "stop",
            "player count",
            "playlist index",
            "power",
            "mode",
            "mixer volume",
            "debug",
            "pref",
            "version",
            "sleep",
            "sync",
            "signalstrength",
            "mute",
            "mixer bass",
            "mixer treble",
            "mixer pitch",
            "rate",
            "display",
            "linesperscreen",
            "displaynow",
            "playerpref",
            "rescan",
            "wipecache",
            "info total artists",
            "info total albums",
            "info total songs",
            "info total genres"                
        };
        
        protected static string[] ExtendedCommandStringValues = new String[] {
            "players", 
            "genres",
            "artists", 
            "albums", 
            "titles", 
            "playlists",
            "playlisttracks",
            "songinfo",
            "status"
        };

        protected static string[] ExtendedCommandStringDelimeters = new String[] {
            Player.FIELD_INDEX, 
            "id",
            "id", 
            "id", 
            "id", 
            "id",
            "playlist index",
            null,
            "playlist index"
        };

        protected Player player;
        
        public Player Player {
            get {
                return player;
            }
        }        
        
        public abstract string CommStr {
            get;
        }
        
        public string Base {
            get {
                if (player == null) {
                    return CommStr;
                } else {
                    return HttpUtility.UrlEncode(player.Id) + " " + CommStr;
                }
            }
        }
    }

    public class BasicCommand : AbstractCommand {
        private CommandString commStr;
        private string[] positionalParams;
        
        public BasicCommand(CommandString command, Player player, string[] positionalParams)
        {
            this.commStr = command;
            this.player = player;
            this.positionalParams = positionalParams;
        }
        
        public BasicCommand(CommandString command, string[] positionalParams) : this(command, null, positionalParams) {}
        
        public string[] PositionalParams {
            get {
                return (string[])positionalParams.Clone();
            }
        }
        
        public override string CommStr {
            get {
                return CommandStringValues[(int)commStr];
            }
        }
                
        public override string ToString() {
            StringBuilder buf = new StringBuilder(CommStr);

	        if (player != null) {
	            buf.Insert(0, player.Id + " ");
	        }
	                            	        
	        if (positionalParams != null) {
    	        foreach (string param in positionalParams) {
    	            buf.Append(" ").Append(param);
    	        }
	        }
	        
	        return buf.ToString();
		}

    }
    
    public class ExtendedCommand : AbstractCommand {
        private ExtendedCommandString commStr;
        private Hashtable taggedParams;
        private ExtendedCommandStartParam start = ExtendedCommandStartParam.ZERO;
        private int perResponse = 100;
        
        public ExtendedCommand(ExtendedCommandString command)
        {
            this.commStr = command;
        }

        public ExtendedCommand(ExtendedCommandString command, Hashtable taggedParams) : this(command)
        {
            this.taggedParams = taggedParams;
        }

        public ExtendedCommand(ExtendedCommandString command, Player player, Hashtable taggedParams, ExtendedCommandStartParam start, int perResponse) : this(command, taggedParams)
        {
            this.player = player;
            this.start = start;
            this.perResponse = perResponse;
        }
        

        public Hashtable TaggedParams {
            get {
                //TODO: make readonly
                return taggedParams;
            }
        }
        
        public override string CommStr {
            get {
                return ExtendedCommandStringValues[(int)commStr];
            }
        }
        
        public ExtendedCommandStartParam Start {
            get {
                return start;
            }
        }
        
        public int PerResponse {
            get {
                return perResponse;
            }
        }        

        public string Delimeter {
            get {
                return ExtendedCommandStringDelimeters[(int)commStr];
            }
        }
        
		public override string ToString() {
            StringBuilder buf = new StringBuilder(CommStr);

	        if (player != null) {
	            buf.Insert(0, player.Id + " ");
	        }
	                            
	        buf.Append(" ").Append(start).Append(" ").Append(perResponse);
	        
	        if (taggedParams != null) {
    	        foreach (string tag in taggedParams.Keys) {
    	            buf.Append(" ").Append(tag).Append(":").Append(taggedParams[tag]);
    	        }
	        }
	        
	        return buf.ToString();
		}
    }
    
    

}
