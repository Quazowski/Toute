using System;

namespace Toute.Core
{
    public class RefreshTokenDataModel
    {
        public virtual string Id{ get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime ExpirationDate { get; set; }
    }
}
