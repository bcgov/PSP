import { Api_Property } from 'models/api/Property';
import { pidFormatter } from 'utils';

import { GenerateAddress } from './GenerateAddress';

export class GenerateProperty {
  pid: string;
  legal_description: string;
  address: GenerateAddress | null;
  constructor(property: Api_Property | null | undefined) {
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_description = property?.landLegalDescription ?? '';
    this.address =
      property?.address !== null && property?.address !== undefined
        ? new GenerateAddress(property.address)
        : null;
  }
}
