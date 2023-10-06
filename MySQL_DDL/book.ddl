CREATE TABLE `book` (
  `pkey` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` varchar(500) NOT NULL,
  `initStock` int NOT NULL,
  `currentStock` int NOT NULL,
  `author` varchar(45) NOT NULL,
  `isbn` varchar(45) NOT NULL,
  PRIMARY KEY (`pkey`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci