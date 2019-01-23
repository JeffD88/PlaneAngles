using System.Collections.Generic;

using Mastercam.Support;
using Mastercam.Database;

namespace PlaneAngles.Services
{
    class SearchService : ISearchService
    {
        public List<MCView> GetViews()
        {
            return new List<MCView>(SearchManager.GetViews());
        }
    }
}
