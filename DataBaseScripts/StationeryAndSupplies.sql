-- Script for creating and inserting data

-- CREATE DATABASE StationeryAndSuppliesDatabase;

-- SHOW DATABASES; 
-- 
-- USE StationeryAndSuppliesDatabase; 
-- 
-- SHOW TABLES; 

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
-- 	CategoryID INT NOT NULL, 
-- 
-- 	FOREIGN KEY (CategoryID) REFERENCES categories (CategoryID)			
-- ); 


-- Need to add password, role column later when adding identity
-- CREATE TABLE users (
-- 	UserID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	Name VARCHAR(50) NOT NULL,
-- 	Email VARCHAR(100) NOT NULL UNIQUE, 
-- 	Phone VARCHAR(100) NULL, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
-- ); 


-- CREATE TABLE reviews (
-- 	ReviewID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Rating TINYINT NOT NULL CHECK (Rating > 0 AND Rating <= 5), 
-- 	Comment TEXT NULL, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 


-- CREATE TABLE cart (
-- 	CartID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID)
-- ); 


-- CREATE TABLE cart_items (
-- 	CartItemID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	CartID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Quantity INT NOT NULL CHECK (Quantity > 0), 
-- 	
-- 	FOREIGN KEY (CartID) REFERENCES cart (CartID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 



-- CREATE TABLE orders (
-- 	OrderID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL, 
-- 	TotalAmount DECIMAL NOT NULL CHECK (TotalAmount >= 0),
-- 	Status ENUM('pending', 'shipped', 'cancelled', 'delivered'), 
-- 	OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, 
-- 	ShippingAddress TEXT NOT NULL,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID)
-- ); 


-- CREATE TABLE order_items (
-- 	OrderItemID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	OrderID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Quantity INT NOT NULL CHECK (Quantity > 0), 
-- 	UnitPrice DECIMAL NOT NULL CHECK (UnitPrice >= 0), 
-- 	
-- 	FOREIGN KEY (OrderID) REFERENCES orders (OrderID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 


-- CREATE TABLE payments (
-- 	PaymentID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	OrderID INT NOT NULL UNIQUE, 
-- 	Amount DECIMAL NOT NULL CHECK (Amount >= 0), 
-- 	PymentMethod VARCHAR(50) NOT NULL, 
-- 	Status ENUM('success', 'failed'), 
-- 	PayedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (OrderID) REFERENCES orders (OrderID)
-- ); 



-- Inserting Data
-- Top-level categories
-- INSERT INTO categories (ParentID, Name) VALUES
-- (NULL, 'Writing Instruments'),
-- (NULL, 'Paper Products'),
-- (NULL, 'Desk Accessories'),
-- (NULL, 'Art Supplies');
-- 
-- -- Subcategories of 'Writing Instruments' (assume ID 1)
-- INSERT INTO categories (ParentID, Name) VALUES
-- (1, 'Pens'),
-- (1, 'Pencils'),
-- (1, 'Markers');
-- 
-- -- Subcategories of 'Paper Products' (assume ID 2)
-- INSERT INTO categories (ParentID, Name) VALUES
-- (2, 'Notebooks'),
-- (2, 'Sticky Notes'),
-- (2, 'Printer Paper');
-- 
-- -- Subcategories of 'Desk Accessories' (assume ID 3)
-- INSERT INTO categories (ParentID, Name) VALUES
-- (3, 'Staplers'),
-- (3, 'Paper Clips'),
-- (3, 'Tape Dispensers');
-- 
-- -- Subcategories of 'Art Supplies' (assume ID 4)
-- INSERT INTO categories (ParentID, Name) VALUES
-- (4, 'Paint Brushes'),
-- (4, 'Sketchbooks'),
-- (4, 'Color Pencils');


-- SELECT 
-- 	C2.Name AS ParentCategory,
-- 	C1.Name AS Category
-- FROM categories C1
-- INNER JOIN categories C2
-- 	ON C1.ParentID = C2.CategoryID; 
















