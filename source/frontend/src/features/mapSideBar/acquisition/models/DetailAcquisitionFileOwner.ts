import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { exists } from '@/utils/utils';

export class DetailAcquisitionFileOwner {
  isPrimary?: boolean;
  ownerName?: string;
  ownerOtherName?: string;
  ownerDisplayAddress?: string;
  ownerContactEmail?: string;
  ownerContactPhone?: string;

  static fromApi(owner: ApiGen_Concepts_AcquisitionFileOwner): DetailAcquisitionFileOwner {
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

const getOwnerDisplayName = (owner: ApiGen_Concepts_AcquisitionFileOwner): string => {
  let ownerDisplayName = '';
  if (owner.isOrganization) {
    const regNumber = owner.registrationNumber ? `Reg#:${owner.registrationNumber}` : null;
    const incNumber = owner.incorporationNumber ? `Inc#:${owner.incorporationNumber}` : null;
    const separator = owner.incorporationNumber && owner.registrationNumber ? ' / ' : null;

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

const getFormattedAddress = (address?: ApiGen_Concepts_Address | null): string => {
  if (!exists(address)) {
    return '';
  }

  let addressDisplay = '';
  const streetAddress1 = address?.streetAddress1 ? address?.streetAddress1.trim() : null;
  const streetAddress2 = address?.streetAddress2 ? address?.streetAddress2.trim() : null;
  const streetAddress3 = address?.streetAddress3 ? address?.streetAddress3.trim() : null;
  const streetAddress4 = concatValues(
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
      const countryDisplay =
        `${address?.country?.description?.trim()} - ${address?.countryOther?.trim()}` || '';
      addressDisplay = addressDisplay.concat(countryDisplay);
    } else {
      const countryDisplay = address?.country?.description?.trim() || '';
      addressDisplay = addressDisplay.concat(countryDisplay);
    }
  }
  return addressDisplay;
};

const concatValues = (nameParts: Array<string | undefined | null>, separator = ' '): string => {
  return nameParts.filter(n => exists(n) && n.trim() !== '').join(separator);
};
