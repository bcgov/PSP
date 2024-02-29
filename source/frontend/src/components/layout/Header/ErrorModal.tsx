import { useDispatch } from 'react-redux';
import styled from 'styled-components';

import GenericModal from '@/components/common/GenericModal';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledGreySection } from '@/features/documents/commonStyles';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { logClear } from '@/store/slices/network/networkSlice';

export interface IErrorModalProps {
  // An array of network action errors.
  errors: IGenericNetworkAction[];
  // Whether to show the modal.
  show?: boolean;
  // Change the visible state.
  setShow: (show: boolean) => void;
}

/**
 * ErrorModal component that displays an error message and information in a modal.
 * @param param0 ErrorModal component properties.
 * @param param0.errors An array of errors.
 * @param [param0.show] Whether to show the model.
 * @param param0.setShow A function to set the show value.
 * @returns ErrorModal component.
 */
export const ErrorModal = ({ errors, show, setShow }: IErrorModalProps) => {
  const dispatch = useDispatch();
  const handleClose = () => {
    setShow(false);
  };
  const handleClear = () => {
    errors.forEach(error => dispatch(logClear(error.name)));
    setShow(false);
  };

  const errorUrl = (error: IGenericNetworkAction): string => {
    return error?.error?.response?.config?.url || '';
  };

  const errorShortUrl = (error: IGenericNetworkAction): string => {
    const url = errorUrl(error);
    if (url.length > 20) {
      return error?.error?.response?.config?.url?.substr(0, 20) + '...';
    } else {
      return url;
    }
  };

  const errorStatus = (error: IGenericNetworkAction): string => {
    return error?.error?.response?.statusText || 'unknown';
  };

  return (
    <GenericModal
      variant="error"
      display={show}
      handleCancel={handleClose}
      title="Errors"
      okButtonText="Close"
      handleOk={handleClear}
      message={
        <ErrorWrapper>
          {errors.map((error: IGenericNetworkAction, index: number) => (
            <ErrorEntry key={index}>
              {process.env.NODE_ENV === 'development' ? (
                <Section header={errorShortUrl(error)} isCollapsable>
                  <SectionField label="Status" labelWidth="2">
                    <ErrorDescription>{errorStatus(error)}</ErrorDescription>
                  </SectionField>
                  <SectionField label="Path" labelWidth="2">
                    <ErrorDescription>{errorUrl(error)}</ErrorDescription>
                  </SectionField>
                  <SectionField label="Data" labelWidth="2">
                    <ErrorDescription>
                      {JSON.stringify(error?.error?.response?.data)}
                    </ErrorDescription>
                  </SectionField>
                </Section>
              ) : (
                <Section header={errorShortUrl(error)} isCollapsable>
                  <SectionField label="Status" labelWidth="3">
                    <ErrorDescription>{errorStatus(error)}</ErrorDescription>
                  </SectionField>
                  <SectionField label="Path" labelWidth="3">
                    <ErrorDescription>{errorUrl(error)}</ErrorDescription>
                  </SectionField>
                  <SectionField label="Data" labelWidth="12">
                    <ErrorDescription>
                      {(error?.error?.response?.data as unknown & { error: string })?.error ?? ''}
                    </ErrorDescription>
                  </SectionField>
                </Section>
              )}
            </ErrorEntry>
          ))}
        </ErrorWrapper>
      }
    />
  );
};

export default ErrorModal;

const ErrorWrapper = styled(StyledGreySection)`
  max-height: 70rem;
  overflow-y: scroll;
  width: 100%;
  padding: 0;
`;

const ErrorEntry = styled.div``;

const ErrorDescription = styled.div`
  word-break: break-all;
`;
