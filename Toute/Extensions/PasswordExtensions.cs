using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Toute
{
    /// <summary>
    /// Password Extensions
    /// </summary>
    public static class PasswordExtensions
    {
        /// <summary>
        /// Extensions that will unsecure SecureString
        ///     NOTE: Should not be used to store password
        ///           in variables. Should be only used
        ///           only explicitly in methods as parameter
        /// </summary>
        /// <param name="secureString">SecureString to unsecure</param>
        /// <returns>Unsecured password</returns>
        public static string Unsecure(this SecureString secureString)
        {
            //Make a new value Pointer, and set it to Zero
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                //Decode the SecureString
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                //returns unsecured SecureString
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                //Free secureString from memory
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }

        }
    }
}
