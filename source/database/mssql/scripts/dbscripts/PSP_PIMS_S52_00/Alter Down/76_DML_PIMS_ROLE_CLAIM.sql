DECLARE @acqCompRequisitionView BIGINT;
DECLARE @acqCompRequisitionAdd BIGINT;
DECLARE @acqCompRequisitionEdit BIGINT;
DECLARE @acqCompRequisitionDelete BIGINT;
SELECT @acqCompRequisitionView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-view';
SELECT @acqCompRequisitionAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-add';
SELECT @acqCompRequisitionEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-edit';
SELECT @acqCompRequisitionDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-delete';



DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @acqCompRequisitionView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @acqCompRequisitionEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @acqCompRequisitionAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @acqCompRequisitionDelete;

GO
