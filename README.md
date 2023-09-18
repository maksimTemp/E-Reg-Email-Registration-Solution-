# E-Reg (Email Registration Solution)

EReg - это веб-приложение для регистрации пользователей по электронной почте. 

## Требования к системе
Для успешной установки и запуска EReg требуются следующие компоненты:

- .NET Core 7;
- Node.js (версия 20.6.1 и выше);
- npm (устанавливается вместе с Node.js);
- Docker (для запуска RabbitMQ).


## Конфигурации

В файле ERegMailServer/appsettings.json можно настроить следующие параметры:
- RabbitMQ: параметры подключения к RabbitMQ;
- SMTP: параметры подключения к SMTP серверу для отправки электронных писем.

В файле ERegWeb/appsettings.json можно настроить следующие параметры:
- DefaultConnection: параметры подключения к базе данных;
- RabbitMQ: параметры подключения к RabbitMQ;
- Rate Limiting Middleware: парметры контроля скорости запросов к веб-приложению или API.

## Запуск

1. Для работы Backend требуется одновременный запуск проектов **ERegServer** и **ERegWeb**. 
2. Для запуска Frontend требуется запуск **ERegUI**:
```shell
cd ERegUI
npm start
```
## Использование

После запуска приложения вы сможете зарегистрировать пользователя по электронной почте и подтвердить его кодом, а также получить уведомления о результатах регистрации.

## Примечания

- Приложение Frontend будет доступно по адресу http://localhost:3000.
- Примечание: Миграция базы данных выполняется автоматически при запуске приложения Backend.
