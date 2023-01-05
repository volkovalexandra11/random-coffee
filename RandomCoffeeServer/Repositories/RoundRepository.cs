using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Repositories;

public class RoundRepository : RepositoryBase
{
    public RoundRepository(YdbService ydb)
        : base(ydb, "rounds")
    {
    }

    public async Task AddRound(RoundDto round)
    {
        var @params = YdbConverter.ToDataParams(round.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO rounds SELECT * FROM AS_TABLE($data);",
            @params);
    }

    public async Task<RoundDto?> FindRoundByGroupId(Guid roundId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM rounds WHERE round_id = $id;",
            new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(roundId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? RoundDto.FromYdbRow(resultSet.Rows[0]) : null;
    }
}