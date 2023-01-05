using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class ScheduleDto
{
    public Guid GroupId { get; init; }
    public int IntervalDays { get; init; }
    public Guid NextRoundId { get; init; }
    
    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["group_id"] = YdbValue.MakeString(GroupId.ToByteArray()),
            ["interval_days"] = YdbValue.MakeInt32(IntervalDays),
            ["next_round_id"] = YdbValue.MakeString(NextRoundId.ToByteArray())
        };
    }
    
    public static ScheduleDto FromYdbRow(ResultSet.Row row)
    {
        return new ScheduleDto
        {
            GroupId = row["group_id"].GetNonNullGuid(),
            IntervalDays = row["name"].GetNonNullInt32(),
            NextRoundId = row["next_round_id"].GetNonNullGuid()
        };
    }
}