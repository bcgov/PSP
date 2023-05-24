#!/usr/bin/make

SHELL := /usr/bin/env bash
.DEFAULT_GOAL := help

ifneq ($(OS),Windows_NT)
POSIXSHELL := 1
else
POSIXSHELL :=
endif

# to see all colors, run
# bash -c 'for c in {0..255}; do tput setaf $c; tput setaf $c | cat -v; echo =$c; done'
# the first 15 entries are the 8-bit colors

# define standard colors
BLACK        := $(shell tput -Txterm setaf 0)
RED          := $(shell tput -Txterm setaf 1)
GREEN        := $(shell tput -Txterm setaf 2)
YELLOW       := $(shell tput -Txterm setaf 3)
LIGHTPURPLE  := $(shell tput -Txterm setaf 4)
PURPLE       := $(shell tput -Txterm setaf 5)
BLUE         := $(shell tput -Txterm setaf 6)
WHITE        := $(shell tput -Txterm setaf 7)

RESET := $(shell tput -Txterm sgr0)

# default "prompt"
P = ${GREEN}[+]${RESET}

help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' Makefile | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

.PHONY: help

# these are various tools required by make commands below
GH := $(shell command -v gh 2> /dev/null)
PYTHON := $(shell command -v python 2> /dev/null)
NODE := $(shell command -v node 2> /dev/null)
JQ := $(shell command -v jq 2> /dev/null)
GIT := $(shell command -v git 2> /dev/null)


##############################################################################
# Dev Tools Requirements
##############################################################################
.PHONY: require-node
require-node:
ifndef NODE
	$(error "NodeJS is not available please install from https://nodejs.org/en/")
endif
	@exit 0

.PHONY: require-python
require-python:
ifndef PYTHON
	$(error "python3 is not available please install from https://docs.python.org/3")
endif
	@exit 0

.PHONY: require-jq
require-jq:
ifndef JQ
	$(error "jq is not available please install from https://stedolan.github.io/jq")
endif
	@exit 0

.PHONY: require-gh
require-gh:
ifndef GH
	$(error "gh (GitHub CLI) is not available please install from https://cli.github.com/")
endif
	@exit 0

require-git:
ifndef GIT
	$(error "GIT is not available please install from https://git-scm.com/downloads")
endif
	@exit 0

.PHONY: requirements
requirements: require-git require-node require-python require-jq require-gh ## Checks development environment requirements (tools in the PATH)
	@echo "git found..."
	@echo "node found..."
	@echo "python3 found..."
	@echo "jq found..."
	@echo "gh cli found..."

.PHONY: github-auth
github-auth: require-gh ## Checks GITHUB_TOKEN has been set
# look for GITHUB_TOKEN in the environment
ifdef GITHUB_TOKEN
	$(info GITHUB_TOKEN found in the environment)
else
	$(info GITHUB_TOKEN not set, you might need to authenticate manually to use GH CLI)
endif
	@exit 0

##############################################################################
# Release Management
##############################################################################
.PHONY: version
version: require-node ## Outputs the latest released version
	@tools/cicd/build/version.sh

.PHONY: version-next
version-next: require-node ## Outputs the next unreleased version
	@tools/cicd/build/version-next.sh

.PHONY: bump
bump: require-node ## Bumps the version number
ifndef $(ARGS)
	$(eval ARGS := --build)
endif
	@tools/cicd/build/bump.sh $(ARGS) --apply

branch = HEAD

.PHONY: tag
tag: require-node require-git require-gh ## Creates a pre-release tag
	$(eval CURRENT_VERSION := $(shell tools/cicd/build/version.sh))
	$(eval tag := v$(CURRENT_VERSION))
	$(eval owner := $(shell gh repo view --json owner --jq '.owner.login'))
	$(eval repo := $(shell gh repo view --json name --jq '.name'))

	$(info Using tag: $(tag))
	$(info Using repo: $(owner)/$(repo))
	@git tag -a -f $(tag) $(branch) -m "Release $(tag)"
	@git push -f --tags --no-verify

.PHONY: release
release: github-auth require-node require-gh ## Creates a new github release
	$(eval CURRENT_VERSION := $(shell tools/cicd/build/version.sh))
	$(eval CURRENT_DATE := $(shell date +'%b %d, %Y'))
	$(eval tag := v$(CURRENT_VERSION))
	$(eval owner := $(shell gh repo view --json owner --jq '.owner.login'))
	$(eval repo := $(shell gh repo view --json name --jq '.name'))

	$(info Releasing version $(CURRENT_VERSION))
	$(info Using tag: $(tag))
	$(info Using previous_tag: $(previous_tag))
	$(info Using repo: $(owner)/$(repo))

ifdef previous_tag
	gh release create $(tag) -R $(owner)/$(repo) --title $(tag) --generate-notes --notes-start-tag $(previous_tag)
else
	gh release create $(tag) -R $(owner)/$(repo) --title $(tag) --generate-notes
endif

##############################################################################
# DevSecOps
##############################################################################
# python is required for DevSecOps tools

.PHONY: devops-install
devops-install: ## Installs software required by DevSecOps tooling (e.g. python, etc)
	@if [ -z "$(PYTHON)" ]; then echo "Python not found. Install from https://docs.python.org/3"; exit 2; fi
	@if [ -z "$(JQ)" ]; then echo "JQ not found. Install from https://stedolan.github.io/jq/"; exit 2; fi
	@python -m ensurepip --upgrade
	@pip install trufflehog3 jtbl

.PHONY: devops-scan
devops-scan: | devops-install ## Scans the repo for accidental leaks of passwords/secrets
	@echo "$(P) Scanning codebase for leaked passwords and secrets..."
	-@trufflehog3 --no-history --config .github/.trufflehog3.yml --format json --output trufflehog_report.json
	@echo "$(P) Generating HTML report..."
	@trufflehog3 -R trufflehog_report.json --output trufflehog_report.html
	@echo "$(P) HTML report saved to trufflehog_report.html"
	@echo
	@./tools/cicd/build/secops_report.sh trufflehog_report.json

##############################################################################
# Docker Development
##############################################################################

restart: | stop build up ## Restart local docker environment

refresh: | down build up ## Recreates local docker environment

.PHONY: infra
infra: ## Starts infrastructure containers (e.g. database, geoserver). Useful for local debugging
	@echo "$(P) Starting up infrastructure containers..."
	@"$(MAKE)" start n="database geoserver"

start: ## Starts the local containers (n=service name)
	@echo "$(P) Starting client and server containers..."
	@docker-compose start $(n)

up: ## Runs the local containers (n=service name)
	@echo "$(P) Running client and server..."
	@docker-compose up -d --no-recreate $(n)

destroy: ## Stops the local containers and removes them (n=service name)
	@echo "$(P) Removing docker containers..."
	@docker-compose rm -s -f $(n)

down: ## Stops the local containers and removes them
	@echo "$(P) Stopping client and server..."
	@docker-compose down

stop: ## Stops the local containers (n=service name)
	@echo "$(P) Stopping client and server..."
	@docker-compose stop $(n)

build: ## Builds the local containers (n=service name)
	@echo "$(P) Building images..."
	@docker-compose build --no-cache $(n)

rebuild: ## Build the local contains (n=service name) and then start them after building
	@"$(MAKE)" build n=$(n)
	@"$(MAKE)" up n=$(n)

clean: ## Removes all local containers, images, volumes, etc
	@echo "$(P) Removing all containers, images, volumes for solution."
	@docker-compose rm -f -v -s
	@docker volume rm -f psp-frontend-node-cache
	@docker volume rm -f psp-api-db-data

logs: ## Shows logs for running containers (n=service name)
	@docker-compose logs -f $(n)

setup: ## Setup local container environment, initialize keycloak and database
	@"$(MAKE)" build; make up; make pause-30; make db-update; make db-seed; make keycloak-sync;

pause-30:
	@echo "$(P) Pausing 30 seconds..."
	@-sleep 30

client-test: ## Runs the client tests in a container
	@echo "$(P) Running client unit tests..."
	@docker-compose run frontend npm test

server-test: ## Runs the server tests in a container
	@echo "$(P) Running server unit tests..."
	@docker-compose run backend dotnet test

server-run: ## Starts local server containers
	@echo "$(P) Starting server containers..."
	@docker-compose up -d backend

npm-clean: ## Removes local containers, images, volumes, for frontend application.
	@echo "$(P) Removing frontend containers and volumes."
	@docker-compose stop frontend
	@docker-compose rm -f -v -s frontend
	@docker volume rm -f psp-frontend-node-cache

npm-refresh: ## Cleans and rebuilds the frontend.  This is useful when npm packages are changed.
	@"$(MAKE)" npm-clean; make build n=frontend; make up;

db-refresh: | server-run pause-30 db-seed keycloak-sync ## Refresh the database and seed it with data.

db-clean: ## create a new, clean database using the script file in the database. defaults to using the folder specified in database/mssql/.env, but can be overriden with n=PSP_PIMS_S15_00.
	@echo "$(P) create a clean database with minimal required data for development"
	TARGET_SPRINT=$(n) docker-compose up -d --build database

db-seed: ## create a new, database seeded with test data using the script file in the database. defaults to using the folder specified in database/mssql/.env, but can be overriden with n=PSP_PIMS_S15_00.
	@echo "$(P) Seed the database with test data. n=FOLDER_NAME (PSP_PIMS_S15_00)"
	TARGET_SPRINT=$(n) SEED=TRUE docker-compose up -d --build --force-recreate database;

db-drop: ## Drop the database.
	@echo "$(P) Drop the database."
	@cd source/backend/dal; dotnet ef database drop;

db-deploy:
	@echo "$(P) deployment script that facilitates releasing database changes."
	@cd source/database/mssql/scripts/dbscripts; TARGET_SPRINT=$(n) ./deploy.sh

db-upgrade: ## Upgrade an existing database to the TARGET_VERSION (if passed) or latest version (default), n=TARGET_VERSION (16.01).
	@echo "$(P) Upgrade an existing database to the TARGET_VERSION (if passed) or latest version (default), n=TARGET_VERSION (16.01)"
	@cd source/database/mssql/scripts/dbscripts; TARGET_VERSION=$(n) ./db-upgrade.sh

db-scaffold: ## Requires local install of sqlcmd
	@echo "$(P) regenerate ef core entities from database"
	@cd source/backend/entities; eval $(grep -v '^#' .env | xargs) dotnet ef dbcontext scaffold Name=PIMS Microsoft.EntityFrameworkCore.SqlServer -o ../entities/ef --schema dbo --context PimsBaseContext --context-namespace Pims.Dal --context-dir . --no-onconfiguring --namespace Pims.Dal.Entities --data-annotations -v -f --startup-project ../api

keycloak-sync: ## Syncs accounts with Keycloak and PIMS
	@echo "$(P) Syncing keycloak with PIMS..."
	@cd tools/keycloak/sync; dotnet build; dotnet run;

convert: ## Convert Excel files to JSON
	@echo "$(P) Convert Excel files to JSON..."
	@cd tools/converters/excel; dotnet build; dotnet run;

backend-test: ## Run backend unit tests
	@echo "$(P) Run backend unit tests"
	@cd source/backend; dotnet test;

frontend-test: ## Run frontend unit tests
	@echo "$(P) Run frontend unit tests"
	@cd frontend; npm run test:watch;

backend-coverage: ## Generate coverage report for backend
	@echo "$(P) Generate coverage report for backend"
	@cd source/backend/tests/unit/api; dotnet build;
	@cd source/backend/tests/unit/dal; dotnet build;
	@cd source/backend/tests/unit/mockdal; dotnet build;
	@cd source/backend; coverlet ./tests/unit/api/bin/Debug/net6.0/Pims.Api.Test.dll --target "dotnet" --targetargs "test ./ --no-build" -o "./tests/TestResults/coverage.json" --exclude "[*.Test]*" --exclude "[*]*Model" --exclude-by-attribute "CompilerGenerated" -f json
	@cd source/backend; coverlet ./tests/unit/dal/bin/Debug/net6.0/Pims.Dal.Test.dll --target "dotnet" --targetargs "test ./ --no-build" -o "./tests/TestResults/coverage.xml" --exclude "[*.Test]*" --exclude "[*]*Model" --exclude-by-attribute "CompilerGenerated" --merge-with "tests/TestResults/coverage.json" -f cobertura
	@cd source/backend; coverlet ./tests/unit/mockdal/bin/Debug/net6.0/Pims.Dal.Mock.Test.dll --target "dotnet" --targetargs "test ./ --no-build" -o "./tests/TestResults/coverage.xml" --exclude "[*.Test]*" --exclude "[*]*Model" --exclude-by-attribute "CompilerGenerated" --merge-with "tests/TestResults/coverage.json" -f cobertura
	@cd source/backend; reportgenerator "-reports:./tests/TestResults/coverage.xml" "-targetdir:./tests/TestResults/Coverage" -reporttypes:Html
	@cd source/backend; start ./tests/TestResults/Coverage/index.htm

frontend-coverage: ## Generate coverage report for frontend
	@echo "$(P) Generate coverage report for frontend"
	@cd frontend; npm run coverage;

env: ## Generate env files
	@echo "$(P) Generate/Regenerate env files required for application (generated passwords only match if database .env file does not already exist)"
	@./tools/cicd/scripts/gen-env-files.sh;

mayan-up: ## Calls the docker compose up for the mayan images
	@echo "$(P) Create or start mayan-edms system"
	@cd tools/mayan-edms; docker-compose --profile all up -d

.PHONY: logs start destroy local setup restart refresh up down stop build rebuild clean client-test server-test pause-30 server-run db-clean db-drop db-seed db-refresh db-script db-scaffold npm-clean npm-refresh keycloak-sync convert backend-coverage frontend-coverage backend-test frontend-test env mayan-up

