import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { PayeeType } from '@/features/mapSideBar/acquisition/models/PayeeTypeModel';
import { getMockApiAcquisitionFileOwnerPerson } from '@/mocks/acquisitionFiles.mock';
import { getMockApiInterestHolderPerson } from '@/mocks/interestHolders.mock';

import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ExpropriationEventFormModel, ExpropriationEventRow } from './models';

describe('Expropriation event history model tests', () => {
  describe('ExpropriationEventFormModel', () => {
    it('createEmpty returns a minimal object', () => {
      const model = ExpropriationEventFormModel.createEmpty(1);
      expect(model).toBeDefined();
      expect(model.id).toBeNull();
      expect(model.acquisitionFileId).toBe(1);
      expect(model.payeeKey).toBe('');
      expect(model.rowVersion).toBe(0);
      expect(model.eventDate).toBeNull();
      expect(model.eventTypeCode).toBeNull();
    });

    it('toApi converts form values to the api format - acquisition owner', () => {
      const apiOwner = getMockApiAcquisitionFileOwnerPerson();
      const payeeOptions: PayeeOption[] = [PayeeOption.createOwner(apiOwner, null)];

      const model = ExpropriationEventFormModel.createEmpty(1);
      model.id = 100;
      model.eventTypeCode = 'MOCK-TYPECODE';
      model.rowVersion = 1;
      model.acquisitionOwnerId = apiOwner.id;
      model.payeeKey = PayeeOption.generateKey(apiOwner.id, PayeeType.Owner);

      const apiExpropriation = model.toApi(payeeOptions);
      expect(apiExpropriation.id).toBe(100);
      expect(apiExpropriation.eventType).toEqual(
        expect.objectContaining<Partial<ApiGen_Base_CodeType<string>>>({ id: 'MOCK-TYPECODE' }),
      );
      expect(apiExpropriation.acquisitionOwnerId).toBe(apiOwner.id);
    });

    it('toApi converts form values to the api format - interest holder', () => {
      const apiInterestHolder = getMockApiInterestHolderPerson();
      const payeeOptions: PayeeOption[] = [
        PayeeOption.createInterestHolder(apiInterestHolder, null),
      ];

      const model = ExpropriationEventFormModel.createEmpty(1);
      model.id = 100;
      model.eventTypeCode = 'MOCK-TYPECODE';
      model.rowVersion = 1;
      model.interestHolderId = apiInterestHolder.interestHolderId;
      model.payeeKey = PayeeOption.generateKey(
        apiInterestHolder.interestHolderId,
        PayeeType.InterestHolder,
      );

      const apiExpropriation = model.toApi(payeeOptions);
      expect(apiExpropriation.id).toBe(100);
      expect(apiExpropriation.eventType).toEqual(
        expect.objectContaining<Partial<ApiGen_Base_CodeType<string>>>({ id: 'MOCK-TYPECODE' }),
      );
      expect(apiExpropriation.interestHolderId).toBe(apiInterestHolder.interestHolderId);
    });

    it('fromApi creates form values from api format', () => {
      const model = ExpropriationEventFormModel.fromApi({
        ...getMockExpropriationEvent(99, 1),
        eventDate: '2020-11-25',
      });

      expect(model).toBeDefined();
      expect(model.id).toBe(99);
      expect(model.acquisitionFileId).toBe(1);
      expect(model.eventDate).toBe('2020-11-25');
      expect(model.eventTypeCode).toBe('ADVPMTSRVDDT');
      expect(model.payeeKey).toBe('OWNER_SOLICITOR-3');
      expect(model.rowVersion).toBe(1);
    });
  });
  describe('ExpropriationEventRow creates form values from api format', () => {
    it('fromApi', () => {
      const model = ExpropriationEventRow.fromApi({
        ...getMockExpropriationEvent(99, 1),
        eventDate: '2020-11-25',
      });

      expect(model).toBeDefined();
      expect(model.id).toBe(99);
      expect(model.acquisitionFileId).toBe(1);
      expect(model.eventDate).toBe('2020-11-25');
      expect(model.eventDescription).toBe('Advanced payment served date');
    });
  });
});
