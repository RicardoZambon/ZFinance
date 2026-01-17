#!/bin/sh

# Substitui os placeholders pelas variáveis de ambiente
envsubst '${BACKEND_HOST} ${BACKEND_PORT}' < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf

# Inicia o Nginx
exec nginx -g 'daemon off;'
