using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Repositories;

public class ScheduleRepository : RepositoryBase
{
    public ScheduleRepository(YdbService ydb)
        : base(ydb, "schedules")
    {
    }

    public async Task AddSchedule(ScheduleDto schedule)
    {
        var @params = YdbConverter.ToDataParams(schedule.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO schedules SELECT * FROM AS_TABLE($data);",
            @params);
    }

    public async Task<ScheduleDto?> FindSchedule(Guid groupId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM schedules WHERE group_id = $id;",
            new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(groupId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? ScheduleDto.FromYdbRow(resultSet.Rows[0]) : null;
    }
}