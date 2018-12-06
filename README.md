# Secret santa telegram bot
Title says pretty much all.

## Как это запустить?
### Понадобится
* Docker ([Установщик для Windows](https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe))
* Токен от бота в Telegram (можно взять у @BotFather)
* ngrok ([для Windows](https://bin.equinox.io/c/4VmDzA7iaHb/ngrok-stable-windows-amd64.zip))

### Что делать?
1. Устанавливаем докер
2. Получаем токен от бота
3. Если у тебя статический ip или есть доменное имя, то шаг можно пропустить. Если нет, то распаковываем архив с ngrok, 
открываем командную строку и выполняем: `".\ngrok.exe http 5000"` Запоминаем адрес вида 22331ae2.ngrok.io 
(схема - http или https - не нужна), который появится в аутпуте команды.
4. Клонируем репо
5. В папке Santa создаём файл `secrets.json`, в котором должно быть следующее:
```json
{
  "address":"<адрес из третьего шага или твой адрес>",
  "token":"<токен бота>"
}
```
6. Для запуска бота в корне открываем командную строку и выполняем:
```
docker-compose up --build
```
Для остановки - `docker-compose stop`, для остановки с удалением информации из базы данных - `docker-compose down`.
