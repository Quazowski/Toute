using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Toute
{
    public static class PasswordExtensions
    {
        public static string Unsecure(this SecureString secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
            
        }
    }
}
