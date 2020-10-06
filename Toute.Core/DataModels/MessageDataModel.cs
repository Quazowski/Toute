﻿using System;

namespace Toute
{
    /// <summary>
    /// Model of message that is saved in DB
    /// </summary>
    public class MessageDataModel
    {
        /// <summary>
        /// Unique Id of the message
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Value of the message
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Date when message was sent
        /// </summary>
        public virtual DateTime DateOfSent { get; set; }

        /// <summary>
        /// Is message sent by user
        /// </summary>
        public virtual bool SentByMe { get; set; }      
    }
}
