using MelonLoader.Preferences;

namespace ItrsTweaks
{
    public class LargerThan0ValueValidator : ValueValidator
    {
        public override object EnsureValid(object value)
        {
            switch (value)
            {
                case float f:
                    if (f < 0.01f) return 0.01f;
                    break;
                case int i:
                    if (i <= 0) return 1;
                    break;
            }
            return value;
        }

        public override bool IsValid(object value)
        {
            switch (value)
            {
                case float f:
                    if (f < 0.01f) return false;
                    break;
                case int i:
                    if (i <= 0) return false;
                    break;
            }

            return true;
        }
    }
}
