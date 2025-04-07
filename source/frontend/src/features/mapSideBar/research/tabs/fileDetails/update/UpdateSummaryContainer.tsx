import { Formik, FormikProps } from 'formik';
import { forwardRef } from 'react';

import * as API from '@/constants/API';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { useUpdateResearch } from '../../../hooks/useUpdateResearch';
import ResearchStatusUpdateSolver from '../ResearchStatusUpdateSolver';
import { UpdateResearchSummaryFormModel } from './models';
import { UpdateResearchFileYupSchema } from './UpdateResearchFileYupSchema';
import UpdateResearchForm from './UpdateSummaryForm';

export interface IUpdateResearchViewProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  onSuccess: () => void;
}

export const UpdateResearchContainer = forwardRef<FormikProps<any>, IUpdateResearchViewProps>(
  (props, formikRef) => {
    const { updateResearchFile } = useUpdateResearch();
    const { setModalContent, setDisplayModal } = useModalContext();
    const { getByType } = useLookupCodeHelpers();

    const fileStatusTypeCodes = getByType(API.RESEARCH_FILE_STATUS_TYPES);

    const saveResearchFile = async (researchFile: ApiGen_Concepts_ResearchFile) => {
      const response = await updateResearchFile(researchFile);
      if (typeof formikRef === 'function' || formikRef === null) {
        throw Error('unexpected ref prop');
      }
      formikRef.current?.setSubmitting(false);
      if (response?.fileName) {
        formikRef.current?.resetForm();
        props.onSuccess();
      }
    };

    return (
      <Formik<UpdateResearchSummaryFormModel>
        enableReinitialize
        innerRef={formikRef}
        validationSchema={UpdateResearchFileYupSchema}
        initialValues={UpdateResearchSummaryFormModel.fromApi(props.researchFile)}
        onSubmit={async (values: UpdateResearchSummaryFormModel) => {
          const updatedResearchFile: ApiGen_Concepts_ResearchFile = values.toApi();
          const currentFileSolver = new ResearchStatusUpdateSolver(props.researchFile);
          const updatedFileSolver = new ResearchStatusUpdateSolver(updatedResearchFile);

          if (!currentFileSolver.isAdminProtected() && updatedFileSolver.isAdminProtected()) {
            const statusCode = fileStatusTypeCodes.find(x => x.id === values.statusTypeCode);
            setModalContent({
              variant: 'info',
              title: 'Confirm status change',
              message: (
                <>
                  <p>
                    You marked this file as {statusCode.name}. If you save it, only the
                    administrator can turn it back on. You will still see it in the management
                    table.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: async () => {
                await saveResearchFile(updatedResearchFile);
                setDisplayModal(false);
              },
              handleCancel: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          } else {
            await saveResearchFile(updatedResearchFile);
          }
        }}
      >
        {formikProps => (
          <StyledFormWrapper>
            <UpdateResearchForm formikProps={formikProps} />
          </StyledFormWrapper>
        )}
      </Formik>
    );
  },
);

export default UpdateResearchContainer;
