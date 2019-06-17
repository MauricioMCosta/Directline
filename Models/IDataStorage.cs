﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Directline.Models
{
    public interface IDataStorage
    {
        IDictionary<string, Conversation> Conversations { get; set; }
        IDictionary<string, BotState> BotStates { get; set; }
    }
}
