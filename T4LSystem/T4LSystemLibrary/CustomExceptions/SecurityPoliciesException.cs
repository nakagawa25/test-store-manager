using System;

namespace T4LSystemLibrary.CustomExceptions
{
    public class SecurityPoliciesException : Exception
    {
        public SecurityPoliciesException(string message) : base(message) { }
    }
}
