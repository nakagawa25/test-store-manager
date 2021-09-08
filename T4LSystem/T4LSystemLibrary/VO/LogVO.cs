using System;

namespace T4LSystemLibrary.VO
{
    public class LogVO : StandardVO
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
    }
}
