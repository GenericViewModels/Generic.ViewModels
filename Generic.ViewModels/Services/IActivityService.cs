using System.Collections.Generic;

namespace GenericViewModels.Services
{
    public interface IActivityService
    {
        void TrackEvent(string message, Dictionary<string, string> data);
    }
}
