# Stage 1: Build the Angular application
FROM node:23-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build-prod

# Stage 2: Serve the application with Nginx
FROM nginx:alpine

# Copy the built application from the 'build' stage to the Nginx server's html directory
COPY --from=build /app/dist/apps/zfinance/browser /usr/share/nginx/html

# Copy nginx configuration as template
COPY ./nginx.conf /etc/nginx/nginx.conf.template

# Copy the entrypoint script
COPY ./docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

# Set default environment variables
ENV BACKEND_HOST=localhost
ENV BACKEND_PORT=8080

EXPOSE 80

ENTRYPOINT ["/docker-entrypoint.sh"]
