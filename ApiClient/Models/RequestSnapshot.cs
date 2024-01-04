//-----------------------------------------------------------------------
//
// THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTIES OF ANY KIND, EXPRESS, IMPLIED, STATUTORY, 
// OR OTHERWISE. EXPECT TO THE EXTENT PROHIBITED BY APPLICABLE LAW, DIGI-KEY DISCLAIMS ALL WARRANTIES, 
// INCLUDING, WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, 
// SATISFACTORY QUALITY, TITLE, NON-INFRINGEMENT, QUIET ENJOYMENT, 
// AND WARRANTIES ARISING OUT OF ANY COURSE OF DEALING OR USAGE OF TRADE. 
// 
// DIGI-KEY DOES NOT WARRANT THAT THE SOFTWARE WILL FUNCTION AS DESCRIBED, 
// WILL BE UNINTERRUPTED OR ERROR-FREE, OR FREE OF HARMFUL COMPONENTS.
// 
//-----------------------------------------------------------------------

namespace ApiClient.Models
{
    public interface IRequestQuerySave<T, T1>
    {
        public void Save(RequestSnapshot requestSnapshot, T database);

        public T1? Convert(RequestSnapshot requestSnapshot);

        public string? Query(string route, string routeParameter, T database, DateTime? afterDate);

        public IQueryable<RequestSnapshot> RequestSnapshots(IQueryable<T1>? table = null);
    }

    public class RequestSnapshot
    {
        public long RequestID { get; set; }

        public string Route { get; set; } = null!;

        public string RouteParameter { get; set; } = null!;

        public string Parameters { get; set; } = null!;

        public string Response { get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
