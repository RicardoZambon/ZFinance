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

# Copy a custom nginx configuration
COPY ./nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
