using Android.Content;
using Android.Widget;

namespace MpcRemoteDroid.Src
{
    public static class AndroidContextExtensions
    {
        public static void ShowToast(this Context context, string message, ToastLength toastLength = ToastLength.Short)
        {
            Toast.MakeText(context, message, toastLength).Show();
        }

        public static void ShowToast(this Context context, int resId, ToastLength toastLength = ToastLength.Short)
        {
            Toast.MakeText(context, resId, toastLength).Show();
        }
    }
}