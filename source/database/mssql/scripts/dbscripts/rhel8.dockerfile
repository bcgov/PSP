FROM mcr.microsoft.com/mssql/rhel/server:2019-CU6-rhel-8

RUN mkdir -p /usr/config
WORKDIR /usr/config
COPY . /usr/config

RUN sed -i 's/5433/1433/g' .env

USER root
WORKDIR /usr/config/scripts/dbscripts

RUN chmod 777 upgrade.sh &&\
    chmod 777 db-deploy-transaction.sh &&\
    chmod 777 db-deploy.sh;


ENV PATH="/opt/mssql-tools/bin:$PATH"


ENTRYPOINT ["./entrypoint.sh"]

EXPOSE 1433
VOLUME ["/var/opt/data"]
