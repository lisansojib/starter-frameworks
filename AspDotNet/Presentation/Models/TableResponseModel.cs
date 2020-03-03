using System.Collections.Generic;

namespace Presentation.Models
{
    public class TableResponseModel
    {
        public TableResponseModel()
        {
        }

        public TableResponseModel(int t, IEnumerable<object> r)
        {
            total = t;
            rows = r;
        }

        public int total { get; set; }
        public IEnumerable<object> rows { get; set; }
    }
}