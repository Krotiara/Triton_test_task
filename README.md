# Triton_test_task
in working

Описание
	 Дан эмулятор устройств. Эмулятор отправляет и получает UDP-пакеты: порт
отправки 62006, порт приема 62005. В эмуляторе можно добавить ограниченное
количество устройств. Каждое устройство отправляет UDP-пакет данных на
широковещательный адрес каждые три секунды. 
Пакет данных: 
4 байта — идентификатор устройства
2 байта — числовое параметр #1 (0-100)
2 байта — числовое параметр #2 (0-255)

Каждое устройство имеет пороги допустимых значений для числового
параметра #2. Считывание порогов производится после отправки запроса на
эмулятор. 

Запрос на чтение порогов устройства: 
4 байта — идентификатор устройства
2 байта — команда на чтение «LR»

Ответ на запрос: 
4 байта — идентификатор устройства
2 байта — команда на чтение «LR»
2 байта — статус ответа
2 байта — значение верхнего порога
2 байта — значение нижнего порога

Пороги допустимых значений устройства можно изменить.
Запрос на изменение порогов устройства:
4 байта — идентификатор устройства
2 байта — команда на чтение «LW»
2 байта — значение верхнего порога
2 байта — значение нижнего порога

Ответ на запрос:
4 байта — идентификатор устройства
2 байта — команда на чтение «LW»
2 байта — статус ответа
2 байта — значение верхнего порога
2 байта — значение нижнего порога

Возможные статусы ответа:
0x0000 — Successes 
0x000F — WrongData
0x00FF — Unknown Error 

Задание
2. Отобразить на веб-странице все устройства, их текущие значения и пороги в
режиме реального времени. Обновление значений должно происходить без
обновления страницы. Если значение параметра #2 выходит за границы порога, 
значение окрашивать в красный цвет.
2.1 (опционально) Реализовать возможность смены порогов определенному
устройству
