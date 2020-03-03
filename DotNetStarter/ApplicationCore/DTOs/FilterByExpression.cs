using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class FilterByExpression
    {
        public FilterByExpression()
        {
            Parameters = new List<string>();
        }
        public string Expression { get; set; }
        public List<string> Parameters { get; set; }
    }
}
