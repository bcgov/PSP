import moment from 'moment';

import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { ApiGen_Concepts_AcquisitionFileTeam } from '../api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { Api_GenerateAcquisitionFile } from './acquisition/GenerateAcquisitionFile';
export class Api_GenerateLetter extends Api_GenerateAcquisitionFile {
  date_generated: string;
  constructor(
    file: ApiGen_Concepts_AcquisitionFile,
    coordinatorContact: ApiGen_Concepts_AcquisitionFileTeam | null | undefined,
  ) {
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
