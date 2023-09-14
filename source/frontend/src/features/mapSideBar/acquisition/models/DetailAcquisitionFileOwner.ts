import { Api_AcquisitionFileOwner } from '@/models/api/AcquisitionFile';
import { Api_Address } from '@/models/api/Address';

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
  let ownerDisplayName = '';
  if (owner.isOrganization) {
    let regNumber = owner.registrationNumber ? `Reg#:${owner.registrationNumber}` : null;
    let incNumber = owner.incorporationNumber ? `Inc#:${owner.incorporationNumber}` : null;
    let separator = owner.incorporationNumber && owner.registrationNumber ? ' / ' : null;

    if (incNumber || regNumber) {
      ownerDisplayName = concatValues(
        [owner.lastNameAndCorpName, ' (', incNumber, separator, regNumber, ')'],
        '',
      );
    } else {
      ownerDisplayName = owner.lastNameAndCorpName ? owner.lastNameAndCorpName : '';
    }
  } else {
    ownerDisplayName = concatValues([owner.givenName, owner.lastNameAndCorpName]);
  }

  return ownerDisplayName;
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
    if (address?.country?.code === 'OTHER') {
      let countryDisplay =
        `${address?.country?.description?.trim()} - ${address?.countryOther?.trim()}` || '';
      addressDisplay = addressDisplay.concat(countryDisplay);
    } else {
      let countryDisplay = address?.country?.description?.trim() || '';
      addressDisplay = addressDisplay.concat(countryDisplay);
    }
  }
  return addressDisplay;
};

const concatValues = (
  nameParts: Array<string | undefined | null>,
  separator: string = ' ',
): string => {
  return nameParts.filter(n => n !== null && n !== undefined && n.trim() !== '').join(separator);
};
