import truncate from 'lodash/truncate';
import { FaCheck, FaTimesCircle } from 'react-icons/fa';
import styled, { useTheme } from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';

import { UploadResponseModel } from './models';

export interface IShapeUploadResultViewProps {
  uploadResult: UploadResponseModel;
}

export const ShapeUploadResultView: React.FunctionComponent<IShapeUploadResultViewProps> = ({
  uploadResult,
}) => {
  const theme = useTheme();

  return (
    <>
      {uploadResult.isSuccess ? (
        <StyledSuccessSection label="Shapefile uploaded successfully" labelWidth={{ xs: 12 }}>
          {truncate(uploadResult.fileName ?? '', { length: 100 })}
          <FaCheck
            data-testid="file-check-icon"
            className="ml-2"
            size="1.6rem"
            color={theme.bcTokens.iconsColorSuccess}
          />
        </StyledSuccessSection>
      ) : (
        <StyledFailSection label="Shapefile upload failed" labelWidth={{ xs: 12 }}>
          {truncate(uploadResult.fileName ?? '', { length: 100 })}
          <FaTimesCircle
            data-testid="file-error-icon"
            className="ml-2"
            size="1.6rem"
            color={theme.bcTokens.iconsColorDanger}
          />
          <div className="pt-2">{uploadResult.errorMessage ?? ''}</div>
        </StyledFailSection>
      )}
    </>
  );
};

export default ShapeUploadResultView;

const StyledSuccessSection = styled(SectionField)`
  color: ${props => props.theme.bcTokens.iconsColorSuccess};
`;

const StyledFailSection = styled(SectionField)`
  color: ${props => props.theme.bcTokens.iconsColorDanger};
`;
