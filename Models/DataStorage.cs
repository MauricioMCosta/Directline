﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Directline.Models
{
    public class DataStorage : IDataStorage
    {
        public IDictionary<string, Conversation> Conversations { get ; set ; }
        public IDictionary<string, BotState> BotStates { get ; set; }

        public DataStorage()
        {
            Conversations = new Dictionary<string, Conversation>();
            BotStates = new Dictionary<string, BotState>();
        }
    }
}
