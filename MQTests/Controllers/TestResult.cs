using System.Collections.Generic;

namespace GeoSearch.Controllers
{
    public class TestResult
    {
        public ICollection<string> Items { get; private set; } = new List<string>();
    }
}
