/**
 * A project activity is a way to link a project, workflow and activity together.
 */
export interface IProjectActivity {
  id?: number;
  projectId?: number;
  projectWorkflowId?: number;
  activityId: number;
  rowVersion?: number;
}
