import moment from 'moment';

import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_Person } from '@/models/api/Person';

import { Api_GenerateAcquisitionFile } from './acquisition/GenerateAcquisitionFile';
export class Api_GenerateLetter extends Api_GenerateAcquisitionFile {
  date_generated: string;
  constructor(file: Api_AcquisitionFile, coordinatorContact: Api_Person | null | undefined) {
    super({
      file,
      coordinatorContact: coordinatorContact ?? null,
      negotiatingAgent: null,
      provincialSolicitor: null,
      ownerSolicitor: null,
      interestHolders: [],
    });
    this.date_generated = moment().format('DD/M/YYYY');
  }
}
