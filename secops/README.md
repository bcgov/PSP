## DevSecOps

The purpose and intent of DevSecOps is to build on the mindset that “everyone is responsible for security” to safely distribute security decisions at speed and scale to those who hold the highest level of context without sacrificing the safety required. – Shannon Lietz

![](../Screenshots/DevSecOps.PNG)

The PIMS Project undertake vulnerability scan as part of our software release pipeline, and do not release if "HIGH RISK" vulnerabilities are identified (Automated, Continous process)

**CODE:** 
Run Static Code Analysis in Real-time to address vulnerabilities in the code at real-time. We do not have to wait once per quarter or wait until the release to production before scanning. Open-source tools for Static Code Analysis can be used within the CI pipeline and Git Actions:

- [Sonarque](https://docs.sonarqube.org/latest/analysis/github-integration/)
- [CodeQL](https://github.com/github/codeql-action)


#### Static Code Analysis for React Frontend

The Project uses Sonarque as a Static Code Analysis and Quality Assurance Tool to collect and analyses our source code and provide reports for the code quality of our project

**Requirement:
- [Sonarque Server](https://github.com/BCDevOps/sonarqube) (community version - free)
- Sonarque cli for Javascript/Typescript (github action)

*GitHub Actions for PIMS Frontend Static Code Analysis*

![](../Screenshots/sonareact.png)

***Set Quality Gate to TRUE to failed merge on PR***

If this is set, sonar scan quality gate must be passed before merge into the master branch
 
#### Pull Request Decoration
- [Quality Gate](https://docs.sonarqube.org/latest/user-guide/quality-gates/) and Metrics in the PR!
- Live update in any issue chanage
- PR status update (merge block)

![](../Screenshots/pr.PNG)

Click on the link will redirect you the Sonarque Scanner Quality Gate reports as below

![](../Screenshots/qgate.PNG)



### Workflow

![](../Screenshots/workflow.png)