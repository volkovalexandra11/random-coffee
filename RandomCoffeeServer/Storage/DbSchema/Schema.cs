using static Ydb.Type.Types;

namespace RandomCoffeeServer.Storage.DbSchema;

public static class Schema
{
    public static readonly YdbTable[] Tables = new[]
    {
        new YdbTable
        {
            TableName = "users",
            Columns = new YdbColumn[]
            {
                new("user_id", PrimitiveTypeId.String),
                new("email", PrimitiveTypeId.Utf8),
                new("first_name", PrimitiveTypeId.Utf8),
                new("last_name", PrimitiveTypeId.Utf8),
                new("profile_picture_url", PrimitiveTypeId.Utf8)
            },
            PrimaryKeyColumns = new[] { 0 },
            Indexes = new[]
            {
                new YdbIndex
                {
                    IndexName = "users_by_email",
                    IndexColumns = new[] { 1 }
                }
            }
        },
        new YdbTable
        {
            TableName = "groups",
            Columns = new YdbColumn[]
            {
                new("group_id", PrimitiveTypeId.String),
                new("admin_user_id", PrimitiveTypeId.String),
                new("name", PrimitiveTypeId.Utf8),
                new("tag", PrimitiveTypeId.Utf8),
                new("is_private", PrimitiveTypeId.Int32),
                new("group_picture_url", PrimitiveTypeId.Utf8),
                new("description", PrimitiveTypeId.Utf8)
            },
            PrimaryKeyColumns = new[] { 0 }
        },
        new YdbTable
        {
            TableName = "groups_users",
            Columns = new YdbColumn[]
            {
                new("group_id", PrimitiveTypeId.String),
                new("user_id", PrimitiveTypeId.String)
            },
            PrimaryKeyColumns = new[] { 0, 1 },
            Indexes = new[]
            {
                new YdbIndex
                {
                    IndexName = "groups_users_by_user_id",
                    IndexColumns = new[] { 1 }
                }
            }
        },
        new YdbTable
        {
            TableName = "user_logins_asp",
            Columns = new YdbColumn[]
            {
                new("login_provider", PrimitiveTypeId.Utf8),
                new("provider_key", PrimitiveTypeId.Utf8),
                new("provider_display_name", PrimitiveTypeId.Utf8),
                new("user_id", PrimitiveTypeId.String),
            },
            PrimaryKeyColumns = new[] { 0, 1 },
            Indexes = new[]
            {
                new YdbIndex
                {
                    IndexName = "user_logins_asp_by_user_id",
                    IndexColumns = new[] { 3 }
                }
            }
        },
        new YdbTable
        {
            TableName = "user_infos_asp",
            Columns = new YdbColumn[]
            {
                new("user_id", PrimitiveTypeId.String),
                new("username", PrimitiveTypeId.Utf8),
                new("normalized_username", PrimitiveTypeId.Utf8),
            },
            PrimaryKeyColumns = new[] { 0 },
            Indexes = new[]
            {
                new YdbIndex()
                {
                    IndexName = "user_infos_asp_by_normalized_username",
                    IndexColumns = new[] { 2 }
                }
            }
        },
        new YdbTable
        {
            TableName = "data_protection_keys_asp",
            Columns = new YdbColumn[]
            {
                new("element_id", PrimitiveTypeId.String),
                new("friendly_name", PrimitiveTypeId.Utf8),
                new("xml", PrimitiveTypeId.Utf8)
            },
            PrimaryKeyColumns = new[] { 0 }
        },
        new YdbTable
        {
            TableName = "schedules",
            Columns = new YdbColumn[]
            {
                new("group_id", PrimitiveTypeId.String),
                new("interval_days", PrimitiveTypeId.Int32),
                new("next_round_id", PrimitiveTypeId.String),
            },
            PrimaryKeyColumns = new[] { 0 }
        },
        new YdbTable
        {
            TableName = "rounds",
            Columns = new YdbColumn[]
            {
                new("round_id", PrimitiveTypeId.String),
                new("group_id", PrimitiveTypeId.String),
                new("date", PrimitiveTypeId.Date),
                new("was_notified_about", PrimitiveTypeId.Int32)
            },
            PrimaryKeyColumns = new[] { 0 },
            Indexes = new[]
            {
                new YdbIndex
                {
                    IndexName = "rounds_by_date",
                    IndexColumns = new[] { 3, 2 }
                }
            }
        },
        new YdbTable
        {
            TableName = "user_rounds",
            Columns = new YdbColumn[]
            {
                new("user_id", PrimitiveTypeId.String),
                new("round_id", PrimitiveTypeId.String),
                new("match_user_id", PrimitiveTypeId.String)
            },
            PrimaryKeyColumns = new[] { 0, 1 },
            Indexes = new[]
            {
                new YdbIndex
                {
                    IndexName = "user_rounds_by_round_id",
                    IndexColumns = new[] { 1 }
                }
            }
        }
    };

    public static readonly YdbTable Users = GetTable("users");
    public static readonly YdbTable Groups = GetTable("groups");
    public static readonly YdbTable GroupsUsers = GetTable("groups_users");
    public static readonly YdbTable Rounds = GetTable("rounds");
    public static readonly YdbTable UserRounds = GetTable("user_rounds");
    public static readonly YdbTable UserLoginsAsp = GetTable("user_logins_asp");
    public static readonly YdbTable UserInfoAsp = GetTable("user_infos_asp");
    public static readonly YdbTable DataProtectionKeysAsp = GetTable("data_protection_keys_asp");

    private static YdbTable GetTable(string tableName)
    {
        return Tables.Single(table => table.TableName == tableName);
    }
}