using System.Security;

namespace Toute
{
    public interface IHavePassword
    {
        public SecureString SecureString { get; }
    }
}
