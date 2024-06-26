INSERT INTO [dbo].[PIMS_COMPENSATION_REQUISITION] ([ACQUISITION_FILE_ID], [ACQUISITION_OWNER_ID], [INTEREST_HOLDER_ID], [ACQUISITION_FILE_TEAM_ID], [CHART_OF_ACCOUNTS_ID], [RESPONSIBILITY_ID], [YEARLY_FINANCIAL_ID], [IS_DRAFT], [FISCAL_YEAR], [AGREEMENT_DT], [EXPROP_NOTICE_SERVED_DT], [EXPROP_VESTING_DT], [GENERATION_DT], [SPECIAL_INSTRUCTION], [DETAILED_REMARKS], [GST_NUMBER], [IS_PAYMENT_IN_TRUST])
VALUES
  ( 1,    1, NULL, NULL, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-341', CONVERT([bit],(0))),
  ( 2,    2, NULL, NULL, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-342', CONVERT([bit],(0))),
  ( 3,    3, NULL, NULL, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-343', CONVERT([bit],(0))),
  ( 5, NULL,    1, NULL, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-344', CONVERT([bit],(0))),
  ( 7, NULL,    2, NULL, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-345', CONVERT([bit],(0))),
  ( 9, NULL,    3, NULL, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-346', CONVERT([bit],(0))),
  (11, NULL,    4, NULL, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-347', CONVERT([bit],(0))),
  ( 7, NULL, NULL,    1, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-345', CONVERT([bit],(0))),
  ( 9, NULL, NULL,    1, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-346', CONVERT([bit],(0))),
  (11, NULL, NULL,    1, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-347', CONVERT([bit],(0)));
GO
