FROM rabbitmq:3
RUN echo 'loopback_users.guest = false' >> /etc/rabbitmq/rabbitmq.conf 
RUN echo 'listeners.tcp.default = 5672' >> /etc/rabbitmq/rabbitmq.conf
RUN echo 'management.tcp.port = 15672' >> /etc/rabbitmq/rabbitmq.conf
RUN echo 'log.file.level = debug' >> /etc/rabbitmq/rabbitmq.conf
RUN echo 'log.syslog = true' >> /etc/rabbitmq/rabbitmq.conf
RUN echo 'log.syslog.host = syslog' >> /etc/rabbitmq/rabbitmq.conf
RUN echo 'log.syslog.port = 514' >> /etc/rabbitmq/rabbitmq.conf