//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//using Avanti.SDK.Models.WorkTech;

//namespace Avanti.SDK.Extensions
//{
//    public static class WorkTech
//    {
//        /// <summary>
//        /// Search for WorkTech import records for the given criteria.
//        /// </summary>
//        /// <param name="api">The <see cref="IAvantiApi">IAvantiApi</see> instance.</param>
//        /// <param name="search">Criteria to search for.</param>
//        /// <param name="sort">Property name to sort by.</param>
//        /// <param name="ascending">Indicates sort direction.</param>
//        /// <param name="cancellationToken">Optional cancellation token.</param>
//        /// <returns>List of records that match the criteria.</returns>
//        public static Task<IEnumerable<WorkTechImport>> SearchWorkTechImport(this IAvantiApi api,
//            WorkTechSearch search,
//            string sort = "TransactionNo",
//            bool ascending = false,
//            CancellationToken cancellationToken = default)
//        {
//            string resource =
//                $"/worktech/import/search?search={search.ToQueryString()}&sort={sort}&ascending={ascending}";

//            return api.GetAsync<IEnumerable<WorkTechImport>>(resource, cancellationToken);
//        }

//        /// <summary>
//        /// Search for WorkTech import records for the given criteria with pagination.
//        /// </summary>
//        /// <param name="api">The <see cref="IAvantiApi">IAvantiApi</see> instance.</param>
//        /// <param name="search">Criteria to search for.</param>
//        /// <param name="sort">Property name to sort by.</param>
//        /// <param name="ascending">Indicates sort direction.</param>
//        /// <param name="page">The page number to request.</param>
//        /// <param name="pageSize">The number of records per page.</param>
//        /// <param name="cancellationToken">Optional cancellation token.</param>
//        /// <returns>Page of records that match the criteria.</returns>
//        public static Task<IEnumerable<WorkTechImport>> SearchWorkTechImport(this IAvantiApi api,
//            WorkTechSearch search,
//            string sort = "TransactionNo",
//            bool ascending = false,
//            int page = 1,
//            int pageSize = 25,
//            CancellationToken cancellationToken = default)
//        {
//            string resource =
//                $"/worktech/import/search?search={search.ToQueryString()}&paged=true&sort={sort}&ascending={ascending}&page={page}&pageSize={pageSize}";

//            return api.GetAsync<IEnumerable<WorkTechImport>>(resource, cancellationToken);
//        }
//    }
//}
