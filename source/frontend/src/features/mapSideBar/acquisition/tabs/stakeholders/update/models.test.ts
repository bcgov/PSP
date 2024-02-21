import { InterestHolderType } from '@/constants/interestHolderTypes';
import { emptyApiInterestHolder, emptyInterestHolderProperty } from '@/mocks/interestHolder.mock';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { toTypeCodeNullable } from '@/utils/formUtils';

import { InterestHolderForm, StakeHolderForm } from './models';

const emptyInterestHolderForm: InterestHolderForm = {
  interestHolderId: null,
  primaryContactId: null,
  isDisabled: false,
  impactedProperties: [],
  contact: null,
  acquisitionFileId: null,
  rowVersion: null,
  interestTypeCode: 'test_code',
  comment: null,
  personId: '',
  organizationId: '',
  propertyInterestTypeCode: '',
};

describe('Interest Holder model tests', () => {
  it('StakeHolderForm fromApi splits a single InterestHolder into interests and non-interests', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      interestHolderType: toTypeCodeNullable(InterestHolderType.INTEREST_HOLDER),
      interestHolderProperties: [
        {
          ...emptyInterestHolderProperty,
          propertyInterestTypes: [
            { id: 'NIP', description: null, displayOrder: null, isDisabled: false },
          ],
          interestHolderId: 1,
        },
        {
          ...emptyInterestHolderProperty,
          propertyInterestTypes: [
            { id: 'IP', description: null, displayOrder: null, isDisabled: false },
          ],
          interestHolderId: 1,
        },
      ],
    };
    const stakeholderModel = StakeHolderForm.fromApi([apiInterestHolders]);
    expect(stakeholderModel.interestHolders).toHaveLength(1);
    expect(stakeholderModel.nonInterestPayees).toHaveLength(1);
  });

  it('StakeHolderForm fromApi splits a single InterestHolder into multiple if an interest holder has multiple properties of different types', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      interestHolderType: {
        id: InterestHolderType.INTEREST_HOLDER,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      interestHolderProperties: [
        {
          ...emptyInterestHolderProperty,
          propertyInterestTypes: [
            { id: 'PI', description: null, displayOrder: null, isDisabled: false },
          ],
          interestHolderId: 1,
        },
        {
          ...emptyInterestHolderProperty,
          propertyInterestTypes: [
            { id: 'IP', description: null, displayOrder: null, isDisabled: false },
          ],
          interestHolderId: 1,
        },
      ],
    };
    const stakeholderModel = StakeHolderForm.fromApi([apiInterestHolders]);
    expect(stakeholderModel.interestHolders).toHaveLength(2);
  });

  it('StakeHolderForm fromApi does not combine multiple stakeholders even if they have the same type', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder[] = [
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        personId: 2,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
        ],
      },
      {
        ...emptyApiInterestHolder,
        interestHolderId: 2,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        personId: 1,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 2,
          },
        ],
      },
    ];
    const stakeholderModel = StakeHolderForm.fromApi(apiInterestHolders);
    expect(stakeholderModel.interestHolders).toHaveLength(2);
  });

  it('StakeHolderForm fromApi does not split an interest holder with multiple properties if the properties have the same type', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder[] = [
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        personId: 2,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
        ],
      },
    ];
    const stakeholderModel = StakeHolderForm.fromApi(apiInterestHolders);
    expect(stakeholderModel.interestHolders).toHaveLength(1);
  });

  it('StakeHolderForm fromApi does split an interest holder with multiple properties if the properties have different types', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder[] = [
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        personId: 2,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'ip2', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
        ],
      },
    ];
    const stakeholderModel = StakeHolderForm.fromApi(apiInterestHolders);
    expect(stakeholderModel.interestHolders).toHaveLength(2);
  });

  it('StakeHolderForm fromApi splits multiple InterestHolders into interests and non-interests', () => {
    const apiInterestHolders: ApiGen_Concepts_InterestHolder[] = [
      {
        ...emptyApiInterestHolder,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        interestHolderId: 1,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'NIP', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'IP', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 1,
          },
        ],
      },
      {
        ...emptyApiInterestHolder,
        interestHolderType: {
          id: InterestHolderType.INTEREST_HOLDER,
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        interestHolderId: 2,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'NIP', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 2,
          },
          {
            ...emptyInterestHolderProperty,
            propertyInterestTypes: [
              { id: 'PI', description: null, displayOrder: null, isDisabled: false },
            ],
            interestHolderId: 2,
          },
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
        contact: { id: 'P1', personId: 1 },
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        contact: { id: 'P1', personId: 1 },
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 2,
            rowVersion: 1,
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
        contact: { id: 'P1', personId: 1 },
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        contact: { id: 'P1', personId: 1 },
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
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
        contact: { id: 'P1', personId: 1 },
        propertyInterestTypeCode: 'IP',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,

            rowVersion: 1,
          },
        ],
      },
    ];
    model.nonInterestPayees = [
      {
        ...emptyInterestHolderForm,
        contact: { id: 'P1', personId: 1 },
        propertyInterestTypeCode: 'NIP',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(1);
  });

  it('StakeHolderForm toApi combines multiple persons with the same id and combines the properties with the same id while having different interest types', () => {
    const model = new StakeHolderForm();
    model.interestHolders = [
      {
        ...emptyInterestHolderForm,
        contact: { id: 'P1', personId: 1 },
        interestTypeCode: InterestHolderType.INTEREST_HOLDER,
        propertyInterestTypeCode: 'IP',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
          },
        ],
      },
      {
        ...emptyInterestHolderForm,
        contact: { id: 'P1', personId: 1 },
        interestTypeCode: InterestHolderType.INTEREST_HOLDER,
        propertyInterestTypeCode: 'PI',
        impactedProperties: [
          {
            ...emptyInterestHolderProperty,
            interestHolderPropertyId: 1,
            acquisitionFilePropertyId: 1,
            rowVersion: 1,
          },
        ],
      },
    ];
    const apiInterestHolders = StakeHolderForm.toApi(model);
    expect(apiInterestHolders).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties).toHaveLength(1);
    expect(apiInterestHolders[0].interestHolderProperties![0].propertyInterestTypes).toHaveLength(
      2,
    );
  });

  it('InterestHolderForm sets contact based on api response', () => {
    const apiInterestHolder: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      personId: 1,
    };

    const interestHolderModel = InterestHolderForm.fromApi(
      apiInterestHolder,
      InterestHolderType.INTEREST_HOLDER,
    );
    expect(interestHolderModel.contact?.personId).toBe(1);
    expect(interestHolderModel.contact?.id).toBe('P1');
  });

  it('InterestHolderForm sets person info from contact', () => {
    const interestHolderModel: InterestHolderForm = {
      ...emptyInterestHolderForm,
      contact: { id: 'P1', personId: 1 },
    };

    const apiInterestHolder = InterestHolderForm.toApi(interestHolderModel, []);
    expect(apiInterestHolder?.personId).toBe(1);
  });

  it('InterestHolderForm sets organization info from contact', () => {
    const interestHolderModel: InterestHolderForm = {
      ...emptyInterestHolderForm,
      contact: { id: 'O1', organizationId: 1 },
    };

    const apiInterestHolder = InterestHolderForm.toApi(interestHolderModel, []);
    expect(apiInterestHolder?.organizationId).toBe(1);
  });
});
