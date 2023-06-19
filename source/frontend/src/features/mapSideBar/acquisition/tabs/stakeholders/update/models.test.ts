import { Api_InterestHolder } from '@/models/api/InterestHolder';

import {
  emptyApiInterestHolder,
  emptyInterestHolderForm,
  emptyInterestHolderProperty,
  InterestHolderForm,
  StakeHolderForm,
} from './models';

describe('Interest Holder model tests', () => {
  it('StakeHolderForm splits a single InterestHolder into interests and non-interests', () => {
    const apiInterestHolders: Api_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      interestHolderProperties: [
        { ...emptyInterestHolderProperty, interestTypeCode: { id: 'NIP' }, interestHolderId: 1 },
        { ...emptyInterestHolderProperty, interestTypeCode: { id: 'IP' }, interestHolderId: 1 },
      ],
    };
    const stakeholderModel = StakeHolderForm.fromApi([apiInterestHolders]);
    expect(stakeholderModel.interestHolders).toHaveLength(1);
    expect(stakeholderModel.nonInterestPayees).toHaveLength(1);
  });

  it('StakeHolderForm splits a single InterestHolder into multiple if an interest holder has multiple properties of different types', () => {
    const apiInterestHolders: Api_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      interestHolderProperties: [
        { ...emptyInterestHolderProperty, interestTypeCode: { id: 'PI' }, interestHolderId: 1 },
        { ...emptyInterestHolderProperty, interestTypeCode: { id: 'IP' }, interestHolderId: 1 },
      ],
    };
    const stakeholderModel = StakeHolderForm.fromApi([apiInterestHolders]);
    expect(stakeholderModel.interestHolders).toHaveLength(2);
  });

  it('StakeHolderForm splits multiple InterestHolders into interests and non-interests', () => {
    const apiInterestHolders: Api_InterestHolder[] = [
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        interestHolderProperties: [
          { ...emptyInterestHolderProperty, interestTypeCode: { id: 'NIP' }, interestHolderId: 1 },
          { ...emptyInterestHolderProperty, interestTypeCode: { id: 'IP' }, interestHolderId: 1 },
        ],
      },
      {
        ...emptyApiInterestHolder,
        interestHolderId: 2,
        interestHolderProperties: [
          { ...emptyInterestHolderProperty, interestTypeCode: { id: 'NIP' }, interestHolderId: 2 },
          { ...emptyInterestHolderProperty, interestTypeCode: { id: 'PI' }, interestHolderId: 2 },
        ],
      },
    ];
    const stakeholderModel = StakeHolderForm.fromApi(apiInterestHolders);
    expect(stakeholderModel.interestHolders).toHaveLength(2);
    expect(stakeholderModel.nonInterestPayees).toHaveLength(2);
  });

  it('StakeHolderForm toApi combines multiple persons with the same id into one interest holder', () => {
    const model = new StakeHolderForm();
    model.interestHolders = [
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 2,
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(2);
  });

  it('StakeHolderForm toApi combines multiple persons with the same id into one interest holder and combines duplicate properties on separate interest holders', () => {
    const model = new StakeHolderForm();
    model.interestHolders = [
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(1);
  });

  it('StakeHolderForm toApi combines multiple persons with the same id even if they are a interest and non-interest', () => {
    const model = new StakeHolderForm();
    model.interestHolders = [
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            interestTypeCode: { id: 'IP' },
          },
        ],
      },
    ];
    model.nonInterestPayees = [
      {
        ...emptyInterestHolderForm,
        personId: 1,
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            interestTypeCode: { id: 'NIP' },
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(1);
  });

  it('StakeHolderForm toApi combines multiple persons with the same id but keeps their properties if the interest types are different', () => {
    const model = new StakeHolderForm();
    model.interestHolders = [
      {
        ...emptyInterestHolderForm,
        personId: 1,
        interestTypeCode: 'IP',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        personId: 1,
        interestTypeCode: 'PI',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(2);
  });

  it('InterestHolderForm sets contact based on api response', () => {
    const apiInterestHolder: Api_InterestHolder = { ...emptyApiInterestHolder, personId: 1 };

    const interestHolderModel = InterestHolderForm.fromApi(apiInterestHolder);
    expect(interestHolderModel.contact?.personId).toBe(1);
    expect(interestHolderModel.contact?.id).toBe('P1');
  });

  it('InterestHolderForm sets person info from contact', () => {
    const interestHolderModel: InterestHolderForm = {
      ...emptyInterestHolderForm,
      contact: { id: 'P1', personId: 1 },
    };

    const apiInterestHolder = InterestHolderForm.toApi(interestHolderModel);
    expect(apiInterestHolder?.personId).toBe(1);
  });

  it('InterestHolderForm sets organization info from contact', () => {
    const interestHolderModel: InterestHolderForm = {
      ...emptyInterestHolderForm,
      contact: { id: 'O1', organizationId: 1 },
    };

    const apiInterestHolder = InterestHolderForm.toApi(interestHolderModel);
    expect(apiInterestHolder?.organizationId).toBe(1);
  });
});
