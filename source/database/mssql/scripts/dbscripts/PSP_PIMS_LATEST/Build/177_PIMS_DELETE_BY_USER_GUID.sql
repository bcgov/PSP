-------------------------------------------------------------------------
-- AI Generated NOT FOR USE IN PRODUCTION
-- Deletes (or dry runs if execute = 0) all tables with APP_CREATE_USER_GUID passed via param
-- Attempts to dynamically delete all child/grandchild entities that were also created with the given APP_CREATE_USER_GUID
-- Ignores PIMS_USER_* tables and any PIMS_PERSON referred to by a PIMS_USER
-- Deletes History tables as well as non-history tables
-------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.SafeDeleteByCreateUserGuidStrict
(
    @AppCreateUserGuid uniqueidentifier,
    @Execute bit = 0,                 -- 0 = preview only (rollback), 1 = execute
    @MaxPasses int = 100,             -- repeated child-first passes
    @ProtectedSchema sysname = N'dbo' -- schema containing PIMS_* tables
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;

    DECLARE @PimsUserFullName nvarchar(517) =
        QUOTENAME(@ProtectedSchema) + N'.' + QUOTENAME(N'PIMS_USER');

    DECLARE @PimsPersonFullName nvarchar(517) =
        QUOTENAME(@ProtectedSchema) + N'.' + QUOTENAME(N'PIMS_PERSON');

    DECLARE @PimsPersonHistFullName nvarchar(517) =
        QUOTENAME(@ProtectedSchema) + N'.' + QUOTENAME(N'PIMS_PERSON_HIST');

    DECLARE @PimsUserObjectId int = OBJECT_ID(@PimsUserFullName);
    DECLARE @PimsPersonObjectId int = OBJECT_ID(@PimsPersonFullName);
    DECLARE @PimsPersonHistObjectId int = OBJECT_ID(@PimsPersonHistFullName);

    -------------------------------------------------------------------------
    -- 0. Validate required protected tables
    -------------------------------------------------------------------------
    IF @PimsUserObjectId IS NULL
        THROW 50000, 'Protected table PIMS_USER was not found in the specified schema.', 1;

    IF @PimsPersonObjectId IS NULL
        THROW 50001, 'Protected table PIMS_PERSON was not found in the specified schema.', 1;

    -------------------------------------------------------------------------
    -- 1. Central ignore list
    --
    --    Any table here is:
    --      - never deleted
    --      - never included in target discovery
    --      - never included in blocker logic
    --      - never included in summary
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#IgnoreTables') IS NOT NULL DROP TABLE #IgnoreTables;
    CREATE TABLE #IgnoreTables
    (
        SchemaName sysname NOT NULL,
        TableName  sysname NOT NULL,
        CONSTRAINT PK_IgnoreTables PRIMARY KEY (SchemaName, TableName)
    );

    INSERT INTO #IgnoreTables (SchemaName, TableName)
    VALUES
        (@ProtectedSchema, N'PIMS_USER'),
        (@ProtectedSchema, N'PIMS_USER_ORGANIZATION'),
        (@ProtectedSchema, N'PIMS_USER_ROLE'),
        (@ProtectedSchema, N'PIMS_USER_TYPE'),
        (@ProtectedSchema, N'PIMS_REGION_USER');

    -------------------------------------------------------------------------
    -- 2. Discover delete-target tables
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#TargetTables') IS NOT NULL DROP TABLE #TargetTables;
    CREATE TABLE #TargetTables
    (
        ObjectId      int           NOT NULL PRIMARY KEY,
        SchemaName    sysname       NOT NULL,
        TableName     sysname       NOT NULL,
        FullName      nvarchar(517) NOT NULL,
        DeleteDepth   int           NOT NULL DEFAULT (0)
    );

    INSERT INTO #TargetTables
    (
        ObjectId,
        SchemaName,
        TableName,
        FullName
    )
    SELECT
        t.object_id,
        s.name,
        t.name,
        QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
    FROM sys.tables t
    JOIN sys.schemas s
        ON s.schema_id = t.schema_id
    JOIN sys.columns c
        ON c.object_id = t.object_id
       AND c.name = N'APP_CREATE_USER_GUID'
    WHERE t.is_ms_shipped = 0
      AND NOT EXISTS
      (
          SELECT 1
          FROM #IgnoreTables i
          WHERE i.SchemaName = s.name
            AND i.TableName = t.name
      );

    -------------------------------------------------------------------------
    -- 3. Build FK metadata among target tables for delete ordering
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#TargetEdges') IS NOT NULL DROP TABLE #TargetEdges;
    CREATE TABLE #TargetEdges
    (
        FKObjectId     int NOT NULL PRIMARY KEY,
        ChildObjectId  int NOT NULL,
        ParentObjectId int NOT NULL
    );

    INSERT INTO #TargetEdges
    (
        FKObjectId,
        ChildObjectId,
        ParentObjectId
    )
    SELECT
        fk.object_id,
        fk.parent_object_id,
        fk.referenced_object_id
    FROM sys.foreign_keys fk
    JOIN #TargetTables child_t
        ON child_t.ObjectId = fk.parent_object_id
    JOIN #TargetTables parent_t
        ON parent_t.ObjectId = fk.referenced_object_id;

    -------------------------------------------------------------------------
    -- 4. Compute child-first delete depth without recursive CTEs
    -------------------------------------------------------------------------
    DECLARE @DepthPass int = 0;
    DECLARE @DepthRowsChanged int = 1;

    UPDATE #TargetTables
    SET DeleteDepth = 0;

    WHILE @DepthRowsChanged > 0
      AND @DepthPass < @MaxPasses
    BEGIN
        SET @DepthPass = @DepthPass + 1;
        SET @DepthRowsChanged = 0;

        ;WITH CandidateDepths AS
        (
            SELECT
                e.ChildObjectId AS ObjectId,
                MAX(p.DeleteDepth + 1) AS ProposedDepth
            FROM #TargetEdges e
            JOIN #TargetTables p
                ON p.ObjectId = e.ParentObjectId
            GROUP BY e.ChildObjectId
        )
        UPDATE tgt
        SET DeleteDepth = c.ProposedDepth
        FROM #TargetTables tgt
        JOIN CandidateDepths c
            ON c.ObjectId = tgt.ObjectId
        WHERE c.ProposedDepth > tgt.DeleteDepth;

        SET @DepthRowsChanged = @@ROWCOUNT;
    END;

    -------------------------------------------------------------------------
    -- 5. Build blocker metadata for every FK that references a target table
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#BlockerEdges') IS NOT NULL DROP TABLE #BlockerEdges;
    CREATE TABLE #BlockerEdges
    (
        FKObjectId        int           NOT NULL PRIMARY KEY,
        ParentObjectId    int           NOT NULL,
        ChildFullName     nvarchar(517) NOT NULL,
        ChildToParentJoin nvarchar(max) NOT NULL
    );

    INSERT INTO #BlockerEdges
    (
        FKObjectId,
        ParentObjectId,
        ChildFullName,
        ChildToParentJoin
    )
    SELECT
        fk.object_id,
        fk.referenced_object_id,
        QUOTENAME(sChild.name) + N'.' + QUOTENAME(tChild.name),
        STUFF
        (
            (
                SELECT
                    N' AND c.' + QUOTENAME(colChild.name) +
                    N' = tgt.' + QUOTENAME(colParent.name)
                FROM sys.foreign_key_columns fkc2
                JOIN sys.columns colChild
                    ON colChild.object_id = fkc2.parent_object_id
                   AND colChild.column_id = fkc2.parent_column_id
                JOIN sys.columns colParent
                    ON colParent.object_id = fkc2.referenced_object_id
                   AND colParent.column_id = fkc2.referenced_column_id
                WHERE fkc2.constraint_object_id = fk.object_id
                ORDER BY fkc2.constraint_column_id
                FOR XML PATH(''), TYPE
            ).value('.', 'nvarchar(max)'),
            1, 5, N''
        )
    FROM sys.foreign_keys fk
    JOIN sys.tables tChild
        ON tChild.object_id = fk.parent_object_id
    JOIN sys.schemas sChild
        ON sChild.schema_id = tChild.schema_id
    JOIN #TargetTables parent_t
        ON parent_t.ObjectId = fk.referenced_object_id
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM #IgnoreTables i
        WHERE i.SchemaName = sChild.name
          AND i.TableName = tChild.name
    );

    -------------------------------------------------------------------------
    -- 6. Generate delete statements
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#GeneratedStatements') IS NOT NULL DROP TABLE #GeneratedStatements;
    CREATE TABLE #GeneratedStatements
    (
        Seq         int IDENTITY(1,1) PRIMARY KEY,
        ObjectId    int           NOT NULL,
        DeleteDepth int           NOT NULL,
        FullName    nvarchar(517) NOT NULL,
        DeleteSql   nvarchar(max) NOT NULL
    );

    DECLARE
        @ObjectId int,
        @FullName nvarchar(517),
        @TableName sysname,
        @SchemaName sysname,
        @ProtectionClause nvarchar(max),
        @BlockerPredicate nvarchar(max),
        @DeleteSql nvarchar(max);

    DECLARE table_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT
            ObjectId,
            FullName,
            TableName,
            SchemaName
        FROM #TargetTables
        ORDER BY DeleteDepth DESC, FullName ASC;

    OPEN table_cursor;
    FETCH NEXT FROM table_cursor INTO @ObjectId, @FullName, @TableName, @SchemaName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @ObjectId = @PimsPersonObjectId
           OR (@PimsPersonHistObjectId IS NOT NULL AND @ObjectId = @PimsPersonHistObjectId)
        BEGIN
            SET @ProtectionClause = N'
  AND NOT EXISTS
      (
          SELECT 1
          FROM ' + @PimsUserFullName + N' pu
          WHERE pu.[PERSON_ID] = tgt.[PERSON_ID]
      )';
        END
        ELSE
        BEGIN
            SET @ProtectionClause = N'';
        END;

        SELECT
            @BlockerPredicate =
                ISNULL
                (
                    (
                        SELECT
                            CHAR(13) + CHAR(10) +
                            N'  AND NOT EXISTS
      (
          SELECT 1
          FROM ' + be.ChildFullName + N' c
          WHERE ' + be.ChildToParentJoin + N'
      )'
                        FROM #BlockerEdges be
                        WHERE be.ParentObjectId = @ObjectId
                        FOR XML PATH(''), TYPE
                    ).value('.', 'nvarchar(max)'),
                    N''
                );

        SET @DeleteSql = N'
DELETE tgt
FROM ' + @FullName + N' tgt
WHERE tgt.[APP_CREATE_USER_GUID] = @AppCreateUserGuid' +
@ProtectionClause +
ISNULL(@BlockerPredicate, N'') + N';

SET @DeletedCount = @@ROWCOUNT;
';

        INSERT INTO #GeneratedStatements
        (
            ObjectId,
            DeleteDepth,
            FullName,
            DeleteSql
        )
        VALUES
        (
            @ObjectId,
            (SELECT DeleteDepth FROM #TargetTables WHERE ObjectId = @ObjectId),
            @FullName,
            @DeleteSql
        );

        FETCH NEXT FROM table_cursor INTO @ObjectId, @FullName, @TableName, @SchemaName;
    END;

    CLOSE table_cursor;
    DEALLOCATE table_cursor;

    -------------------------------------------------------------------------
    -- 7. Logging tables
    -------------------------------------------------------------------------
    IF OBJECT_ID('tempdb..#ExecutionLog') IS NOT NULL DROP TABLE #ExecutionLog;
    CREATE TABLE #ExecutionLog
    (
        Mode         nvarchar(20)   NOT NULL,
        PassNo       int            NOT NULL,
        FullName     nvarchar(517)  NOT NULL,
        DeletedCount int            NOT NULL,
        ErrorNumber  int            NULL,
        ErrorMessage nvarchar(4000) NULL
    );

    IF OBJECT_ID('tempdb..#Summary') IS NOT NULL DROP TABLE #Summary;
    CREATE TABLE #Summary
    (
        FullName         nvarchar(517) NOT NULL,
        MatchingGuidRows int           NOT NULL,
        ProtectedRows    int           NOT NULL,
        BlockedRows      int           NOT NULL,
        DeletableNowRows int           NOT NULL
    );

    -------------------------------------------------------------------------
    -- 8. Execute or preview (preview runs inside a transaction and rolls back)
    -------------------------------------------------------------------------
    DECLARE @PassNo int = 0;
    DECLARE @DeletedThisPass int;
    DECLARE @DeletedCount int;
    DECLARE @ExecSql nvarchar(max);
    DECLARE @RunMode nvarchar(20) = CASE WHEN @Execute = 1 THEN N'EXECUTE' ELSE N'PREVIEW' END;

    BEGIN TRY
        IF @Execute = 0
            BEGIN TRAN PreviewRun;

        WHILE @PassNo < @MaxPasses
        BEGIN
            SET @PassNo = @PassNo + 1;
            SET @DeletedThisPass = 0;

            DECLARE exec_cursor CURSOR LOCAL FAST_FORWARD FOR
                SELECT DeleteSql, FullName
                FROM #GeneratedStatements
                ORDER BY DeleteDepth DESC, Seq ASC;

            OPEN exec_cursor;
            FETCH NEXT FROM exec_cursor INTO @ExecSql, @FullName;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                BEGIN TRY
                    SET @DeletedCount = 0;

                    EXEC sp_executesql
                        @ExecSql,
                        N'@AppCreateUserGuid uniqueidentifier, @DeletedCount int OUTPUT',
                        @AppCreateUserGuid = @AppCreateUserGuid,
                        @DeletedCount = @DeletedCount OUTPUT;

                    INSERT INTO #ExecutionLog
                    (
                        Mode,
                        PassNo,
                        FullName,
                        DeletedCount,
                        ErrorNumber,
                        ErrorMessage
                    )
                    VALUES
                    (
                        @RunMode,
                        @PassNo,
                        @FullName,
                        ISNULL(@DeletedCount, 0),
                        NULL,
                        NULL
                    );

                    SET @DeletedThisPass = @DeletedThisPass + ISNULL(@DeletedCount, 0);
                END TRY
                BEGIN CATCH
                    INSERT INTO #ExecutionLog
                    (
                        Mode,
                        PassNo,
                        FullName,
                        DeletedCount,
                        ErrorNumber,
                        ErrorMessage
                    )
                    VALUES
                    (
                        @RunMode,
                        @PassNo,
                        @FullName,
                        0,
                        ERROR_NUMBER(),
                        ERROR_MESSAGE()
                    );
                END CATCH;

                FETCH NEXT FROM exec_cursor INTO @ExecSql, @FullName;
            END;

            CLOSE exec_cursor;
            DEALLOCATE exec_cursor;

            IF @DeletedThisPass = 0
                BREAK;
        END;

        IF @Execute = 0 AND @@TRANCOUNT > 0
            ROLLBACK TRAN PreviewRun;
    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('local', 'exec_cursor') >= -1
        BEGIN
            CLOSE exec_cursor;
            DEALLOCATE exec_cursor;
        END;

        IF @Execute = 0 AND @@TRANCOUNT > 0
            ROLLBACK TRAN PreviewRun;

        THROW;
    END CATCH;

    -------------------------------------------------------------------------
    -- 9. Build end-of-run summary
    --
    --    After preview rollback or after real execution state.
    -------------------------------------------------------------------------
    DECLARE
        @ProtectedExpr nvarchar(max),
        @BlockedExpr nvarchar(max),
        @SummarySql nvarchar(max);

    DECLARE summary_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT
            ObjectId,
            FullName,
            TableName,
            SchemaName
        FROM #TargetTables
        ORDER BY FullName;

    OPEN summary_cursor;
    FETCH NEXT FROM summary_cursor INTO @ObjectId, @FullName, @TableName, @SchemaName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @ObjectId = @PimsPersonObjectId
           OR (@PimsPersonHistObjectId IS NOT NULL AND @ObjectId = @PimsPersonHistObjectId)
        BEGIN
            SET @ProtectedExpr = N'EXISTS
(
    SELECT 1
    FROM ' + @PimsUserFullName + N' pu
    WHERE pu.[PERSON_ID] = tgt.[PERSON_ID]
)';
        END
        ELSE
        BEGIN
            SET @ProtectedExpr = N'1 = 0';
        END;

        SELECT
            @BlockedExpr =
                N'(1 = 0)' +
                ISNULL
                (
                    (
                        SELECT
                            CHAR(13) + CHAR(10) +
                            N' OR EXISTS
(
    SELECT 1
    FROM ' + be.ChildFullName + N' c
    WHERE ' + be.ChildToParentJoin + N'
)'
                        FROM #BlockerEdges be
                        WHERE be.ParentObjectId = @ObjectId
                        FOR XML PATH(''), TYPE
                    ).value('.', 'nvarchar(max)'),
                    N''
                );

        SET @SummarySql = N'
INSERT INTO #Summary
(
    FullName,
    MatchingGuidRows,
    ProtectedRows,
    BlockedRows,
    DeletableNowRows
)
SELECT
    N''' + REPLACE(@FullName, N'''', N'''''') + N''',
    ISNULL((SELECT COUNT(*) FROM ' + @FullName + N' tgt WHERE tgt.[APP_CREATE_USER_GUID] = @AppCreateUserGuid), 0),
    ISNULL((SELECT COUNT(*) FROM ' + @FullName + N' tgt WHERE tgt.[APP_CREATE_USER_GUID] = @AppCreateUserGuid AND (' + @ProtectedExpr + N')), 0),
    ISNULL((SELECT COUNT(*) FROM ' + @FullName + N' tgt WHERE tgt.[APP_CREATE_USER_GUID] = @AppCreateUserGuid AND NOT (' + @ProtectedExpr + N') AND (' + @BlockedExpr + N')), 0),
    ISNULL((SELECT COUNT(*) FROM ' + @FullName + N' tgt WHERE tgt.[APP_CREATE_USER_GUID] = @AppCreateUserGuid AND NOT (' + @ProtectedExpr + N') AND NOT (' + @BlockedExpr + N')), 0);
';

        EXEC sp_executesql
            @SummarySql,
            N'@AppCreateUserGuid uniqueidentifier',
            @AppCreateUserGuid = @AppCreateUserGuid;

        FETCH NEXT FROM summary_cursor INTO @ObjectId, @FullName, @TableName, @SchemaName;
    END;

    CLOSE summary_cursor;
    DEALLOCATE summary_cursor;

    -------------------------------------------------------------------------
    -- 10. Output
    -------------------------------------------------------------------------
    SELECT
        WarningType = N'DepthCalculationCapReached',
        WarningText = N'Maximum depth propagation passes were reached while computing delete order. Delete ordering may be approximate for cyclic graphs.',
        LimitValue = @MaxPasses
    WHERE @DepthPass = @MaxPasses;

    SELECT
        SchemaName,
        TableName
    FROM #IgnoreTables
    ORDER BY SchemaName, TableName;

    SELECT
        Seq,
        DeleteDepth,
        FullName,
        DeleteSql
    FROM #GeneratedStatements
    ORDER BY DeleteDepth DESC, Seq ASC;

    SELECT
        Mode,
        PassNo,
        FullName,
        DeletedCount,
        ErrorNumber,
        ErrorMessage
    FROM #ExecutionLog
    ORDER BY PassNo, FullName;

    SELECT
        FullName,
        MatchingGuidRows,
        ProtectedRows,
        BlockedRows,
        DeletableNowRows
    FROM #Summary
    WHERE MatchingGuidRows > 0
       OR ProtectedRows > 0
       OR BlockedRows > 0
       OR DeletableNowRows > 0
    ORDER BY FullName;
END;
GO