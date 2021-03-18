using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PublicAddressBook.Client.Helpers
{
    public static class HeaderParser
    {
        public static PagingInfo FindAndParsePagingInfo(System.Net.Http.Headers.HttpResponseHeaders responseHeaders)
        {
            IEnumerable<string> paginationHeaderValues;
            responseHeaders.TryGetValues("X-Pagination", out paginationHeaderValues);
            if (paginationHeaderValues.Any())
                return JsonConvert.DeserializeObject<PagingInfo>(paginationHeaderValues.FirstOrDefault());
            else
                return null;
        }
    }
}