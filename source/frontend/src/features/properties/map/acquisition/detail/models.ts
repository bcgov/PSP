import {
  Api_AcquisitionFile,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
} from 'models/api/AcquisitionFile';
import { Api_Address } from 'models/api/Address';
import { formatApiPersonNames } from 'utils/personUtils';

export class DetailAcquisitionFile {
  fileName?: string;
  legacyFileNumber?: string;
  assignedDate?: string;
  deliveryDate?: string;
  acquisitionPhysFileStatusTypeDescription?: string;
  acquisitionTypeDescription?: string;
  regionDescription?: string;
  acquisitionTeam: DetailAcquisitionFilePerson[] = [];

  static fromApi(model?: Api_AcquisitionFile): DetailAcquisitionFile {
    const detail = new DetailAcquisitionFile();
    detail.fileName = model?.fileName;
    detail.legacyFileNumber = model?.legacyFileNumber;
    detail.assignedDate = model?.assignedDate;
    detail.deliveryDate = model?.deliveryDate;
    detail.acquisitionPhysFileStatusTypeDescription =
      model?.acquisitionPhysFileStatusTypeCode?.description;
    detail.acquisitionTypeDescription = model?.acquisitionTypeCode?.description;
    detail.regionDescription = model?.regionCode?.description;
    detail.acquisitionTeam =
      model?.acquisitionTeam?.map(x => DetailAcquisitionFilePerson.fromApi(x)) || [];

    return detail;
  }
}

export class DetailAcquisitionFilePerson {
  personId?: number;
  personName?: string;
  personProfileTypeCodeDescription?: string;

  static fromApi(model: Api_AcquisitionFilePerson): DetailAcquisitionFilePerson {
    const personDetail = new DetailAcquisitionFilePerson();
    personDetail.personId = model?.person?.id;
    personDetail.personName = formatApiPersonNames(model?.person);
    personDetail.personProfileTypeCodeDescription = model?.personProfileType?.description;

    return personDetail;
  }
}

export class DetailAcquisitionFileOwner {
  isPrimary?: boolean;
  ownerName?: string;
  ownerOtherName?: string;
  ownerDisplayAddress?: string;
  ownerContactEmail?: string;
  ownerContactPhone?: string;

  static fromApi(owner: Api_AcquisitionFileOwner): DetailAcquisitionFileOwner {
    return {
      isPrimary: owner.isPrimaryContact,
      ownerName: getOwnerDisplayName(owner),
      ownerOtherName: owner.otherName?.trim() || '',
      ownerDisplayAddress: getFormattedAddress(owner.address),
      ownerContactEmail: owner.contactEmailAddr || '',
      ownerContactPhone: owner.contactPhoneNum || '',
    };
  }
}

const getOwnerDisplayName = (owner: Api_AcquisitionFileOwner): string => {
  let nameDisplay = concatValues([owner.givenName, owner.lastNameAndCorpName]);
  if (owner.incorporationNumber && owner.incorporationNumber.trim() !== '') {
    nameDisplay = nameDisplay.concat(` (${owner.incorporationNumber})`);
  }
  return nameDisplay;
};

const getFormattedAddress = (address?: Api_Address | null): string => {
  if (address === null || address === undefined) {
    return '';
  }

  let addressDisplay = '';
  let streetAddress1 = address?.streetAddress1 ? address?.streetAddress1.trim() : null;
  let streetAddress2 = address?.streetAddress2 ? address?.streetAddress2.trim() : null;
  let streetAddress3 = address?.streetAddress3 ? address?.streetAddress3.trim() : null;
  let streetAddress4 = concatValues(
    [address?.municipality, address?.province?.description, address?.postal],
    ', ',
  );

  if (streetAddress1) {
    addressDisplay = addressDisplay.concat(streetAddress1, '\n');
  }

  if (streetAddress2) {
    addressDisplay = addressDisplay.concat(streetAddress2, '\n');
  }

  if (streetAddress3) {
    addressDisplay = addressDisplay.concat(streetAddress3, '\n');
  }

  if (streetAddress4) {
    addressDisplay = addressDisplay.concat(streetAddress4, '\n');
  }

  if (address?.country?.description) {
    let countryDisplay = address?.country?.description?.trim() || '';
    addressDisplay = addressDisplay.concat(countryDisplay);
  }

  return addressDisplay;
};

const concatValues = (
  nameParts: Array<string | undefined | null>,
  separator: string = ' ',
): string => {
  return nameParts.filter(n => n !== null && n !== undefined && n.trim() !== '').join(separator);
};
