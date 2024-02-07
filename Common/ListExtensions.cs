using BookStore.API.Data;
using BookStore.API.DataModels;
using BookStore.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using static BookStore.API.Common.Enumerations;

namespace BookStore.API.Common
{
    public static class ListExtensions
    {
        //Apply pagination extension
        public static List<T> ApplyPagination<T>(this List<T> input, ListRequestModel requestModel)
        {
            var pagination = requestModel.Pagination is null ? new PaginationModel() : requestModel.Pagination;
            return input.Skip((pagination.PageNo - 1) * pagination.PageSize)
                        .Take(pagination.PageSize).ToList();
        }

        //Get Property value
        private static object? GetPropertyValue(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo?.GetValue(obj) ?? null;
        }

        //Get Property Type
        private static Type? GetPropertyType(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo?.PropertyType ?? null;
        }

        //Apply filter extension
        public static List<T> ApplyListBook<T>(this List<T> input, FilterModel filterModel, BaseTypes baseType)

        {


            switch (baseType)
            {
                case BaseTypes.String:
                    var stringValue = Convert.ToString(filterModel.Values[0] ?? "0");
                    switch (filterModel.Operator)
                    {
                        case Operators.EqualTo:
                            return input.Where(x => Convert.ToString(GetPropertyValue(x, filterModel.Column.ToLower())) == stringValue.ToLower()).ToList();
                        case Operators.Contains:
                            return input.Where(x => Convert.ToString(GetPropertyValue(x, filterModel.Column.ToLower())).Contains(stringValue.ToLower())).ToList();
                        default:
                            break;
                    }
                    break;
                case BaseTypes.Number:
                    var intValue = Convert.ToDouble(filterModel.Values[0] ?? "0");
                    var intValue2 = Convert.ToDouble(filterModel.Values.Length < 2 ? "0" : filterModel.Values[1] ?? "0");
                    switch (filterModel.Operator)
                    {
                        case Operators.EqualTo:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) == intValue).ToList();
                        case Operators.NotEqualTo:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) != intValue).ToList();
                        case Operators.GreaterThan:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) > intValue).ToList();
                        case Operators.GreaterThanOrEqual:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) >= intValue).ToList();
                        case Operators.LesserThan:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) < intValue).ToList();
                        case Operators.LessThanOrEqual:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) <= intValue).ToList();
                        case Operators.Between:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) > intValue && Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) < intValue2).ToList();
                        case Operators.BetweenOrEqual:
                            return input.Where(x => Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) >= intValue && Convert.ToInt32(GetPropertyValue(x, filterModel.Column)) <= intValue2).ToList();
                        default:
                            return input;
                    }
                    break;

                case BaseTypes.DateTime:
                    var dateValue = Convert.ToDateTime(filterModel.Values[0] ?? "0");
                    var dateValue2 = Convert.ToDateTime(filterModel.Values.Length < 2 ? "0" : filterModel.Values[1] ?? "0");
                    switch (filterModel.Operator)
                    {
                        case Operators.EqualTo:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) == dateValue).ToList();
                        case Operators.NotEqualTo:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) != dateValue).ToList();
                        case Operators.GreaterThan:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) > dateValue).ToList();
                        case Operators.GreaterThanOrEqual:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) >= dateValue).ToList();
                        case Operators.LesserThan:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) < dateValue).ToList();
                        case Operators.LessThanOrEqual:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) <= dateValue).ToList();
                        case Operators.Between:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) > dateValue && Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) < dateValue2).ToList();
                        case Operators.BetweenOrEqual:
                            return input.Where(x => Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) >= dateValue && Convert.ToDateTime(GetPropertyValue(x, filterModel.Column)) <= dateValue2).ToList();
                        default:
                            return input;
                    }
                    break;
                default:
                    break;
            }

            return input;

        }

        //Generic filter for fixed field dynamic operator and search content
        //public static List<Books> ApplyFilters(List<Books> books, FilterModel filterModel)
        //{
        //    var intValue = Convert.ToInt32(filterModel.Values[0] ?? "0");
        //    var intValue2 = Convert.ToInt32(filterModel.Values[1] ?? "0");

        //    switch (filterModel.Operator)
        //    {
        //        case Operators.EqualTo:
        //            return books.Where(x => x.Id == intValue).ToList();
        //        case Operators.Contains:
        //            return books.Where(x => x.Id.ToString().Contains(filterModel.Values[0] ?? "0")).ToList();
        //        case Operators.NotEqualTo:
        //            return books.Where(x => x.Id != intValue).ToList();
        //        case Operators.GreaterThanOrEqual:
        //            return books.Where(x => x.Id >= intValue).ToList();
        //        case Operators.GreaterThan:
        //            return books.Where(x => x.Id > intValue).ToList();
        //        case Operators.LessThanOrEqual:
        //            return books.Where(x => x.Id <= intValue).ToList();
        //        case Operators.LesserThan:
        //            return books.Where(x => x.Id < intValue).ToList();
        //        case Operators.Between:
        //            return books.Where(x => x.Id > intValue && x.Id < intValue2).ToList();
        //        case Operators.BetweenOrEqual:
        //            return books.Where(x => x.Id >= intValue && x.Id <= intValue2).ToList();
        //        default:
        //            return books;
        //    }
        //}

        //Fixed Column Dynamic Operator With Multiple Condition
        //public static List<Books> FixedColumnDynamicOperatorWithMultipleCondition(List<Books> books, List<FilterModel> requestFilterList)
        //{


        //    foreach (var item in requestFilterList)
        //    {
        //        books = books.ApplyListBook(item, BaseTypes.Number);
        //    }
        //    return books;
        //}

        //Generic filter for dynamic field fixed operator and search content
        public static List<Books> DynamicColumnDynamicOperatorWithMultipleCondition(List<Books> books, List<FilterModel> requestFilterList)
        {
            //var filterModel = requestFilterList[0];


            foreach (var requestfilterItem in requestFilterList)
            {
                var item = books[0];

                var baseType = new BaseTypes();
                var propertyType = GetPropertyType(item, requestfilterItem.Column);

                if (propertyType == typeof(DateOnly) || propertyType == typeof(DateTime))
                    baseType = BaseTypes.DateTime;
                else if (propertyType == typeof(int) || propertyType == typeof(float) || propertyType == typeof(double))
                    baseType = BaseTypes.Number;
                else
                    baseType = BaseTypes.String;

                books = books.ApplyListBook(requestfilterItem, baseType);
            }

            return books;
        }

        public static List<T> ApplySorting<T>(this List<T> input, ListRequestModel listRequestModel)
        {

            //if (listRequestModel.Sorting.SortOrder)
            //{
            //    return input.OrderBy(b=> GetPropertyValue(b, listRequestModel.Sorting.SortBy)).ToList();
            //}
            //else
            //{
            //    return input.OrderByDescending(b => GetPropertyValue(b, listRequestModel.Sorting.SortBy)).ToList();
            //}

            if (listRequestModel.Sorting.SortOrder)
                return input.OrderBy(b => GetPropertyValue(b, listRequestModel.Sorting.SortBy)).ToList() ?? [];
            else
                return input.OrderByDescending(b => GetPropertyValue(b, listRequestModel.Sorting.SortBy)).ToList() ?? [];
        }

        //public static UrlModel GetUrl(IHttpContextAccessor httpContextAccessor, int currentPage, int lastPage)
        //{
        //    var httpRequest = httpContextAccessor.HttpContext.Request;
        //    var host = httpRequest.Host.Value;
        //    var scheme = httpRequest.Scheme;
        //    var path = httpRequest.Path;


        //    var queryString = httpRequest.Query;
        //    var queryDict = queryString.ToDictionary(
        //                    kvp => kvp.Key,
        //                    kvp => kvp.Value.ToString()
        //                     );

        //    var previousPageDict = new Dictionary<string, string>(queryDict);
        //    var nextPageDict = new Dictionary<string, string>(queryDict);

        //    var previousPageNumber = currentPage > 1 ? (int?)currentPage - 1 : null;
        //    var nextPageNumber = currentPage < lastPage ? (int?)currentPage + 1 : null;

        //    if (previousPageDict.ContainsKey("Pagination.PageNo"))
        //        previousPageDict["Pagination.PageNo"] = previousPageNumber.ToString();
        //    if (nextPageDict.ContainsKey("Pagination.PageNo"))
        //        nextPageDict["Pagination.PageNo"] = nextPageNumber.ToString();

        //    var currentPageQueryString = QueryHelpers.AddQueryString("", queryDict);
        //    var previousPageQueryString = QueryHelpers.AddQueryString("", previousPageDict);
        //    var nextPageQueryString = QueryHelpers.AddQueryString("", nextPageDict);

        //    var currentUrl = $"{scheme}://{host}{path}{currentPageQueryString}";
        //    var previousPageUrl = previousPageNumber.HasValue ? $"{scheme}://{host}{path}{previousPageQueryString}" : null;
        //    var nextPageUrl =nextPageNumber.HasValue? $"{scheme}://{host}{path}{nextPageQueryString}":null;

        //    return new UrlModel
        //    {
        //        CurrentUrl = currentUrl,
        //        NextUrl = nextPageUrl,
        //        PreviousUrl = previousPageUrl
        //    };
        //}

        public static string GenerateUrl (IHttpContextAccessor httpContextAccessor, int Page)
        {
            var Url =  httpContextAccessor.HttpContext.Request.GetDisplayUrl()
                .Replace(httpContextAccessor.HttpContext.Request.QueryString.ToString(),string.Empty);
            var queryDictionary = httpContextAccessor.HttpContext.Request.Query
                .ToDictionary(x => x.Key, x => x.Value.ToString());
            if(queryDictionary.ContainsKey("Pagination.PageNo"))
                queryDictionary["Pagination.PageNo"] = Page.ToString();
            return Uri.UnescapeDataString($"{Url}{new QueryBuilder(queryDictionary).ToQueryString()}");
        }

    }
}
