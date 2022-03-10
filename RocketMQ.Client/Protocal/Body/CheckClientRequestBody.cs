﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketMQ.Client//.Protocal.Body
{
    public class CheckClientRequestBody : RemotingSerializable
    {
        public String clientId { get; set; }
        public String group { get; set; }
        public SubscriptionData subscriptionData { get; set; }
    }
}
