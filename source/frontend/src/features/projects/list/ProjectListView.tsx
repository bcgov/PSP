import { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import ProjectIcon from '@/assets/images/projects-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { StyledAddButton } from '@/components/common/styles';
import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import { useApiProjects } from '@/hooks/pims-api/useApiProjects';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import useKeycloakWrapper, { IUserInfo } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { getUserRegionsOptions } from '@/utils/formUtils';
import { exists, formatGuid } from '@/utils/utils';

import { IProjectFilter } from '../interfaces';
import { ProjectFilterModel } from './ProjectFilter/models/ProjectFilterModel';
import ProjectFilter from './ProjectFilter/ProjectFilter';
import { ProjectSearchResultModel } from './ProjectSearchResults/models';
import { ProjectSearchResults } from './ProjectSearchResults/ProjectSearchResults';

/**
 * Page that displays Project files information.
 */
export const ProjectListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { searchProjects } = useApiProjects();
  const { hasClaim, obj } = useKeycloakWrapper();
  const { sub } = obj.userInfo as IUserInfo;
  const formattedGuid = formatGuid(sub);
  const [userRegionsOptions, setUserRegionsOptions] = useState<MultiSelectOption[]>(null);

  const history = useHistory();

  const lookupCodes = useLookupCodeHelpers();
  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();

  const pimsRegionsTypes = lookupCodes.getOptionsByType(API.REGION_TYPES);
  const pimsRegionOptions: MultiSelectOption[] = pimsRegionsTypes.map<MultiSelectOption>(x => {
    return { id: x.code as string, text: x.label };
  });

  const {
    results,
    filter,
    sort,
    totalItems,
    currentPage,
    totalPages,
    pageSize,
    setFilter,
    setSort,
    setCurrentPage,
    setPageSize,
    loading,
  } = useSearch<ApiGen_Concepts_Project, IProjectFilter>(
    new ProjectFilterModel(userRegionsOptions).toApi(),
    searchProjects,
    'No matching results can be found. Try widening your search criteria.',
  );

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: IProjectFilter) => {
      setFilter(filter);
    },
    [setFilter],
  );

  const handleResetFilter = useCallback(() => {
    setFilter(new ProjectFilterModel(userRegionsOptions).toApi());
  }, [setFilter, userRegionsOptions]);

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

  useEffect(() => {
    if (
      userRegionsOptions === null &&
      exists(retrieveUserInfoResponse) &&
      exists(pimsRegionsTypes)
    ) {
      const userRegionsOptions = getUserRegionsOptions(
        retrieveUserInfoResponse?.userRegions,
        pimsRegionsTypes,
      );
      setUserRegionsOptions(userRegionsOptions);
      setFilter(new ProjectFilterModel(userRegionsOptions).toApi());
    }
  }, [pimsRegionsTypes, retrieveUserInfoResponse, setFilter, userRegionsOptions]);

  return (
    <CommonStyled.ListPage>
      <CommonStyled.PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <ProjectIcon title="Project icon" fill="currentColor" />
              <span className="ml-2">Projects</span>
            </div>

            {hasClaim(Claims.PROJECT_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/project/new')}>
                <FaPlus />
                &nbsp;Create Project
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <ProjectFilter
                initialValues={ProjectFilterModel.fromApi(filter, userRegionsOptions)}
                pimsRegionsOptions={pimsRegionOptions}
                setFilter={changeFilter}
                onResetFilter={handleResetFilter}
              />
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <ProjectSearchResults
          results={results.map(x => ProjectSearchResultModel.fromApi(x))}
          totalItems={totalItems}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          loading={loading}
        ></ProjectSearchResults>
      </CommonStyled.PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default ProjectListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;

  svg {
    vertical-align: baseline;
  }
`;
