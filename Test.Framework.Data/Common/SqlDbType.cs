namespace Test.Framework.Data
{
    public enum SqlDbType
    {
        [CustomDescription("MySql Db","MySql")]
        MySql = 1,
        [CustomDescription("SqlServer Db","SqlServer")]
        SqlServer = 2,
        [CustomDescription("Oracle Db","Oracle")]
        Oracle = 3,
        [CustomDescription("Sqlite Db","Sqlite")]
        SqlLite = 4,
        [CustomDescription("PostGreSql Db","Postgresql")]
        PostGreSql = 5
    }
}
