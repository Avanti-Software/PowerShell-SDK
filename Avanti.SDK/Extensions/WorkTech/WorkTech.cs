using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.SDK.Models.WorkTech;

namespace Avanti.SDK.Extensions.WorkTech
{
    public static class WorkTech
    {
        /// <summary>
        /// Add a new WorkTech import record.
        /// </summary>
        /// <param name="api">The <see cref="IAvantiApi">IAvantiApi</see> instance.</param>
        /// <param name="record">The record to be added to the database.</param>
        /// <returns>The newly created record with primary key set.</returns>
        public static async Task<WorkTechImport> AddWorkTechImport(this IAvantiApi api, WorkTechImport dto)
        {
            string json = JsonSerializer.Serialize(dto);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = "UTF-8"
            };

            HttpResponseMessage response = await api.PostAsync("/api/worktech/import", content);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<WorkTechImport>(
                await response.Content.ReadAsStreamAsync());
        }

        /// <summary>
        /// Search for WorkTech import records for the given criteria.
        /// </summary>
        /// <param name="api">The <see cref="IAvantiApi">IAvantiApi</see> instance.</param>
        /// <param name="search">Criteria to search for.</param>
        /// <param name="sort">Property name to sort by.</param>
        /// <param name="ascending">Indicates sort direction.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>List of records that match the criteria.</returns>
        public static async Task<WorkTechImport[]> SearchWorkTechImport(this IAvantiApi api,
            WorkTechSearch search,
            string sort = "TransactionNo",
            bool ascending = false)
        {
            string resource =
                $"/worktech/import/search?search={search.ToQueryString()}&sort={sort}&ascending={ascending}";

            HttpResponseMessage response = await api.GetAsync(resource);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<WorkTechImport[]>(
                await response.Content.ReadAsStreamAsync());
        }

        /// <summary>
        /// Search for WorkTech import records for the given criteria with pagination.
        /// </summary>
        /// <param name="api">The <see cref="IAvantiApi">IAvantiApi</see> instance.</param>
        /// <param name="search">Criteria to search for.</param>
        /// <param name="sort">Property name to sort by.</param>
        /// <param name="ascending">Indicates sort direction.</param>
        /// <param name="page">The page number to request.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Page of records that match the criteria.</returns>
        public static async Task<WorkTechImport[]> SearchWorkTechImport(this IAvantiApi api,
            WorkTechSearch search,
            string sort = "TransactionNo",
            bool ascending = false,
            int page = 1,
            int pageSize = 25)
        {
            string resource =
                $"/worktech/import/search?search={search.ToQueryString()}&paged=true&sort={sort}&ascending={ascending}&page={page}&pageSize={pageSize}";

            HttpResponseMessage response = await api.GetAsync(resource);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<WorkTechImport[]>(
                await response.Content.ReadAsStreamAsync());
        }
    }
}
