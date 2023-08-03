import { chain, uniqBy } from 'lodash';

import { InterestHolderType } from '@/constants/interestHolderTypes';
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

    const interestHolderModels = apiModel.filter(
      x => x.interestHolderType?.id === InterestHolderType.INTEREST_HOLDER,
    );

    const interestHoldersByType: InterestHolderForm[] = [];
    interestHolderModels.forEach((ih: Api_InterestHolder) => {
      ih.interestHolderProperties.forEach((ihp: Api_InterestHolderProperty) => {
        if (ihp.propertyInterestTypes !== null) {
          ihp.propertyInterestTypes.forEach(pit => {
            const formModel = InterestHolderForm.fromApi(ih, InterestHolderType.INTEREST_HOLDER);
            formModel.propertyInterestTypeCode = pit?.id ?? '';

            const matchingInterestHolder = interestHoldersByType.find(
              i =>
                i.interestHolderId === ih.interestHolderId &&
                i.propertyInterestTypeCode === pit?.id,
            );
            if (!matchingInterestHolder) {
              formModel.impactedProperties = [InterestHolderPropertyForm.fromApi(ihp)];
              interestHoldersByType.push(formModel);
            } else {
              matchingInterestHolder.impactedProperties.push(
                InterestHolderPropertyForm.fromApi(ihp),
              );
            }
          });
        } else {
          ihp.propertyInterestTypes = [];
        }
      });
    });
    stakeHolderForm.interestHolders = interestHoldersByType.filter(
      ihp => ihp.propertyInterestTypeCode !== 'NIP',
    );
    stakeHolderForm.nonInterestPayees = interestHoldersByType.filter(
      ihp => ihp.propertyInterestTypeCode === 'NIP',
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
            .map(gih => gih.propertyInterestTypeCode);

          return InterestHolderPropertyForm.toApi(ihp, matchingInterestTypes);
        });

        const apiInterestHolder = InterestHolderForm.toApi(
          {
            ...groupedByInterestHolder[0],
          },
          mergedInterestHolderProperties,
        );
        return apiInterestHolder;
      })
      .value()
      .filter((x): x is Api_InterestHolder => x !== null);
  }
}

export class InterestHolderPropertyFormModel {
  interestHolderPropertyId: string = '';
  acquisitionFilePropertyId: string = '';
  propertyInterestType: string = '';

  static fromApi(model: Api_InterestHolderProperty): InterestHolderPropertyFormModel[] {
    return (
      model.propertyInterestTypes
        ?.map<InterestHolderPropertyFormModel>(x => {
          return {
            interestHolderPropertyId: model.interestHolderPropertyId?.toString() || '',
            acquisitionFilePropertyId: model.acquisitionFilePropertyId?.toString() || '',
            propertyInterestType: x.id || '',
          };
        })
        .filter((x): x is InterestHolderPropertyFormModel => x !== undefined) || []
    );
  }
}

export class InterestHolderForm {
  interestHolderId: number | null = null;
  personId: string = '';
  organizationId: string = '';
  contact: IContactSearchResult | null = null;
  primaryContactId?: number | null = null;
  impactedProperties: InterestHolderPropertyForm[] = [];
  interestTypeCode: string = '';
  propertyInterestTypeCode: string = '';
  acquisitionFileId: number | null = null;
  isDisabled: boolean = false;
  rowVersion: number | null = null;
  comment: string | null = null;

  constructor(interestHolderType: InterestHolderType, acquisitionFileId?: number | null) {
    this.interestTypeCode = interestHolderType;
    this.acquisitionFileId = acquisitionFileId ?? null;
  }

  static fromApi(
    apiModel: Api_InterestHolder,
    interestTypeCode: InterestHolderType,
  ): InterestHolderForm {
    const interestHolderForm = new InterestHolderForm(interestTypeCode);
    interestHolderForm.interestHolderId = apiModel.interestHolderId;
    interestHolderForm.personId = apiModel.personId?.toString() || '';
    interestHolderForm.organizationId = apiModel.organizationId?.toString() || '';
    interestHolderForm.impactedProperties = apiModel.interestHolderProperties.map(ihp =>
      InterestHolderPropertyForm.fromApi(ihp),
    );
    interestHolderForm.primaryContactId = apiModel.primaryContactId;
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
    interestHolderForm.comment = apiModel.comment;
    interestHolderForm.primaryContactId = apiModel.primaryContactId;

    return interestHolderForm;
  }

  static toApi(
    model: InterestHolderForm,
    properties: Api_InterestHolderProperty[],
  ): Api_InterestHolder | null {
    const personId = model.contact?.personId ?? null;
    const organizationId = !personId ? model.contact?.organizationId ?? null : null;
    if (personId === null && organizationId === null) {
      return null;
    }

    return {
      interestHolderId: model.interestHolderId,
      personId: personId,
      person: null,
      organizationId: organizationId,
      organization: null,
      primaryContactId: model.primaryContactId ?? null,
      isDisabled: model.isDisabled,
      interestHolderProperties: properties,
      acquisitionFileId: model.acquisitionFileId,
      rowVersion: model.rowVersion ?? undefined,
      comment: model.comment,
      interestHolderType: { id: model.interestTypeCode },
      primaryContact: null,
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
    interestHolderPropertyForm.rowVersion = apiModel.rowVersion;

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
      rowVersion: model.rowVersion,
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
  primaryContact: Api_Person | null = null;

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
    interestHolderViewRow.primaryContact = apiInterestHolder?.primaryContact ?? null;
    return interestHolderViewRow;
  }
}
