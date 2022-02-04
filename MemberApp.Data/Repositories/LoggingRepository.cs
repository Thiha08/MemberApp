using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;

namespace MemberApp.Data.Repositories
{
    public class LoggingRepository : EntityBaseRepository<Error>, ILoggingRepository
    {
        public LoggingRepository(MemberAppContext context)
            : base(context)
        { }

        public override void Commit()
        {
            try
            {
                base.Commit();
            }
            catch { }
        }
    }
}
