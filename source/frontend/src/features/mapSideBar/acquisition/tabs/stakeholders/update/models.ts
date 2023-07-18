import { chain, uniqBy } from 'lodash';

import { IContactSearchResult } from '@/interfaces';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import Api_TypeCode from '@/models/api/TypeCode';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

export class StakeHolderForm {
  public interestHolders: InterestHolderForm[] = [];
  public nonInterestPayees: InterestHolderForm[] = [];

  static fromApi(apiModel: Api_InterestHolder[]): StakeHolderForm {
    const stakeHolderForm = new StakeHolderForm();
    const interestHoldersByType: InterestHolderForm[] = [];
    apiModel.forEach((ih: Api_InterestHolder) => {
      ih.interestHolderProperties.forEach((ihp: Api_InterestHolderProperty) => {
        ihp.propertyInterestTypes.forEach(pit => {
          interestHoldersByType.push(InterestHolderForm.fromApi(ih, pit?.id ?? ''));
        });
      });
    });
    stakeHolderForm.interestHolders = interestHoldersByType.filter(
      ihp => ihp.interestTypeCode !== 'NIP',
    );
    stakeHolderForm.nonInterestPayees = interestHoldersByType.filter(
      ihp => ihp.interestTypeCode === 'NIP',
    );

    return stakeHolderForm;
  }

  static toApi(model: StakeHolderForm): Api_InterestHolder[] {
    // Group by personId or organizationId, create a unique list of all impacted properties for each group, and then map back to API model.
    return chain(model.interestHolders.concat(model.nonInterestPayees))
      .groupBy((ih: InterestHolderForm) => ih.contact?.personId ?? ih.contact?.organizationId)
      .map(groupedByInterestHolder => {
        const allPropertiesForInterestHolder = groupedByInterestHolder.flatMap(
          (ih: InterestHolderForm) => ih.impactedProperties,
        );

        const mergedInterestHolderProperties = uniqBy(
          allPropertiesForInterestHolder,
          'acquisitionFilePropertyId',
        ).map((ihp: InterestHolderPropertyForm) => {
          const matchingInterestTypes = groupedByInterestHolder
            .filter(gih =>
              gih.impactedProperties.some(
                ih => ih.acquisitionFilePropertyId === ihp.acquisitionFilePropertyId,
              ),
            )
            .map(gih => gih.interestTypeCode) as string[];
          return InterestHolderPropertyForm.toApi(ihp, matchingInterestTypes);
        });

        const apiInterestHolder = InterestHolderForm.toApi({
          ...groupedByInterestHolder[0],
        });
        apiInterestHolder.interestHolderProperties = mergedInterestHolderProperties;
        return apiInterestHolder;
      })
      .value();
  }
}

export class InterestHolderForm {
  interestHolderId: number | null = null;
  personId: number | null = null;
  person: Api_Person | null = null;
  organizationId: number | null = null;
  organization: Api_Organization | null = null;
  contact: IContactSearchResult | null = null;
  impactedProperties: InterestHolderPropertyForm[] = [];
  interestTypeCode: string | null = null;
  acquisitionFileId: number | null = null;
  isDisabled: boolean = false;
  rowVersion: number | null = null;

  constructor(acquisitionFileId?: number | null) {
    this.acquisitionFileId = acquisitionFileId ?? null;
  }

  static fromApi(apiModel: Api_InterestHolder, interestTypeCode: string): InterestHolderForm {
    const interestHolderForm = new InterestHolderForm();
    interestHolderForm.interestHolderId = apiModel.interestHolderId;
    interestHolderForm.personId = apiModel.personId;
    interestHolderForm.organizationId = apiModel.organizationId;
    interestHolderForm.impactedProperties = apiModel.interestHolderProperties.map(ihp =>
      InterestHolderPropertyForm.fromApi(ihp),
    );
    interestHolderForm.rowVersion = apiModel.rowVersion ?? null;
    interestHolderForm.isDisabled = apiModel.isDisabled;
    interestHolderForm.interestTypeCode = interestTypeCode ?? '';
    interestHolderForm.contact = {
      id: apiModel.personId ? `P${apiModel.personId}` : `O${apiModel.organizationId}`,
      personId: apiModel.personId ?? undefined,
      organizationId: apiModel.organizationId ?? undefined,
      firstName: apiModel.person?.firstName ?? '',
      surname: apiModel.person?.surname ?? '',
      middleNames: apiModel.person?.middleNames ?? '',
      organizationName: apiModel.organization?.name ?? '',
    };
    interestHolderForm.acquisitionFileId = apiModel.acquisitionFileId;

    return interestHolderForm;
  }

  static toApi(model: InterestHolderForm): Api_InterestHolder {
    return {
      interestHolderId: model.interestHolderId,
      interestHolderType: { id: 'INTHLDR' },
      personId: model.contact?.personId ?? null,
      person: null,
      organizationId: !model.contact?.personId ? model.contact?.organizationId ?? null : null,
      organization: null,
      isDisabled: model.isDisabled,
      interestHolderProperties: model.impactedProperties.map(ip =>
        InterestHolderPropertyForm.toApi(
          ip,
          model.interestTypeCode ? [model.interestTypeCode] : [],
        ),
      ),
      acquisitionFileId: model.acquisitionFileId,
      rowVersion: model.rowVersion ?? undefined,
    };
  }
}

export class InterestHolderPropertyForm {
  interestHolderPropertyId: number | null = null;
  interestHolderId: number | null = null;
  acquisitionFilePropertyId: number | null = null;
  isDisabled: boolean = false;
  rowVersion: number | null = null;

  static fromApi(apiModel: Api_InterestHolderProperty): InterestHolderPropertyForm {
    const interestHolderPropertyForm = new InterestHolderPropertyForm();
    interestHolderPropertyForm.interestHolderPropertyId = apiModel.interestHolderPropertyId;
    interestHolderPropertyForm.interestHolderId = apiModel.interestHolderId;
    interestHolderPropertyForm.acquisitionFilePropertyId = apiModel.acquisitionFilePropertyId;
    interestHolderPropertyForm.isDisabled = apiModel.isDisabled;

    return interestHolderPropertyForm;
  }

  static toApi(
    model: InterestHolderPropertyForm,
    interestTypeCodes: string[],
  ): Api_InterestHolderProperty {
    return {
      interestHolderId: model.interestHolderId,
      interestHolderPropertyId: model.interestHolderPropertyId ?? null,
      acquisitionFilePropertyId: model.acquisitionFilePropertyId,
      acquisitionFileProperty: null,
      isDisabled: model.isDisabled,
      rowVersion: model.rowVersion ?? undefined,
      propertyInterestTypes: interestTypeCodes.map(itc => ({ id: itc })),
    };
  }
}

export class InterestHolderViewForm {
  id: number | null = null;
  identifier: string = '';
  groupedPropertyInterests: InterestHolderViewRow[] = [];

  static fromApi(apiModel: Api_InterestHolderProperty) {
    const interestHolderViewForm = new InterestHolderViewForm();
    interestHolderViewForm.id = apiModel.acquisitionFileProperty?.id ?? null;
    interestHolderViewForm.identifier = `${
      getFilePropertyName(apiModel.acquisitionFileProperty ?? undefined).label
    }: ${getFilePropertyName(apiModel.acquisitionFileProperty ?? undefined).value}`;
    return interestHolderViewForm;
  }
}

export class InterestHolderViewRow {
  id: number | null = null;
  interestHolderProperty: Api_InterestHolderProperty | null = null;
  person: Api_Person | null = null;
  organization: Api_Organization | null = null;
  interestHolderType: Api_TypeCode<string> | null = null;

  static fromApi(
    apiInterestHolderProperty: Api_InterestHolderProperty,
    apiInterestHolder: Api_InterestHolder | undefined,
    interestHolderType: Api_TypeCode<string>,
  ) {
    const interestHolderViewRow = new InterestHolderViewRow();
    interestHolderViewRow.id = apiInterestHolder?.interestHolderId ?? 0;
    interestHolderViewRow.interestHolderProperty = apiInterestHolderProperty;
    interestHolderViewRow.person = apiInterestHolder?.person ?? null;
    interestHolderViewRow.organization = apiInterestHolder?.organization ?? null;
    interestHolderViewRow.interestHolderType = interestHolderType ?? null;
    return interestHolderViewRow;
  }
}

export const emptyInterestHolderProperty: Api_InterestHolderProperty = {
  interestHolderId: null,
  propertyInterestTypes: [],
  interestHolderPropertyId: null,
  acquisitionFileProperty: null,
  acquisitionFilePropertyId: null,
  isDisabled: false,
};

export const emptyApiInterestHolder: Api_InterestHolder = {
  interestHolderId: null,
  acquisitionFileId: null,
  person: null,
  personId: null,
  organization: null,
  organizationId: null,
  isDisabled: false,
  interestHolderProperties: [],
  interestHolderType: null,
};

export const emptyInterestHolderForm: InterestHolderForm = {
  interestHolderId: null,
  person: null,
  personId: null,
  organization: null,
  organizationId: null,
  isDisabled: false,
  impactedProperties: [],
  contact: null,
  acquisitionFileId: null,
  rowVersion: null,
  interestTypeCode: null,
};
