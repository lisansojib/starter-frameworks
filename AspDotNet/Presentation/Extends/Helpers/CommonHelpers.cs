using ApplicationCore.DTOs;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Presentation.Extends.Helpers
{
    public interface ICommonHelpers
    {
        FilterByExpression GetFilterByModel(string value);
        string GetFilterBy(string value);
    }

    public class CommonHelpers : ICommonHelpers
    {
        public string GetFilterBy(string value)
        {
            var filterBy = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                var singleFilter = true;
                dynamic filterObj = JsonConvert.DeserializeObject(value);
                foreach (var item in filterObj)
                {
                    var appendAnd = singleFilter ? "" : " And ";
                    filterBy += $"{appendAnd}{item.Name} like '%{item.Value.Value}%'";
                    singleFilter = false;
                }
            }

            return filterBy;
        }

        public FilterByExpression GetFilterByModel(string value)
        {
            FilterByExpression filterExpressionModel = null;

            if (!string.IsNullOrEmpty(value))
            {
                filterExpressionModel = new FilterByExpression();
                StringBuilder expression = null;
                var singleFilter = true;
                dynamic filterObj = JsonConvert.DeserializeObject(value);
                int i = 0;
                foreach (var item in filterObj)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(item.Value)))
                        continue;

                    var appendAnd = singleFilter ? "" : " AND ";
                    expression = new StringBuilder();
                    expression.Append($"{appendAnd}{item.Name}.ToString().Contains(@{i++})");
                    singleFilter = false;

                    filterExpressionModel.Parameters.Add(Convert.ToString(item.Value));
                }

                filterExpressionModel.Expression = expression.ToString();
            }

            return filterExpressionModel;
        }
    }
}