FROM node:24-alpine AS builder

WORKDIR /app
COPY . .
RUN npm install && npm run build

FROM nginx:alpine

COPY --from=builder /app/build /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
