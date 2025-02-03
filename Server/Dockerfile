FROM node:20-alpine

RUN mkdir -p /usr/src/server-dragon-ball
WORKDIR /usr/src/server-dragon-ball
COPY . .

RUN npm install

EXPOSE 3000 

ENTRYPOINT ["npm", "run", "start"]