INSERT INTO [dbo].[PIMS_COMPENSATION_REQUISITION] ([ACQUISITION_FILE_ID], [CHART_OF_ACCOUNTS_ID], [RESPONSIBILITY_ID], [YEARLY_FINANCIAL_ID], [IS_DRAFT], [FISCAL_YEAR], [AGREEMENT_DT], [GENERATION_DT], [SPECIAL_INSTRUCTION], [DETAILED_REMARKS], [GST_NUMBER], [IS_PAYMENT_IN_TRUST])
VALUES
  ( 1, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-341', CONVERT([bit],(0))),
  ( 2, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-342', CONVERT([bit],(0))),
  ( 3, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-343', CONVERT([bit],(0))),
  ( 5, NULL, NULL, NULL, CONVERT([bit],(0)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-344', CONVERT([bit],(0))),
  ( 7, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-345', CONVERT([bit],(0))),
  ( 9, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-346', CONVERT([bit],(0))),
  (11, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-347', CONVERT([bit],(0))),
  ( 7, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-345', CONVERT([bit],(0))),
  ( 9, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-346', CONVERT([bit],(0))),
  (11, NULL, NULL, NULL, CONVERT([bit],(1)), NULL, NULL, NULL, N'This is a special instruction.', N'This is a detailed remark.', N'12-347', CONVERT([bit],(0)));
GO
