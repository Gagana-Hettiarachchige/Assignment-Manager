/* 
* FILE          : AssignmentDatabase.sql
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-17
* DESCRIPTION   :
*	This file holds the assignments database.
*/

DROP DATABASE IF EXISTS AM;
CREATE DATABASE AM;
USE AM;



/* Holds assigment details. */
CREATE TABLE Assignments
(
    AssignmentNumber INT NOT NULL AUTO_INCREMENT,
    ClassName VARCHAR(30) NOT NULL,
    AssignmentName VARCHAR(30) NOT NULL,
    AssignmentWeight FLOAT(10) NOT NULL,
    DueDate DATETIME NOT NULL,
    AssignmentStatus VARCHAR(30) NOT NULL,
    LocalResources VARCHAR(5000) NULL,
    OnlineResources VARCHAR(5000) NULL,
    PRIMARY KEY (AssignmentNumber)
);


/* Creating user to access assignment manager database from code. */
DROP USER IF EXISTS "AssignmentManager"@"localhost";
CREATE USER "AssignmentManager"@"localhost" IDENTIFIED BY "manage";
GRANT ALL PRIVILEGES ON AM.* TO "AssignmentManager"@"localhost";