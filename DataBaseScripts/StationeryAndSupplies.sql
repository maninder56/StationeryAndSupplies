-- Script for creating and inserting data

-- CREATE DATABASE StationeryAndSuppliesDatabase;

USE StationeryAndSuppliesDatabase; 

-- Creating Tables 

-- CREATE TABLE categories (
-- 	CategoryID INT PRIMARY KEY AUTO_INCREMENT,
-- 	ParentID INT NULL,
-- 	Name VARCHAR(100) NOT NULL
-- ); 


-- CREATE TABLE products (
-- 	ProductID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	Name VARCHAR(200) NOT NULL, 
-- 	Descripttion TEXT NULL, 
-- 	Price DECIMAL(10,2) CHECK (Price >= 0.01),
-- 	Stock INT NOT NULL CHECK (Stock >= 0),
-- 	Status ENUM('active', 'inactive', 'archived'),
-- 	ImageUrl VARCHAR(200) NULL, 
-- 	CreatedAt DATE DEFAULT (CURRENT_DATE()), 
-- 	
-- 	CategoryID INT NOT NULL, 
-- 	FOREIGN KEY (CategoryID)
-- 		REFERENCES categories (CategoryID)			
-- ); 




