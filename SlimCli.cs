/*
 * Copyright 2016 Adam Reeve
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under the License 
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express 
 * or implied. See the License for the specific language governing permissions and limitations 
 * under the License.
 */

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Reflection;

namespace Com.AdamReeve.Slim.SlimCliLib
{

	public class SlimCli
	{
			
	    private readonly int PORT;
	    private readonly string HOST;

	    private Server server;
	    
	    private TcpClient client;
	    private StreamReader reader;
	    private StreamWriter writer;

	    public SlimCli(string host, int port)
	    {
	        this.PORT = port;
	        this.HOST = host;
	        
	        try {
	            client = new TcpClient(HOST, PORT);
	        } catch {
	            Console.WriteLine("Unable to connect to {0}:{1}", HOST, PORT);
	        }
	        
	        
	        
	        if (client == null || !client.Connected) {
	            Console.WriteLine("Unable to connect to {0}:{1}", HOST, PORT);
	        } else {	        
    	        reader = new StreamReader(client.GetStream(), Encoding.ASCII);
    	        writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
    	        writer.AutoFlush = true;	        	       
    	        
    	        server = new Server(this);
	        }
	    }
	    
	    public bool Connected {
	        get {
	            return client != null && client.Connected;
	        }
	    }
	    
	    public int ServerPort {
	        get {
	            return PORT;
	        }
	    }
	    
	    public string ServerHost {
	        get {
	            return HOST;
	        }
	    }	    
	    
	    public Server getServer() {
	        if (!Connected) {
	            return null;
	        }
	        return server;
	    }
	    	    
	    public BasicResponse makeRequest(BasicCommand command) {
	        if (!Connected) {
	            return null;
	        }
	        lock(this) {
    	        return new BasicResponse(command, sendMessage(command.ToString()));
	        }
	    }
	    
	    public ExtendedResponse makeRequest(ExtendedCommand command) {
	        if (!Connected) {
	            return null;
	        }
	        lock(this) {
    	        return new ExtendedResponse(command, sendMessage(command.ToString()));
	        }
	    }	    

	    private string sendMessage(string message) {;
	        //TODO: need to urlencode each parameter individually
	        writer.WriteLine(message);
		    return reader.ReadLine();
	    }
	    	    
	    public string getVersion() {
	        Assembly a = Assembly.GetExecutingAssembly();
		    return a.GetName().Version.ToString();
	    }
	}	
	
}
