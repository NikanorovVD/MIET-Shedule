# Используем официальный образ Node.js версии 18.x
FROM node:24-alpine

# Рабочий каталог
WORKDIR /app

COPY MietSchedule.client/package*.json ./
RUN npm install

COPY MietSchedule.client .
RUN npm run build

EXPOSE 5173

CMD ["npm", "run", "dev"]