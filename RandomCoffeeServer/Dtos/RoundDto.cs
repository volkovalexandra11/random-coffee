using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class RoundDto
{
    public Guid RoundId { get; init; }
    public Guid GroupId { get; init; }
    public DateTime Date { get; init; }
    public bool WasNotifiedAbout { get; init; }
    
    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["round_id"] = YdbValue.MakeString(RoundId.ToByteArray()),
            ["group_id"] = YdbValue.MakeString(GroupId.ToByteArray()),
            ["date"] = YdbValue.MakeDate(Date),
            ["was_notified_about"] = YdbValue.MakeInt32(WasNotifiedAbout ? 1 : 0)
        };
    }

    public static RoundDto FromYdbRow(ResultSet.Row row)
    {
        return new RoundDto
        {
            RoundId = row["round_id"].GetNonNullGuid(),
            GroupId = row["group_id"].GetNonNullGuid(),
            Date = row["date"].GetNonNullDatetime(),
            WasNotifiedAbout = row["was_notified_about"].GetNonNullBool()
        };
    }
}