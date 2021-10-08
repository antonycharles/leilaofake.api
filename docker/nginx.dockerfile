FROM nginx:latest
LABEL Antony Charles
COPY /docker/config/nginx.conf /etc/nginx/nginx.conf
COPY /docker/certificates/certificate_postgresql.crt /etc/nginx/certificate_postgresql.crt
COPY /docker/certificates/private_postgresql.key /etc/nginx/private_postgresql.key
COPY /docker/certificates/certificate_leilaofake.crt /etc/nginx/certificate_leilaofake.crt
COPY /docker/certificates/private_leilaofake.key /etc/nginx/private_leilaofake.key
EXPOSE 80 443
ENTRYPOINT ["nginx"]
# Parametros extras para o entrypoint
CMD ["-g", "daemon off;"]
