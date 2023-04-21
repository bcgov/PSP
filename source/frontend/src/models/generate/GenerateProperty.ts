import { Api_Property } from 'models/api/Property';
import { pidFormatter } from 'utils';
export class GenerateProperty {
  pid: string;
  legal_description: string;
  constructor(property: Api_Property | null | undefined) {
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_description = property?.landLegalDescription ?? '';
  }
}
