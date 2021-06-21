using System.Collections.Generic;

namespace MQGeoSearch.Controllers
{
    public class TestResult
    {
        public ICollection<string> Items { get; private set; }

        public TestResult(ICollection<string> items)
        {
            Items = items;
        }
    }
}
