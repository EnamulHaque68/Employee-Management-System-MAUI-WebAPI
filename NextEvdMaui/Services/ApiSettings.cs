using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextEvdMaui.Services
{
    public class ApiSettings
    {
        private const string AndroidEmulatorBaseAddress = "http://10.0.2.2:5295/";
        private const string WindowsBaseAddress = "http://localhost:5295/";

        public static string BaseAddress
        {
            get
            {
#if ANDROID
                return AndroidEmulatorBaseAddress;
#elif WINDOWS
                return WindowsBaseAddress;
#else
                throw new PlatformNotSupportedException("This app supports Android and Windows only.");
#endif
            }
        }

        public static string GetImageUrl(string relativePath)
        {
            return $"{BaseAddress.TrimEnd('/')}/{relativePath.TrimStart('/')}";
        }
    }
}