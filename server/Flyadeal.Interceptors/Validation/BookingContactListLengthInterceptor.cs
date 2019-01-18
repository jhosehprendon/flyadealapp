namespace Flyadeal.Interceptors.Validation
{
    public class BookingContactListLengthInterceptor : ArrayLengthInterceptor
    {
        public BookingContactListLengthInterceptor() : base(1, 1)
        {
        }
    }
}
