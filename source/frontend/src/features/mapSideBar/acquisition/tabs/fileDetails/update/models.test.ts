import { UpdateAcquisitionSummaryFormModel } from './models';

describe('UpdateAcquisitionSummaryFormModel', () => {
  describe('toApi noticeOfClaim handling', () => {
    it('omits noticeOfClaim when date and comment are empty', () => {
      const model = new UpdateAcquisitionSummaryFormModel();
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
      const model = new UpdateAcquisitionSummaryFormModel();
      model.noticeOfClaim = {
        id: null,
        acquisitionFileId: null,
        managementFileId: null,
        comment: '  file update comment  ',
        receivedDate: '',
        rowVersion: null,
      };

      const apiModel = model.toApi();

      expect(apiModel.noticeOfClaim).toHaveLength(1);
      expect(apiModel.noticeOfClaim?.[0]).toEqual(
        expect.objectContaining({
          comment: 'file update comment',
          receivedDate: null,
        }),
      );
    });
  });
});
