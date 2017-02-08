using System;
using System.Collections;

namespace Com.AdamReeve.Slim.SlimCliLib
{
	/// <summary>
	/// Description of Track.
	/// </summary>

	public class Track
	{
   	    private SlimCli client;

        private int? id;
        private string title;
        private string genre;
        private string artist;
        private string composer;
        private string band;
        private string conductor;
        private string album;
        private float? duration;
        private int? disc = null;
        private int? disccount;
        private int? tracknum;
        private string year;
        private int? bpm;
        private string comment;
        private string contentType;
        private string tagVersion;
        private string bitrate;
        private int? filesize;
        private string drm;
        private bool coverArt;
        private DateTime? modTime;
        private Uri fileUrl;

        private const string FIELD_ID = "id";
        private const string FIELD_TITLE = "title";
        private const string FIELD_GENRE = "genre";
        private const string FIELD_GENRE_ID = "genre_id";
        private const string FIELD_ARTIST = "artist";
        private const string FIELD_ARTIST_ID = "artist_id";
        private const string FIELD_COMPOSER = "composer";
        private const string FIELD_BAND = "band";
        private const string FIELD_CONDUCTOR = "conductor";
        private const string FIELD_ALBUM = "album";
        private const string FIELD_ALBUM_ID = "album_id";
        private const string FIELD_DURATION = "duration";
        private const string FIELD_DISC = "disc";
        private const string FIELD_DISCCOUNT = "disccount";
        private const string FIELD_TRACKNUM = "tracknum";
        private const string FIELD_YEAR = "year";
        private const string FIELD_BPM = "bpm";
        private const string FIELD_COMMENT = "comment";
        private const string FIELD_CONTENT_TYPE = "type";
        private const string FIELD_TAGVERSION = "tagversion";
        private const string FIELD_BITRATE = "bitrate";
        private const string FIELD_FILESIZE = "filesize";
        private const string FIELD_DRM = "drm";
        private const string FIELD_COVERART = "coverart";
        private const string FIELD_MODTIME = "modificationTime";
        private const string FIELD_FILEURL = "url";

        internal Track(SlimCli client, Hashtable map) {
            this.client = client;
            id          = parseInt(map[FIELD_ID]);
	        title       = (string)map[FIELD_TITLE];
	        genre       = (string)map[FIELD_GENRE];
	        artist      = (string)map[FIELD_ARTIST];
	        composer    = (string)map[FIELD_COMPOSER];
	        band        = (string)map[FIELD_BAND];
	        conductor   = (string)map[FIELD_CONDUCTOR];
	        album       = (string)map[FIELD_ALBUM];
	        duration    = parseFloat(map[FIELD_DURATION]);
	        disc        = parseInt(map[FIELD_DISC]);
	        disccount   = parseInt(map[FIELD_DISCCOUNT]);
	        tracknum    = parseInt(map[FIELD_TRACKNUM]);
	        year        = (string)map[FIELD_YEAR];
	        bpm         = parseInt(map[FIELD_BPM]);
	        comment     = (string)map[FIELD_COMMENT];
	        contentType = (string)map[FIELD_CONTENT_TYPE];
	        tagVersion  = (string)map[FIELD_TAGVERSION];
	        bitrate     = (string)map[FIELD_BITRATE];
	        filesize    = parseInt(map[FIELD_FILESIZE]);
	        drm         = (string)map[FIELD_DRM];
	        coverArt    = "1".Equals(map[FIELD_COVERART]);
	        modTime     = parseDate(map[FIELD_MODTIME]);
	        fileUrl     = new Uri((string)map[FIELD_FILEURL]);
        }
        
        private int? parseInt(object val) {
            if (val == null) {
                return null;
            } else {
                return int.Parse((string)val);
            }
        }
        
        private float? parseFloat(object val) {
            if (val == null) {
                return null;
            } else {
                return float.Parse((string)val);
            }
        }
        
        private DateTime? parseDate(object val) {
            if (val == null) {
                return null;
            } else {
                return DateTime.Parse((string)val);
            }
        }
        

        public int? Id {
            get {
                return id;
            }
        }
        
        public string Title {
            get {
                return title;
            }
        }
        
        public string Genre {
            get {
                return genre;
            }
        }
        
        public string Artist {
            get {
                return artist;
            }
        }
        
        public string Composer {
            get {
                return composer;
            }
        }
        
        public string Band {
            get {
                return band;
            }
        }
        
        public string Conductor {
            get {
                return conductor;
            }
        }
        
        public string Album {
            get {
                return album;
            }
        }
        
        public float? Duration {
            get {
                return duration;
            }
        }
        
        public int? Disc {
            get {
                return disc;
            }
        }
        
        public int? Disccount {
            get {
                return disccount;
            }
        }
        
        public int? Tracknum {
            get {
                return tracknum;
            }
        }
        
        public string Year {
            get {
                return year;
            }
        }
        
        public int? Bpm {
            get {
                return bpm;
            }
        }
        
        public string Comment {
            get {
                return comment;
            }
        }
        
        public string ContentType {
            get {
                return contentType;
            }
        }
        
        public string TagVersion {
            get {
                return tagVersion;
            }
        }
        
        public string Bitrate {
            get {
                return bitrate;
            }
        }
        
        public int? Filesize {
            get {
                return filesize;
            }
        }
        
        public string Drm {
            get {
                return drm;
            }
        }
        
        public bool CoverArt {
            get {
                return coverArt;
            }
        }
        
        public DateTime? ModTime {
            get {
                return modTime;
            }
        }
        
        public Uri FileUrl {
            get {
                return fileUrl;
            }
        }
        
        
        
	}
}
