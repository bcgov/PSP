FROM mcr.microsoft.com/mssql/server:2019-CU27-ubuntu-20.04

RUN mkdir -p /usr/config
WORKDIR /usr/config
COPY . /usr/config

# Make sure GIT has the following permissions set.
# RUN chmod +x ./entrypoint.sh
# RUN chmod +x ./setup.sh
ENTRYPOINT ["./entrypoint.sh"]

EXPOSE 1433
VOLUME ["/var/opt/data"]
