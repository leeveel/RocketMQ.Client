﻿namespace RocketMQ.Client
{
    public enum SendStatus
    {
        SEND_OK,
        FLUSH_DISK_TIMEOUT,
        FLUSH_SLAVE_TIMEOUT,
        SLAVE_NOT_AVAILABLE
    }
}
