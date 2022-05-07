namespace PaymentGateway.Core.Domain.Common;

    public static class Guard
    {
        public static void Against<TException>(bool assertion, string message)
            where TException : Exception
            =>  Against<TException>(assertion, message, innerException: null);
        
        public static void Against<TException>(bool assertion, string messageFormat, params object[] args)
           where TException : Exception
            => Against<TException>(assertion, string.Format(messageFormat, args), innerException: null);
        
        public static void Against<TException>(bool assertion, string message, Exception innerException)
            where TException : Exception
        {
            if (assertion == false)
                return;

            throw (TException)typeof(TException)
                .GetConstructor(new Type[] { typeof(string), typeof(Exception) })
                .Invoke(new object[] { message, innerException });
        }
        
        public static void Against<TException>(bool assertion, Exception innerException, string messageFormat, params object[] args)
            where TException : Exception
            => Against<TException>(assertion, string.Format(messageFormat, args), innerException);
    }