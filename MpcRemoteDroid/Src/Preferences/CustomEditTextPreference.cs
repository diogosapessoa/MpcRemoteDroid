using Android.Content;
using Android.Preferences;
using Android.Util;

namespace MpcRemoteDroid.Src.Preferences
{
    public class CustomEditTextPreference : EditTextPreference
    {
        public CustomEditTextPreference(Context context, IAttributeSet attrs) : base(context, attrs) { }

        protected override void OnDialogClosed(bool positiveResult)
        {
            base.OnDialogClosed(positiveResult);
            if (!string.IsNullOrEmpty(Text))
            {
                Summary = Text;
            }
        }
    }
}