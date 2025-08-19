using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.OS
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTimeOffset OffsetNow => DateTimeOffset.Now;

        public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }

    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        //DateTime UtcNow { get; }

        DateTimeOffset OffsetNow { get; }


        //DateTimeOffset OffsetUtcNow { get; }
    }
}
