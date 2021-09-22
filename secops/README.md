## DevSecOps

The purpose and intent of DevSecOps is to build on the mindset that “everyone is responsible for security” to safely distribute security decisions at speed and scale to those who hold the highest level of context without sacrificing the safety required. – Shannon Lietz

![Design Pattern](../Screenshots/DevSecOps.PNG)

The PIMS Project undertake vulnerability scan as part of our software release pipeline, and do not release if "HIGH RISK" vulnerabilities are identified (Automated, Continous process)

**CODE:** 
Run Static Code Analysis in Real-time to address vulnerabilities in the code at real-time. We do not have to wait once per quarter or wait until the release to production before scanning. Open-source tools for Static Code Analysis can be used within the CI pipeline and Git Actions:

- [Sonarque](https://docs.sonarqube.org/latest/analysis/github-integration/)
- [CodeQL](https://github.com/github/codeql-action)

#### Pull Request Decoration
- Quality Gate and Metrics in the PR!
- Live update in any issue chanage
- PR status update (merge block)

### Workflow

![Design Pattern](../Screenshots/workflow.png)