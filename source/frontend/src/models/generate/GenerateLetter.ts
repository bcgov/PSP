import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Person } from 'models/api/Person';
import moment from 'moment';

import { Api_GenerateFile } from './GenerateFile';
export class Api_GenerateLetter extends Api_GenerateFile {
  date_generated: string;
  constructor(file: Api_AcquisitionFile, coordinatorContact: Api_Person | null | undefined) {
    super(file, coordinatorContact);
    this.date_generated = moment().format('DD/M/YYYY');
  }
}
