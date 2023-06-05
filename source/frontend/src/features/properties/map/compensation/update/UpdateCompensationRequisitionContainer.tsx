import { FormikProps } from 'formik';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';
import { useCallback, useEffect, useState } from 'react';

import { CompensationRequisitionFormModel, PayeeOption } from '../models';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: Api_Compensation;
  acquisitionFile: Api_AcquisitionFile;
  formikRef: React.Ref<FormikProps<CompensationRequisitionFormModel>>;
  onSuccess: () => void;
  onCancel: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, acquisitionFile, formikRef, onSuccess, onCancel, View }) => {
  const {
    updateCompensationRequisition: { execute: updateCompensationRequisition, loading: isUpdating },
  } = useCompensationRequisitionRepository();

  const {
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
    getAcquisitionFileSolicitors: {
      execute: retrieveAcquisitionFileSolicitors,
      loading: loadingAcquisitionFileSolicitors,
    },
    getAcquisitionFileRepresentatives: {
      execute: retrieveAcquisitionFileRepresentatives,
      loading: loadingAcquisitionFileRepresentatives,
    },
  } = useAcquisitionProvider();

  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);

  const updateCompensation = async (compensation: CompensationRequisitionFormModel) => {
    const compensationApiModel = compensation.toApi(payeeOptions);

    const result = await updateCompensationRequisition(compensationApiModel);
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  const fetchContacts = useCallback(async () => {
    if (acquisitionFile.id) {
      const acquisitionOwnersCall = retrieveAcquisitionOwners(acquisitionFile.id);
      const acquisitionSolicitorsCall = retrieveAcquisitionFileSolicitors(acquisitionFile.id);
      const acquisitionRepresentativesCall = retrieveAcquisitionFileRepresentatives(
        acquisitionFile.id,
      );

      await Promise.all([
        acquisitionOwnersCall,
        acquisitionSolicitorsCall,
        acquisitionRepresentativesCall,
      ]).then(([acquisitionOwners, acquisitionSolicitors, acquisitionRepresentatives]) => {
        const options = payeeOptions;

        if (acquisitionOwners !== undefined) {
          const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
            PayeeOption.createOwner(x),
          );
          options.push(...ownersOptions);
        }

        if (acquisitionSolicitors !== undefined) {
          const acquisitionSolicitorOptions: PayeeOption[] = acquisitionSolicitors.map(x =>
            PayeeOption.createOwnerSolicitor(x),
          );
          options.push(...acquisitionSolicitorOptions);
        }

        if (acquisitionRepresentatives !== undefined) {
          const acquisitionSolicitorOptions: PayeeOption[] = acquisitionRepresentatives.map(x =>
            PayeeOption.createOwnerRepresentative(x),
          );
          options.push(...acquisitionSolicitorOptions);
        }

        const teamMemberOptions: PayeeOption[] =
          acquisitionFile.acquisitionTeam
            ?.filter((x): x is Api_AcquisitionFilePerson => !!x)
            .filter(x => x.personProfileTypeCode === 'MOTILAWYER')
            .map(x => PayeeOption.createTeamMember(x)) || [];
        options.push(...teamMemberOptions);

        setPayeeOptions(options);
      });
    }
  }, [
    payeeOptions,
    acquisitionFile.acquisitionTeam,
    acquisitionFile.id,
    retrieveAcquisitionOwners,
    retrieveAcquisitionFileSolicitors,
    retrieveAcquisitionFileRepresentatives,
  ]);

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  return (
    <View
      isLoading={
        isUpdating ||
        loadingAcquisitionOwners ||
        loadingAcquisitionFileSolicitors ||
        loadingAcquisitionFileRepresentatives
      }
      formikRef={formikRef}
      initialValues={CompensationRequisitionFormModel.fromApi(compensation)}
      payeeOptions={payeeOptions}
      acquisitionFile={acquisitionFile}
      onSave={updateCompensation}
      onCancel={onCancel}
    />
  );
};

export default UpdateCompensationRequisitionContainer;
