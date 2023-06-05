import { Api_AcquisitionFile } from './AcquisitionFile';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';

export interface Api_InterestHolder extends Api_ConcurrentVersion {
  id: number;
  personId?: number;
  organizationId?: number;
  isDisabled?: boolean;
  acquisitionFileId: number;
  acquisitionFile: Api_AcquisitionFile;
  organization: Api_Organization;
  person: Api_Person;
}
