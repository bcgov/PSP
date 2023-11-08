import { AcquisitionTeamFormModel } from './models';

describe('acquisition model tests', () => {
  it('omits the primary contact if unset', () => {
    let model = new AcquisitionTeamFormModel('testType');
    model.primaryContactId = '';
    expect(model.toApi(1)?.primaryContactId).toBeUndefined();
  });

  it('omits the primary contact if null', () => {
    let model = new AcquisitionTeamFormModel('testType');
    model.primaryContactId = null as any;
    expect(model.toApi(1)?.primaryContactId).toBeUndefined();
  });

  it('omits the primary contact if undefined', () => {
    let model = new AcquisitionTeamFormModel('testType');
    model.primaryContactId = undefined as any;
    expect(model.toApi(1)?.primaryContactId).toBeUndefined();
  });
});
