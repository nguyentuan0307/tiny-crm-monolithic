using Microsoft.AspNetCore.Mvc.Filters;
using TinyCRM.API.Exceptions;

namespace TinyCRM.API.Helper.Filters
{
    public class SortFilterAttributeQuery : ActionFilterAttribute
    {
        public string Filters { get; set; } = null!;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var values = context.HttpContext.Request.Query["Sorting"].ToString().ToLower();
            if (string.IsNullOrWhiteSpace(values)) return;
            var filterArray = Filters.ToLower().Split(new[] { ',' }, StringSplitOptions.TrimEntries);

            var value = values.Split(new[] { ',' }, StringSplitOptions.TrimEntries);
            foreach (var item in value)
            {
                var sort = item.Split(new[] { ' ' }, StringSplitOptions.TrimEntries);
                if (!filterArray.Contains(sort[0]))
                {
                    throw new BadRequestHttpException("Sort field does not exist");
                }
                if (sort.Length > 1 && !sort[1].Equals("asc") && !sort[1].Equals("desc"))
                {
                    throw new BadRequestHttpException("Invalid sort direction");
                }
            }
        }
    }
}