using GenericViewModels.Services;
using System.Collections.Generic;

namespace GenericViewModels.Extensions
{
    public static class ActivityServiceExtensions
    {
        public static void TrackEvent(this IActivityService activity, string message)
            => activity.TrackEvent(message, null);

        public static void TrackEvent(this IActivityService activity, string message, string key, string value)
        {
            var dict = new Dictionary<string, string> { [$"{key}"] = value };
            activity.TrackEvent(message, dict);
        }
    }
}
