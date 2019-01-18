namespace Flyadeal.Interceptors.Validation
{
    public class BookingContactNamesLengthInterceptor : ArrayLengthInterceptor
    {
        public BookingContactNamesLengthInterceptor() : base(1, 1)
        {
        }
    }
}
