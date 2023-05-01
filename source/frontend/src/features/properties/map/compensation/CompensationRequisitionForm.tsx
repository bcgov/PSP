import { Api_Compensation } from 'models/api/Compensation';

import { AcquisitionForm } from '../acquisition/add/models';
import { CompensationRequisitionFormModel } from './models';

export interface ICompensationRequisitionFormProps {
  compensation: CompensationRequisitionFormModel;
  acquisitionFile: AcquisitionForm;
  editMode: boolean;
  isEditable: boolean;
  setEditMode: (editMode: boolean) => void;
  onSave: (compensation: Api_Compensation) => Promise<Api_Compensation | undefined>;
}
