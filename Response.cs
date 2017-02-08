using System;
using System.Web;
using System.Collections;
using System.Text;

namespace Com.AdamReeve.Slim.SlimCliLib
{
    
    public interface Response {
        
        bool Valid {
            get;
        }
        
        Player Player {
            get;
        }	        

        Command Command {
            get;
        }
        
        string Raw {
            get;
        }
        
        string Decoded {
            get;
        }
        
        string dump();

    }
    
    public abstract class AbstractResponse : Response {
        protected string raw;
        protected string decoded;
        protected Command command;
        protected bool valid;
        protected Player player;	        
        
        public bool Valid {
            get {
                return valid;
            }
        }
                
        public Player Player {
            get {
                return player;
            }
        }	        

        public Command Command {
            get {
                return command;
            }
        }
        
        public string Raw {
            get {
                return raw;	                
            }
        }
        
        public string Decoded {
            get {
                return decoded;	                
            }
        }
        
        public abstract void dump(StringBuilder buf);
        
        public string dump() {
            StringBuilder buf = new StringBuilder();
            buf.AppendLine("=== RESPONSE ===");
            buf.Append("Command String> ").AppendLine(command.ToString());
            buf.Append("Player> ").AppendLine(player == null ? "NONE" : player.Name);
            buf.Append("Raw Response> ").AppendLine(raw);
            buf.Append("Valid> ").AppendLine(valid ? "YES" : "NO");
            
            if (valid) {
                this.dump(buf);
            }
            
            return buf.ToString();
        }
        
    }
    
    
    public class BasicResponse : AbstractResponse {
        private string[] responseParams;
        
        public string[] ResponseParams {
            get {
                if (!valid) {
                    return null;
                }
                return (string[])responseParams.Clone();
            }
        }
        
        internal BasicResponse(Player player, BasicCommand command, string raw) : this(command, raw) {
            this.player = player;
        }

        internal BasicResponse(BasicCommand command, string raw) {
            this.command = command;
            this.raw = raw;
            this.decoded = HttpUtility.UrlDecode(raw);
        
            string commandBase = command.Base;
            valid = raw.StartsWith(commandBase, StringComparison.CurrentCultureIgnoreCase);
            
            if (valid) {
                responseParams = raw.Substring(commandBase.Length).Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i=0; i<responseParams.Length; i++) {
                    responseParams[i] = HttpUtility.UrlDecode(responseParams[i]);
                }
            } else {
                throw new InvalidResponseException("Response appears invalid", this);
            }
        }
        
        public override void dump(StringBuilder buf) {       
            buf.AppendLine();
            buf.AppendLine("Response Params>");
            foreach(string param in responseParams) {
			    buf.AppendLine(param);
			}
        }

    }

    public class ExtendedResponse : AbstractResponse {
        private IList responses = new ArrayList();
        private Hashtable taggedParams = new Hashtable();
        
        public IList Responses {
            get {
                if (!valid) {
                    return null;
                }
                //TODO: make readonly
                return responses;
            }
        }

        public Hashtable TaggedParams {
            get {
                if (!valid) {
                    return null;
                }
                //TODO: make readonly
                return taggedParams;
            }
        }
        
        
        internal ExtendedResponse(ExtendedCommand command, string raw) {
            this.command = command;
            this.raw = raw;
            this.decoded = HttpUtility.UrlDecode(raw);
            
            string commandStr = command.ToString();
            
            string commandBase = command.Base;
            valid = raw.StartsWith(commandBase, StringComparison.CurrentCultureIgnoreCase);            

            if (valid) {
                Hashtable currentBucket = taggedParams;

                string[] responseParams = raw.Substring(HttpUtility.UrlEncode(commandStr).Length + 1).Split(new char[]{' '});
                
                for (int i=0; i<responseParams.Length; i++) {
                    string decodedParam = HttpUtility.UrlDecode(responseParams[i]);
                    
                    int sepPos = decodedParam.IndexOf(':');
                    if (sepPos == -1) {
                        //bad data
                        continue;
                    }
                    
                    string tag = decodedParam.Substring(0, sepPos);
                    string value = decodedParam.Substring(sepPos + 1);

                    if (command.Delimeter.Equals(tag)) {
                        currentBucket = new Hashtable();
                        responses.Add(currentBucket);
                        currentBucket.Add(tag, value);
                    } else {
                        currentBucket.Add(tag, value);
                    }
                }

            } else {
                throw new InvalidResponseException("Response appears invalid", this);
            }
        }
        
        internal ExtendedResponse(Player player, ExtendedCommand command, string raw) : this(command, raw) {
            this.player = player;
        }

        public override void dump(StringBuilder buf) {       
            buf.AppendLine();
            buf.AppendLine("Tagged Return Params>");
            foreach(string key in taggedParams.Keys) {
                buf.AppendFormat("{0} = {1}", key, taggedParams[key]).AppendLine();
			}

			foreach(Hashtable responseMap in responses) {			    
                buf.AppendLine();
    			buf.AppendLine("Tagged Return Object>");
    			foreach(string key in responseMap.Keys) {
    			    buf.AppendFormat("{0} = {1}", key, responseMap[key]).AppendLine();
    			}
			}       
        }

    }
    
    public class InvalidResponseException : Exception {
        Response response;
        
        public InvalidResponseException(string msg, Response response) : base(msg)
        {
            this.response = response;
        }
                
        public Response Response {
            get {
                return response;
            }
        }
        
        
    }

}
