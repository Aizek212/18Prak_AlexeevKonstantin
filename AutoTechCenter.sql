-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Фев 27 2025 г., 22:07
-- Версия сервера: 8.0.30
-- Версия PHP: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `AutoTechCenter`
--

DELIMITER $$
--
-- Процедуры
--
CREATE DEFINER=`root`@`%` PROCEDURE `ValidateEmails` ()   BEGIN
    SELECT 
        ClientID, 
        Email,
        CASE 
            WHEN Email LIKE '%_@__%.__%' THEN 'Valid'
            ELSE 'Invalid'
        END AS EmailStatus
    FROM Clients;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `Cars`
--

CREATE TABLE `Cars` (
  `CarID` int NOT NULL,
  `ClientID` int NOT NULL,
  `Brand` varchar(50) NOT NULL,
  `Model` varchar(50) NOT NULL,
  `Year` int NOT NULL,
  `LicensePlate` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Cars`
--

INSERT INTO `Cars` (`CarID`, `ClientID`, `Brand`, `Model`, `Year`, `LicensePlate`) VALUES
(1, 1, 'Toyota', 'Corolla', 2015, 'A123BC'),
(2, 2, 'Ford', 'Focus', 2018, 'B456DE');

-- --------------------------------------------------------

--
-- Структура таблицы `Clients`
--

CREATE TABLE `Clients` (
  `ClientID` int NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Phone` varchar(15) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Address` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Clients`
--

INSERT INTO `Clients` (`ClientID`, `FirstName`, `LastName`, `Phone`, `Email`, `Address`) VALUES
(1, 'Алексей', 'Горохов', '+7999999999', 'dsa121311@gmail.com', 'Москва, ул. Пушкина д. 22'),
(2, 'Николай', 'Петров', '+78888888888', 'qword22@gmail.com', 'Саранск, ул. Каравая д. 2');

-- --------------------------------------------------------

--
-- Структура таблицы `OrderedServices`
--

CREATE TABLE `OrderedServices` (
  `OrderID` int NOT NULL,
  `ServiceID` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `Orders`
--

CREATE TABLE `Orders` (
  `OrderID` int NOT NULL,
  `ClientID` int NOT NULL,
  `CarID` int NOT NULL,
  `OrderDate` date NOT NULL,
  `CompletionDate` date DEFAULT NULL,
  `Status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Orders`
--

INSERT INTO `Orders` (`OrderID`, `ClientID`, `CarID`, `OrderDate`, `CompletionDate`, `Status`) VALUES
(1, 1, 1, '2025-02-04', '2025-02-23', 'Ожидание'),
(2, 2, 2, '2025-02-02', '2025-02-23', 'В пути');

--
-- Триггеры `Orders`
--
DELIMITER $$
CREATE TRIGGER `TrackOrderStatusChange` AFTER UPDATE ON `Orders` FOR EACH ROW BEGIN
    IF OLD.Status != NEW.Status THEN
        INSERT INTO OrderStatusHistory (OrderID, Status, ChangeDate)
        VALUES (NEW.OrderID, NEW.Status, NOW());
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `OrderStatusHistory`
--

CREATE TABLE `OrderStatusHistory` (
  `HistoryID` int NOT NULL,
  `OrderID` int NOT NULL,
  `Status` varchar(50) NOT NULL,
  `ChangeDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `OrderStatusHistory`
--

INSERT INTO `OrderStatusHistory` (`HistoryID`, `OrderID`, `Status`, `ChangeDate`) VALUES
(1, 1, 'Отменён', '2025-02-27 22:03:48'),
(2, 1, 'Ожидание', '2025-02-27 22:03:59');

-- --------------------------------------------------------

--
-- Структура таблицы `Reviews`
--

CREATE TABLE `Reviews` (
  `ReviewID` int NOT NULL,
  `OrderID` int NOT NULL,
  `Rating` int NOT NULL,
  `Comment` text
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `Services`
--

CREATE TABLE `Services` (
  `ServiceID` int NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Cost` decimal(10,2) NOT NULL,
  `Duration` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Cars`
--
ALTER TABLE `Cars`
  ADD PRIMARY KEY (`CarID`),
  ADD KEY `ClientID` (`ClientID`);

--
-- Индексы таблицы `Clients`
--
ALTER TABLE `Clients`
  ADD PRIMARY KEY (`ClientID`);

--
-- Индексы таблицы `OrderedServices`
--
ALTER TABLE `OrderedServices`
  ADD PRIMARY KEY (`OrderID`,`ServiceID`),
  ADD KEY `ServiceID` (`ServiceID`);

--
-- Индексы таблицы `Orders`
--
ALTER TABLE `Orders`
  ADD PRIMARY KEY (`OrderID`),
  ADD KEY `ClientID` (`ClientID`),
  ADD KEY `CarID` (`CarID`);

--
-- Индексы таблицы `OrderStatusHistory`
--
ALTER TABLE `OrderStatusHistory`
  ADD PRIMARY KEY (`HistoryID`),
  ADD KEY `OrderID` (`OrderID`);

--
-- Индексы таблицы `Reviews`
--
ALTER TABLE `Reviews`
  ADD PRIMARY KEY (`ReviewID`),
  ADD KEY `OrderID` (`OrderID`);

--
-- Индексы таблицы `Services`
--
ALTER TABLE `Services`
  ADD PRIMARY KEY (`ServiceID`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Cars`
--
ALTER TABLE `Cars`
  MODIFY `CarID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Clients`
--
ALTER TABLE `Clients`
  MODIFY `ClientID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Orders`
--
ALTER TABLE `Orders`
  MODIFY `OrderID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `OrderStatusHistory`
--
ALTER TABLE `OrderStatusHistory`
  MODIFY `HistoryID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Reviews`
--
ALTER TABLE `Reviews`
  MODIFY `ReviewID` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `Services`
--
ALTER TABLE `Services`
  MODIFY `ServiceID` int NOT NULL AUTO_INCREMENT;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `Cars`
--
ALTER TABLE `Cars`
  ADD CONSTRAINT `cars_ibfk_1` FOREIGN KEY (`ClientID`) REFERENCES `Clients` (`ClientID`);

--
-- Ограничения внешнего ключа таблицы `OrderedServices`
--
ALTER TABLE `OrderedServices`
  ADD CONSTRAINT `orderedservices_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `Orders` (`OrderID`),
  ADD CONSTRAINT `orderedservices_ibfk_2` FOREIGN KEY (`ServiceID`) REFERENCES `Services` (`ServiceID`);

--
-- Ограничения внешнего ключа таблицы `Orders`
--
ALTER TABLE `Orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ClientID`) REFERENCES `Clients` (`ClientID`),
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`CarID`) REFERENCES `Cars` (`CarID`);

--
-- Ограничения внешнего ключа таблицы `OrderStatusHistory`
--
ALTER TABLE `OrderStatusHistory`
  ADD CONSTRAINT `orderstatushistory_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `Orders` (`OrderID`);

--
-- Ограничения внешнего ключа таблицы `Reviews`
--
ALTER TABLE `Reviews`
  ADD CONSTRAINT `reviews_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `Orders` (`OrderID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
