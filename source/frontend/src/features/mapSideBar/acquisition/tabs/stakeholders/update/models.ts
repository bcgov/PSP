import { chain, uniqBy } from 'lodash';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { IContactSearchResult } from '@/interfaces';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

export class StakeHolderForm {
  public interestHolders: InterestHolderForm[] = [];
  public nonInterestPayees: InterestHolderForm[] = [];

  static fromApi(apiModel: Api_InterestHolder[]): StakeHolderForm {
    const stakeHolderForm = new StakeHolderForm();

    // a person may have an interest and a non-interest in the same property. We need to split the person/organization into its interest and non-interest components.
    /*stakeHolderForm.nonInterestPayees = apiModel
      .map(ih =>
        InterestHolderForm.fromApi({
          ...ih,
          interestHolderProperties: ih.interestHolderProperties.filter(
            ihp => ihp.interestTypeCode?.id === 'NIP',
          ),
        }),
      )
      .filter(ih => ih.impactedProperties.length > 0);

    const groupedInterestHolders: Api_InterestHolder[] = [];
    apiModel
      .flatMap(ih => ih.interestHolderProperties)
      .forEach(ihp => {
        // Group interest holders with the same interest type code together.
        const matchingInterestHolder = groupedInterestHolders.find(
          gih => gih.interestHolderProperties[0].interestTypeCode?.id === ihp.interestTypeCode?.id,
        );
        const ihpParent = apiModel.find(ih => ih.interestHolderId === ihp.interestHolderId);
        if (
          !!ihpParent &&
          (!matchingInterestHolder ||
            ihpParent?.personId !== matchingInterestHolder?.personId ||
            ihpParent?.organizationId !== matchingInterestHolder?.organizationId)
        ) {
          groupedInterestHolders.push({
            ...ihpParent,
            interestHolderProperties: [ihp],
          });
        } else {
          matchingInterestHolder?.interestHolderProperties.push(ihp);
        }
      });
    stakeHolderForm.interestHolders = groupedInterestHolders
      .map(ih =>
        InterestHolderForm.fromApi({
          ...ih,
          interestHolderProperties: ih.interestHolderProperties.filter(
            ihp => ihp.interestTypeCode?.id !== 'NIP',
          ),
        }),
      )
      .filter(ih => ih.impactedProperties.length > 0);*/

    return stakeHolderForm;
  }

  static toApi(model: StakeHolderForm): Api_InterestHolder[] {
    // Group by personId or organizationId, create a unique list of all impacted properties for each group, and then map back to API model.
    /*return chain(model.interestHolders.concat(model.nonInterestPayees))
      .forEach(
        (
          ih: InterestHolderForm, // copy the interest type from the interest holder to the impacted properties
        ) =>
          ih.impactedProperties.forEach(
            (ihp: Api_InterestHolderProperty) =>
              (ihp.propertyInterestTypes = ih.interestPropertyInterestTypeCode
                ? [{ id: ih.interestPropertyInterestTypeCode }]
                : null),
          ),
      )
      .groupBy((ih: InterestHolderForm) => ih.contact?.personId ?? ih.contact?.organizationId)
      .map(gip => {
        const interestHolderProperties = uniqBy(
          gip.flatMap((ih: InterestHolderForm) => ih.impactedProperties),
          (ihp: Api_InterestHolderProperty) => {
            if (ihp.propertyInterestTypes !== null && ihp.propertyInterestTypes.length > 0) {
              return `${ihp.acquisitionFilePropertyId}-${ihp.propertyInterestTypes[0]?.id}`;
            } else {
              return '';
            }
          },
        ); // combine interest holder properties with the same interest type code and acquisition file property id.

        return InterestHolderForm.toApi({
          ...gip[0],
          impactedProperties: interestHolderProperties,
        });
      })
      .value()
      .filter((x): x is Api_InterestHolder => x !== null);*/

    return [];
  }
}

export class InterestHolderPropertyFormModel {
  //acquisitionFileProperty = {};
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
  //personId: string = '';
  //person: Api_Person | null = null;
  //organizationId: string = '';
  //organization: Api_Organization | null = null;
  contact: IContactSearchResult | null = null;
  impactedProperties: Api_InterestHolderProperty[] = [];
  //impactedProperties: InterestHolderPropertyFormModel[] = [];
  interestTypeCode: string;
  interestPropertyInterestTypeCode: string = '';
  acquisitionFileId: number | null = null;
  isDisabled: boolean = false;
  rowVersion: number | null = null;
  comment: string | null = null;

  constructor(interestHolderType: InterestHolderType, acquisitionFileId?: number | null) {
    this.interestTypeCode = interestHolderType;
    this.acquisitionFileId = acquisitionFileId ?? null;
  }

  static fromApi(apiModel: Api_InterestHolder): InterestHolderForm {
    const interestHolderType =
      apiModel.interestHolderType.id !== undefined
        ? (apiModel.interestHolderType.id as InterestHolderType)
        : InterestHolderType.INTEREST_HOLDER;

    const interestHolderForm = new InterestHolderForm(
      interestHolderType,
      apiModel.acquisitionFileId,
    );
    interestHolderForm.interestHolderId = apiModel.interestHolderId;
    //interestHolderForm.personId = apiModel.personId?.toString() || '';
    //interestHolderForm.organizationId = apiModel.organizationId?.toString() || '';
    /*interestHolderForm.impactedProperties = apiModel.interestHolderProperties.map(x =>
      InterestHolderPropertyFormModel.fromApi(x),
    );*/
    interestHolderForm.rowVersion = apiModel.rowVersion ?? null;
    interestHolderForm.isDisabled = apiModel.isDisabled;

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

    return interestHolderForm;
  }

  static toApi(model: InterestHolderForm): Api_InterestHolder | null {
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
      isDisabled: model.isDisabled,
      interestHolderProperties: [],
      /*interestHolderProperties: model.impactedProperties.map(ihp => ({
        interestHolderPropertyId: ihp.interestHolderPropertyId,
        interestHolderId: ihp.interestHolderId,
        acquisitionFilePropertyId: ihp.acquisitionFilePropertyId,
        propertyInterestTypes:
          model.interestPropertyInterestTypeCode !== ''
            ? [{ id: model.interestPropertyInterestTypeCode }]
            : null,
        rowVersion: ihp.rowVersion ?? undefined,
        acquisitionFileProperty: null,
        isDisabled: false,
      })),*/
      acquisitionFileId: model.acquisitionFileId,
      rowVersion: model.rowVersion ?? undefined,
      comment: model.comment,
      interestHolderType: { id: model.interestTypeCode },
      primaryContact: null,
      primaryContactId: null,
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

  static fromApi(
    apiInterestHolderProperty: Api_InterestHolderProperty,
    apiInterestHolder?: Api_InterestHolder,
  ) {
    const interestHolderViewRow = new InterestHolderViewRow();
    interestHolderViewRow.id = apiInterestHolder?.interestHolderId ?? 0;
    interestHolderViewRow.interestHolderProperty = apiInterestHolderProperty;
    interestHolderViewRow.person = apiInterestHolder?.person ?? null;
    interestHolderViewRow.organization = apiInterestHolder?.organization ?? null;
    return interestHolderViewRow;
  }
}
