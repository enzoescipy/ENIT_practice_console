CREATE TABLE `log` (
  `pkey` int NOT NULL AUTO_INCREMENT,
  `userPkey` int NOT NULL,
  `bookPkey` int NOT NULL,
  `isBorrow` tinyint NOT NULL,
  `time` varchar(100) NOT NULL,
  `count` int NOT NULL,
  `returnLeft` int NOT NULL,
  `logOrder` int NOT NULL,
  PRIMARY KEY (`pkey`),
  KEY `userPkey_idx` (`userPkey`),
  KEY `bookPkey_idx` (`bookPkey`),
  CONSTRAINT `bookPkey` FOREIGN KEY (`bookPkey`) REFERENCES `book` (`pkey`),
  CONSTRAINT `userPkey` FOREIGN KEY (`userPkey`) REFERENCES `user` (`pkey`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci