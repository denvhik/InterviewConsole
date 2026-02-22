namespace EmployeeService
{
    public static class Constants
    {
        public const string SqlGetEmployeeSubtree = @"
                                WITH EmpTree AS (
                                SELECT e.ID, e.Name, e.ManagerID, e.Enable
                                FROM dbo.Employee e
                                WHERE e.ID = @id

                                UNION ALL

                                SELECT c.ID, c.Name, c.ManagerID, c.Enable
                                FROM dbo.Employee c
                                INNER JOIN EmpTree p ON c.ManagerID = p.ID
                                )
                                SELECT ID, Name, ManagerID, Enable
                                FROM EmpTree
                                OPTION (MAXRECURSION 20);";

        public const string SqlEmployeeEnabled = @"
                                UPDATE dbo.Employee
                                SET Enable = @enable
                                WHERE ID = @id;";
    }
}
