using System.Collections.Generic;

using Mastercam.Database;

namespace PlaneAngles.Services
{
    public interface ISearchService
    {
        List<MCView> GetViews();
    }
}
