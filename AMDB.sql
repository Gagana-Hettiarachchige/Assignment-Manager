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
    CourseWeight FLOAT(10) NOT NULL,
    DueDate DATETIME NOT NULL,
    SolutionPath VARCHAR(500) NULL,
    RepositoryLink VARCHAR(500) NULL,
    DropboxLink VARCHAR(500) NULL,
    RequirementsPage VARCHAR(500) NULL,
    PRIMARY KEY (AssignmentNumber)
);