﻿using System;

namespace RocketMQ.Client
{
    public class EndTransactionRequestHeader : CommandCustomHeader
    {
        [CFNotNull]
        public String producerGroup { get; set; }
        [CFNotNull]
        public long tranStateTableOffset { get; set; }
        [CFNotNull]
        public long commitLogOffset { get; set; }
        [CFNotNull]
        public int commitOrRollback { get; set; } // TRANSACTION_COMMIT_TYPE
                                                  // TRANSACTION_ROLLBACK_TYPE
                                                  // TRANSACTION_NOT_TYPE

        [CFNullable]
        public bool fromTransactionCheck { get; set; } = false;

        [CFNotNull]
        public String msgId { get; set; }

        public String transactionId { get; set; }

        public void checkFields() 
        {
            if (MessageSysFlag.TRANSACTION_NOT_TYPE == this.commitOrRollback)
            {
                return;
            }

            if (MessageSysFlag.TRANSACTION_COMMIT_TYPE == this.commitOrRollback)
            {
                return;
            }

            if (MessageSysFlag.TRANSACTION_ROLLBACK_TYPE == this.commitOrRollback)
            {
                return;
            }

            throw new RemotingCommandException("commitOrRollback field wrong");
        }

        public override String ToString()
        {
            return "EndTransactionRequestHeader{" +
                "producerGroup='" + producerGroup + '\'' +
                ", tranStateTableOffset=" + tranStateTableOffset +
                ", commitLogOffset=" + commitLogOffset +
                ", commitOrRollback=" + commitOrRollback +
                ", fromTransactionCheck=" + fromTransactionCheck +
                ", msgId='" + msgId + '\'' +
                ", transactionId='" + transactionId + '\'' +
                '}';
        }
    }
}
