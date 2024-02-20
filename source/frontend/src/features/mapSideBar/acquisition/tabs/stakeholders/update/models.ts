import { chain, uniqBy } from 'lodash';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';
import { exists } from '@/utils/utils';

export class StakeHolderForm {
  public interestHolders: InterestHolderForm[] = [];
  public nonInterestPayees: InterestHolderForm[] = [];

  static fromApi(apiModel: ApiGen_Concepts_InterestHolder[]): StakeHolderForm {
    const stakeHolderForm = new StakeHolderForm();

    const interestHolderModels = apiModel.filter(
      x => x.interestHolderType?.id === InterestHolderType.INTEREST_HOLDER,
    );

    const interestHoldersByType: InterestHolderForm[] = [];
    interestHolderModels.forEach((ih: ApiGen_Concepts_InterestHolder) => {
      ih.interestHolderProperties?.forEach((ihp: ApiGen_Concepts_InterestHolderProperty) => {
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

  static toApi(model: StakeHolderForm): ApiGen_Concepts_InterestHolder[] {
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
      .filter(exists);
  }
}

export class InterestHolderPropertyFormModel {
  interestHolderPropertyId = '';
  acquisitionFilePropertyId = '';
  propertyInterestType = '';

  static fromApi(model: ApiGen_Concepts_InterestHolderProperty): InterestHolderPropertyFormModel[] {
    return (
      model.propertyInterestTypes
        ?.map<InterestHolderPropertyFormModel>(x => {
          return {
            interestHolderPropertyId: model.interestHolderPropertyId?.toString() || '',
            acquisitionFilePropertyId: model.acquisitionFilePropertyId?.toString() || '',
            propertyInterestType: x.id || '',
          };
        })
        .filter(exists) || []
    );
  }
}

export class InterestHolderForm {
  interestHolderId: number | null = null;
  personId = '';
  organizationId = '';
  contact: IContactSearchResult | null = null;
  primaryContactId?: number | null = null;
  impactedProperties: InterestHolderPropertyForm[] = [];
  interestTypeCode = '';
  propertyInterestTypeCode = '';
  acquisitionFileId: number | null = null;
  isDisabled = false;
  rowVersion: number | null = null;
  comment: string | null = null;

  constructor(interestHolderType: InterestHolderType, acquisitionFileId?: number | null) {
    this.interestTypeCode = interestHolderType;
    this.acquisitionFileId = acquisitionFileId ?? null;
  }

  static fromApi(
    apiModel: ApiGen_Concepts_InterestHolder,
    interestTypeCode: InterestHolderType,
  ): InterestHolderForm {
    const interestHolderForm = new InterestHolderForm(interestTypeCode);
    interestHolderForm.interestHolderId = apiModel.interestHolderId;

    interestHolderForm.impactedProperties =
      apiModel.interestHolderProperties?.map(ihp => InterestHolderPropertyForm.fromApi(ihp)) || [];

    interestHolderForm.rowVersion = apiModel.rowVersion ?? null;
    interestHolderForm.isDisabled = apiModel.isDisabled;
    interestHolderForm.interestTypeCode = interestTypeCode ?? '';

    if (apiModel.personId) {
      interestHolderForm.personId = apiModel.personId?.toString() || '';
      interestHolderForm.contact = {
        id: `P${apiModel.personId}`,
        personId: apiModel.personId ?? undefined,
        firstName: apiModel.person?.firstName ?? '',
        surname: apiModel.person?.surname ?? '',
        middleNames: apiModel.person?.middleNames ?? '',
      };
    } else {
      interestHolderForm.organizationId = apiModel.organizationId?.toString() || '';
      interestHolderForm.contact = {
        id: `O${apiModel.organizationId}`,
        organizationId: apiModel.organizationId ?? undefined,
        organizationName: apiModel.organization?.name ?? '',
      };
      interestHolderForm.primaryContactId = apiModel.primaryContactId;
    }

    interestHolderForm.acquisitionFileId = apiModel.acquisitionFileId;
    interestHolderForm.comment = apiModel.comment;

    return interestHolderForm;
  }

  static toApi(
    model: InterestHolderForm,
    properties: ApiGen_Concepts_InterestHolderProperty[],
  ): ApiGen_Concepts_InterestHolder | null {
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
      comment: model.comment,
      interestHolderType: toTypeCodeNullable(model.interestTypeCode),
      primaryContact: null,
      ...getEmptyBaseAudit(model.rowVersion),
    };
  }
}

export class InterestHolderPropertyForm {
  interestHolderPropertyId: number | null = null;
  interestHolderId: number | null = null;
  acquisitionFilePropertyId: number | null = null;
  rowVersion: number | null = null;

  static fromApi(apiModel: ApiGen_Concepts_InterestHolderProperty): InterestHolderPropertyForm {
    const interestHolderPropertyForm = new InterestHolderPropertyForm();
    interestHolderPropertyForm.interestHolderPropertyId = apiModel.interestHolderPropertyId;
    interestHolderPropertyForm.interestHolderId = apiModel.interestHolderId;
    interestHolderPropertyForm.acquisitionFilePropertyId = apiModel.acquisitionFilePropertyId;
    interestHolderPropertyForm.rowVersion = apiModel.rowVersion;

    return interestHolderPropertyForm;
  }

  static toApi(
    model: InterestHolderPropertyForm,
    interestTypeCodes: string[],
  ): ApiGen_Concepts_InterestHolderProperty {
    return {
      interestHolderId: model.interestHolderId,
      interestHolderPropertyId: model.interestHolderPropertyId ?? null,
      acquisitionFilePropertyId: model.acquisitionFilePropertyId,
      acquisitionFileProperty: null,
      propertyInterestTypes: interestTypeCodes.map(itc => toTypeCodeNullable(itc)).filter(exists),
      ...getEmptyBaseAudit(model.rowVersion),
    };
  }
}

export class InterestHolderViewForm {
  id: number | null = null;
  identifier = '';
  groupedPropertyInterests: InterestHolderViewRow[] = [];

  static fromApi(apiModel: ApiGen_Concepts_InterestHolderProperty) {
    const interestHolderViewForm = new InterestHolderViewForm();
    interestHolderViewForm.id = apiModel.acquisitionFileProperty?.id ?? null;
    interestHolderViewForm.identifier = `${
      getFilePropertyName(apiModel.acquisitionFileProperty).label
    }: ${getFilePropertyName(apiModel.acquisitionFileProperty).value}`;
    return interestHolderViewForm;
  }
}

export class InterestHolderViewRow {
  id: number | null = null;
  interestHolderProperty: ApiGen_Concepts_InterestHolderProperty | null = null;
  person: ApiGen_Concepts_Person | null = null;
  organization: ApiGen_Concepts_Organization | null = null;
  interestHolderType: ApiGen_Base_CodeType<string> | null = null;
  primaryContact: ApiGen_Concepts_Person | null = null;

  static fromApi(
    apiInterestHolderProperty: ApiGen_Concepts_InterestHolderProperty,
    apiInterestHolder: ApiGen_Concepts_InterestHolder | undefined,
    interestHolderType: ApiGen_Base_CodeType<string>,
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
