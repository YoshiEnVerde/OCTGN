﻿using System;
using System.Collections.Generic;

namespace Skylabs.ConsoleHelper
{
    public struct ConsoleArgument
    {
        public String Argument { get; set; }

        public String Value { get; set; }
    }

    public class ConsoleMessage
    {
        public String RawData
        {
            get
            {
                return _rawData;
            }
            set
            {
                _rawData = value.TrimStart(new[] { ' ' });
                ParseMessage();
            }
        }

        public String Header { get; set; }

        public List<ConsoleArgument> Args { get; set; }

        private String _rawData;

        public ConsoleMessage()
        {
            RawData = "";
        }

        public ConsoleMessage(String rawData)
        {
            RawData = rawData;
        }

        public void ParseMessage(String data)
        {
            RawData = data;
        }

        private void ParseMessage()
        {
            //TODO better handling of jibberish. Probubly be best to use regex. It'd be a lot cleaner and sexier.
            RawData = RawData.TrimStart(new[] { ' ' });
            int ws = RawData.IndexOf(' ');
            Header = "";
            Args = new List<ConsoleArgument>();
            if(ws == -1)
            {
                Header = RawData;
            }
            else
            {
                Header = RawData.Substring(0, ws);
                try
                {
                    String args = RawData.Substring(ws + 1);
                    args = args.TrimStart(new[] { ' ' });
                    if(!args.Equals(""))
                    {
                        String[] araw = args.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if(araw.Length != 0)
                        {
                            foreach(String a in araw)
                            {
                                string temp = a.Trim();
                                ws = temp.IndexOf(' ');
                                if(ws == -1)
                                {
                                    ConsoleArgument ca = new ConsoleArgument {Argument = temp};
                                    Args.Add(ca);
                                }
                                else
                                {
                                    ConsoleArgument ca = new ConsoleArgument
                                                             {
                                                                 Argument = temp.Substring(0, ws),
                                                                 Value = temp.Substring(ws + 1)
                                                             };
                                    Args.Add(ca);
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    new ConsoleEventError("Error parsing arguments. ", e).WriteEvent(true);
                }
            }
        }
    }
}