using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextEvdMaui.Services
{
    public sealed class ApiResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ApiResult Ok()
        {
            return new ApiResult { Success = true };
        }

        public static ApiResult Fail(string message)
        {
            return new ApiResult
            {
                Success = false,
                ErrorMessage = message
            };
        }
    }
}