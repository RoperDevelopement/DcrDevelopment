using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public sealed class Users : SerializedObjectDictionary<User>
    {
        public override string DirectoryPath
        { get { return @"Config\Users"; } }

        public User GetByEncodedCardId(string cardId)
        {
            foreach (User user in this.Values)
            { 
                if (user.CardId.Equals(cardId))
                { return user; }
            }
            return null;
        }

        public User EnsureGetByEncodedCardId(string cardId)
        {
            User user = this.GetByEncodedCardId(cardId);
            if (user == null)
            { throw new KeyNotFoundException("Unable to find user with the specified key."); }
            return user;
        }

        static readonly Users _Instance = new Users();
        public static Users Instance
        { get { return _Instance; } }
    }    
}
