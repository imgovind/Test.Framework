1) Change the host part of the connection strings for both MySql and MongoDb
2) Make sure to create the database in MySql and the corresponding table.
3) Be sure to change the application parameter in the Database Target in NLog.config.
4) Also make sure that the <extensions></extensions> element comes before the <targets></targets> element.

Creating NLog Database
-- --------------------------------------------------------
-- Host:                         ec2-54-148-74-148.us-west-2.compute.amazonaws.com
-- Server version:               10.0.14-MariaDB - MariaDB Server
-- Server OS:                    Linux
-- HeidiSQL Version:             9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for NLog
CREATE DATABASE IF NOT EXISTS `NLog` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `NLog`;


-- Dumping structure for table NLog.ErrorsLog
CREATE TABLE IF NOT EXISTS `ErrorsLog` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `LogId` char(36) DEFAULT '',
  `IsInMongo` tinyint(1) DEFAULT '1',
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `LogDate` datetime DEFAULT NULL,
  `Application` varchar(500) DEFAULT NULL,
  `Level` varchar(500) DEFAULT NULL,
  `Logger` mediumtext,
  `Message` mediumtext,
  `MachineName` mediumtext,
  `UserName` mediumtext,
  `CallSite` mediumtext,
  `Thread` varchar(500) DEFAULT NULL,
  `Exception` mediumtext,
  `StackTrace` mediumtext,
  PRIMARY KEY (`Id`),
  KEY `LoggingId` (`LogId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for trigger NLog.before_insert_NLog_ErrorsLog_LogId
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='';
DELIMITER //
CREATE TRIGGER `before_insert_NLog_ErrorsLog_LogId` BEFORE INSERT ON `ErrorsLog` FOR EACH ROW SET new.LogId = uuid()//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
