import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { exists } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { ManagementTeamSubFormModel, WithManagementTeam } from './ManagementTeamSubFormModel';

export class ManagementFormModel implements WithManagementTeam {
  fileName: string | null = '';
  additionalDetails: string | null = '';
  filePurpose: string | null = '';
  legacyFileNum: string | null = '';
  fileStatusTypeCode: string | null = null;
  project: IAutocompletePrediction | null = null;
  productId: string | null = null;
  fundingTypeCode: string | null = null;
  programTypeCode: string | null = null;
  fileProperties: PropertyForm[] = [];
  team: ManagementTeamSubFormModel[] = [];

  constructor(
    readonly id: number | null = null,
    readonly fileNumber: string | null = null,
    readonly rowVersion: number | null = null,
    managementFileStatus = 'ACTIVE',
  ) {
    this.id = id;
    this.fileNumber = fileNumber;
    this.fileStatusTypeCode = managementFileStatus;
  }

  toApi(): ApiGen_Concepts_ManagementFile {
    return {
      id: this.id ?? 0,
      fileName: this.fileName ?? null,
      additionalDetails: this.additionalDetails ?? null,
      filePurpose: this.filePurpose ?? null,
      fileNumber: this.fileNumber ?? null,
      legacyFileNum: this.legacyFileNum ?? null,
      fileStatusTypeCode: toTypeCodeNullable(this.fileStatusTypeCode),
      totalAllowableCompensation: null,
      project: null,
      projectId: this.project?.id !== undefined && this.project?.id !== 0 ? this.project?.id : null,
      product: null,
      productId: this.productId ? Number(this.productId) : null,
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      programTypeCode: toTypeCodeNullable(this.programTypeCode),
      managementTeam: this.team
        .filter(x => !!x.contact && !!x.teamProfileTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter(exists),
      fileProperties: this.fileProperties.map(x => this.toPropertyApi(x)),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  private toPropertyApi(x: PropertyForm): ApiGen_Concepts_ManagementFileProperty {
    const apiFileProperty = x.toFilePropertyApi(this.id);
    return {
      ...apiFileProperty,
      file: null,
    };
  }

  static fromApi(model: ApiGen_Concepts_ManagementFile): ManagementFormModel {
    const managementForm = new ManagementFormModel(
      model.id,
      model.fileNumber,
      model.rowVersion,
      model.fileStatusTypeCode?.id ?? undefined,
    );

    managementForm.additionalDetails = model.additionalDetails ?? '';
    managementForm.filePurpose = model.filePurpose ?? '';
    managementForm.project = model.project
      ? { id: model.project?.id || 0, text: model.project?.description || '' }
      : null;
    managementForm.productId = model.product?.id?.toString() ?? '';
    managementForm.fundingTypeCode = fromTypeCode(model.fundingTypeCode) ?? '';
    managementForm.fileName = model.fileName ?? '';
    managementForm.team =
      model.managementTeam?.map(x => ManagementTeamSubFormModel.fromApi(x)) || [];
    managementForm.fileProperties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    managementForm.programTypeCode = fromTypeCode(model.programTypeCode) ?? '';

    return managementForm;
  }
}
