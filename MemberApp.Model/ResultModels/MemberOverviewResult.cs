using MemberApp.Model.Constants;

namespace MemberApp.Model.ResultModels
{
    public class MemberOverviewResult
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public ServiceStatus ServiceStatus { get; set; }
    }
}
