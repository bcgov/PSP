import { ManagementFormModel } from './ManagementFormModel';

describe('ManagementFormModel', () => {
  describe('toApi noticeOfClaim handling', () => {
    it('omits noticeOfClaim when date and comment are empty', () => {
      const model = new ManagementFormModel();
      model.noticeOfClaim = {
        id: null,
        acquisitionFileId: null,
        managementFileId: null,
        comment: '',
        receivedDate: '',
        rowVersion: null,
      };

      const apiModel = model.toApi();

      expect(apiModel.noticeOfClaim).toEqual([]);
    });

    it('normalizes empty date to null and trims comment', () => {
      const model = new ManagementFormModel();
      model.noticeOfClaim = {
        id: null,
        acquisitionFileId: null,
        managementFileId: null,
        comment: '  comment from formik  ',
        receivedDate: '',
        rowVersion: null,
      };

      const apiModel = model.toApi();

      expect(apiModel.noticeOfClaim).toHaveLength(1);
      expect(apiModel.noticeOfClaim?.[0]).toEqual(
        expect.objectContaining({
          comment: 'comment from formik',
          receivedDate: null,
        }),
      );
    });

    it('keeps receivedDate when provided and normalizes empty comment', () => {
      const model = new ManagementFormModel();
      model.noticeOfClaim = {
        id: null,
        acquisitionFileId: null,
        managementFileId: null,
        comment: '   ',
        receivedDate: '2026-04-17',
        rowVersion: null,
      };

      const apiModel = model.toApi();

      expect(apiModel.noticeOfClaim).toHaveLength(1);
      expect(apiModel.noticeOfClaim?.[0]).toEqual(
        expect.objectContaining({
          comment: null,
          receivedDate: '2026-04-17',
        }),
      );
    });
  });
});
